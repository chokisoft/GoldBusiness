using GoldBusiness.Application.Interfaces;
using GoldBusiness.Domain.DTOs;
using GoldBusiness.Domain.Entities;
using GoldBusiness.Infrastructure.Context;
using GoldBusiness.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace GoldBusiness.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _config;
        private readonly ApplicationDbContext _context;
        private readonly ILogger<AuthService> _logger;

        public AuthService(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration config,
            ApplicationDbContext context,
            ILogger<AuthService> logger)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _config = config;
            _context = context;
            _logger = logger;
        }

        public async Task<AuthResponseDTO?> AuthenticateAsync(LoginDTO login)
        {
            try
            {
                // 1. Validar usuario
                var loginIdentifier = login.Username?.Trim() ?? string.Empty;
                var user = await _userManager.FindByNameAsync(loginIdentifier);

                if (user == null)
                {
                    user = await _userManager.FindByEmailAsync(loginIdentifier);
                }

                if (user == null || !user.IsActive)
                {
                    _logger.LogWarning("Intento de login fallido para usuario: {Username} desde IP: {IpAddress}", 
                        loginIdentifier, login.IpAddress);
                    
                    return new AuthResponseDTO
                    {
                        Succeeded = false,
                        Message = "Usuario o contraseña incorrectos",
                        Data = null
                    };
                }

                // 2. Validar contraseña
                var validPassword = await _userManager.CheckPasswordAsync(user, login.Password);
                if (!validPassword)
                {
                    _logger.LogWarning("Contraseña incorrecta para usuario: {Username} desde IP: {IpAddress}", 
                        loginIdentifier, login.IpAddress);
                    
                    await _userManager.AccessFailedAsync(user); // Incrementa intentos fallidos
                    
                    return new AuthResponseDTO
                    {
                        Succeeded = false,
                        Message = "Usuario o contraseña incorrectos",
                        Data = null
                    };
                }

                // 3. Verificar si la cuenta está bloqueada
                if (await _userManager.IsLockedOutAsync(user))
                {
                    _logger.LogWarning("Intento de login en cuenta bloqueada: {Username}", loginIdentifier);
                    
                    return new AuthResponseDTO
                    {
                        Succeeded = false,
                        Message = "Cuenta temporalmente bloqueada. Intente más tarde.",
                        Data = null
                    };
                }

                // 4. Resetear contador de intentos fallidos
                await _userManager.ResetAccessFailedCountAsync(user);

                // 5. Revocar tokens antiguos del usuario (mantener solo los últimos 5)
                await RevokeOldUserTokensAsync(user.Id, login.IpAddress);

                // 6. Generar tokens
                var (accessToken, expiresAt) = await GenerateAccessTokenAsync(user);
                var refreshToken = await GenerateAndStoreRefreshTokenAsync(
                    user.Id, 
                    login.IpAddress, 
                    login.UserAgent);

                // 7. Obtener información del usuario
                var userInfo = await BuildUserInfoAsync(user);

                _logger.LogInformation("Login exitoso para usuario: {Username} desde IP: {IpAddress}", 
                    loginIdentifier, login.IpAddress);

                // 8. Construir respuesta
                return new AuthResponseDTO
                {
                    Succeeded = true,
                    Message = "Autenticación exitosa",
                    Data = new AuthDataDTO
                    {
                        Token = accessToken,
                        RefreshToken = refreshToken,
                        ExpiresAt = expiresAt.ToString("o"),
                        User = userInfo
                    }
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error durante autenticación para usuario: {Username}", login.Username?.Trim());
                throw;
            }
        }

        public async Task<AuthResponseDTO?> AuthenticateExternalUserAsync(string userId, string? ipAddress, string? userAgent)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null || !user.IsActive)
                {
                    _logger.LogWarning("Login externo fallido para userId: {UserId} desde IP: {IpAddress}", userId, ipAddress);
                    return new AuthResponseDTO
                    {
                        Succeeded = false,
                        Message = "Usuario no autorizado para acceso externo",
                        Data = null
                    };
                }

                await RevokeOldUserTokensAsync(user.Id, ipAddress);

                var (accessToken, expiresAt) = await GenerateAccessTokenAsync(user);
                var refreshToken = await GenerateAndStoreRefreshTokenAsync(
                    user.Id,
                    ipAddress,
                    userAgent);

                var userInfo = await BuildUserInfoAsync(user);

                _logger.LogInformation("Login externo exitoso para usuario: {Username} desde IP: {IpAddress}",
                    user.UserName, ipAddress);

                return new AuthResponseDTO
                {
                    Succeeded = true,
                    Message = "Autenticación externa exitosa",
                    Data = new AuthDataDTO
                    {
                        Token = accessToken,
                        RefreshToken = refreshToken,
                        ExpiresAt = expiresAt.ToString("o"),
                        User = userInfo
                    }
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error durante autenticación externa para userId: {UserId}", userId);
                throw;
            }
        }

        public async Task<AuthResponseDTO?> RefreshTokenAsync(string refreshToken, string? ipAddress, string? userAgent)
        {
            try
            {
                // 1. Buscar refresh token en BD
                var storedToken = await _context.RefreshToken
                    .Include(rt => rt.User)
                    .FirstOrDefaultAsync(rt => rt.Token == refreshToken);

                if (storedToken == null || !storedToken.IsActive)
                {
                    _logger.LogWarning("Intento de refresh con token inválido desde IP: {IpAddress}", ipAddress);
                    return null;
                }

                // 2. Revocar token usado (rotación de tokens)
                await RevokeTokenInternalAsync(storedToken, ipAddress, "Reemplazado por refresh");

                // 3. Generar nuevos tokens
                var user = storedToken.User!;
                var (accessToken, expiresAt) = await GenerateAccessTokenAsync(user);
                var newRefreshToken = await GenerateAndStoreRefreshTokenAsync(
                    user.Id, 
                    ipAddress, 
                    userAgent,
                    storedToken.Token); // Guardar token anterior

                // 4. Obtener información del usuario
                var userInfo = await BuildUserInfoAsync(user);

                _logger.LogInformation("Refresh token exitoso para usuario: {UserId} desde IP: {IpAddress}", 
                    user.Id, ipAddress);

                return new AuthResponseDTO
                {
                    Succeeded = true,
                    Message = "Token renovado exitosamente",
                    Data = new AuthDataDTO
                    {
                        Token = accessToken,
                        RefreshToken = newRefreshToken,
                        ExpiresAt = expiresAt.ToString("o"),
                        User = userInfo
                    }
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error durante refresh token desde IP: {IpAddress}", ipAddress);
                throw;
            }
        }

        public async Task<bool> RevokeTokenAsync(string token, string? ipAddress)
        {
            var refreshToken = await _context.RefreshToken
                .FirstOrDefaultAsync(rt => rt.Token == token);

            if (refreshToken == null || !refreshToken.IsActive)
                return false;

            await RevokeTokenInternalAsync(refreshToken, ipAddress, "Revocado manualmente");
            
            _logger.LogInformation("Token revocado manualmente desde IP: {IpAddress}", ipAddress);
            return true;
        }

        public async Task<bool> RevokeAllUserTokensAsync(string userId, string? ipAddress)
        {
            var activeTokens = await _context.RefreshToken
                .Where(rt => rt.UserId == userId && rt.RevokedAt == null && rt.ExpiresAt > DateTime.UtcNow)
                .ToListAsync();

            foreach (var token in activeTokens)
            {
                await RevokeTokenInternalAsync(token, ipAddress, "Revocación masiva");
            }

            _logger.LogInformation("Todos los tokens del usuario {UserId} fueron revocados desde IP: {IpAddress}", 
                userId, ipAddress);

            return true;
        }

        public async Task<int> CleanExpiredTokensAsync()
        {
            var expiredTokens = await _context.RefreshToken
                .Where(rt => rt.ExpiresAt < DateTime.UtcNow)
                .ToListAsync();

            _context.RefreshToken.RemoveRange(expiredTokens);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Se limpiaron {Count} refresh tokens expirados", expiredTokens.Count);
            return expiredTokens.Count;
        }

        #region Métodos Privados

        private async Task<(string token, DateTime expiresAt)> GenerateAccessTokenAsync(ApplicationUser user)
        {
            // 1. Claims del usuario
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);

            // 2. Claims de roles
            var roleClaims = new List<Claim>();
            foreach (var roleName in roles)
            {
                roleClaims.Add(new Claim(ClaimTypes.Role, roleName));

                var roleObj = await _roleManager.FindByNameAsync(roleName);
                if (roleObj != null)
                {
                    var roleClaimsFromDb = await _roleManager.GetClaimsAsync(roleObj);
                    roleClaims.AddRange(roleClaimsFromDb);
                }
            }

            // 3. Claims base del token
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName ?? ""),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email ?? ""),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()) // ID único del token
            };

            claims.AddRange(userClaims);
            claims.AddRange(roleClaims);

            // 4. Configuración JWT
            var jwtKey = _config["Jwt:Key"]
                ?? throw new InvalidOperationException("JWT Key no configurada");
            var jwtIssuer = _config["Jwt:Issuer"]
                ?? throw new InvalidOperationException("JWT Issuer no configurado");
            var jwtAudience = _config["Jwt:Audience"] ?? jwtIssuer;
            var expirationMinutes = _config.GetValue<int>("Jwt:AccessTokenExpirationMinutes", 30);

            // 5. Generar token
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expiresAt = DateTime.UtcNow.AddMinutes(expirationMinutes);

            var token = new JwtSecurityToken(
                issuer: jwtIssuer,
                audience: jwtAudience,
                claims: claims,
                expires: expiresAt,
                signingCredentials: creds);

            var accessToken = new JwtSecurityTokenHandler().WriteToken(token);

            return (accessToken, expiresAt);
        }

        private async Task<string> GenerateAndStoreRefreshTokenAsync(
            string userId, 
            string? ipAddress, 
            string? userAgent,
            string? replacedToken = null)
        {
            var refreshToken = new RefreshToken
            {
                UserId = userId,
                Token = GenerateSecureToken(),
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddDays(_config.GetValue<int>("Jwt:RefreshTokenExpirationDays", 7)),
                CreatedByIp = ipAddress,
                UserAgent = userAgent,
                ReplacedByToken = replacedToken
            };

            _context.RefreshToken.Add(refreshToken);
            await _context.SaveChangesAsync();

            return refreshToken.Token;
        }

        private static string GenerateSecureToken()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        private async Task RevokeTokenInternalAsync(RefreshToken token, string? ipAddress, string reason)
        {
            token.RevokedAt = DateTime.UtcNow;
            token.RevokedByIp = ipAddress;
            
            _context.RefreshToken.Update(token);
            await _context.SaveChangesAsync();
        }

        private async Task RevokeOldUserTokensAsync(string userId, string? ipAddress)
        {
            // Mantener solo los últimos 5 tokens activos por usuario
            var userTokens = await _context.RefreshToken
                .Where(rt => rt.UserId == userId && rt.RevokedAt == null)
                .OrderByDescending(rt => rt.CreatedAt)
                .ToListAsync();

            var tokensToRevoke = userTokens.Skip(5).ToList();
            
            foreach (var token in tokensToRevoke)
            {
                await RevokeTokenInternalAsync(token, ipAddress, "Límite de sesiones excedido");
            }
        }

        private async Task<UserInfoDTO> BuildUserInfoAsync(ApplicationUser user)
        {
            var roles = await _userManager.GetRolesAsync(user);
            var userClaims = await _userManager.GetClaimsAsync(user);

            var roleClaims = new List<Claim>();
            foreach (var roleName in roles)
            {
                var roleObj = await _roleManager.FindByNameAsync(roleName);
                if (roleObj != null)
                {
                    roleClaims.AddRange(await _roleManager.GetClaimsAsync(roleObj));
                }
            }

            var allClaims = userClaims.Concat(roleClaims).ToList();
            var fullName = allClaims.FirstOrDefault(c => c.Type == "fullName")?.Value ?? user.UserName ?? string.Empty;
            var permissions = allClaims
                .Where(c => c.Type == "permission")
                .Select(c => c.Value)
                .Where(v => !string.IsNullOrWhiteSpace(v))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();

            var accessLevels = allClaims
                .Where(c => c.Type == "accessLevel")
                .Select(c => c.Value)
                .Where(v => !string.IsNullOrWhiteSpace(v))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();

            if (accessLevels.Count == 0)
            {
                accessLevels.Add("*");
            }

            var authProvider = allClaims
                .FirstOrDefault(c => c.Type == "authProvider")?.Value;

            return new UserInfoDTO
            {
                UserName = user.UserName ?? string.Empty,
                Email = user.Email ?? string.Empty,
                FullName = fullName,
                Roles = roles.ToList(),
                Permissions = permissions,
                AccessLevels = accessLevels,
                AuthProvider = string.IsNullOrWhiteSpace(authProvider) ? "Local" : authProvider
            };
        }

        #endregion
    }
}
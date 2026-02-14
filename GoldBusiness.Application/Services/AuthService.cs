using GoldBusiness.Application.Interfaces;
using GoldBusiness.Domain.DTOs;
using GoldBusiness.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace GoldBusiness.Application.Services
{
    public class AuthService(
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager,
        IConfiguration config) : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly RoleManager<IdentityRole> _roleManager = roleManager;
        private readonly IConfiguration _config = config;

        public async Task<AuthResponseDTO?> AuthenticateAsync(LoginDTO login)
        {
            // Validar usuario
            var user = await _userManager.FindByNameAsync(login.Username);
            if (user == null || !user.IsActive)
            {
                return new AuthResponseDTO
                {
                    Succeeded = false,
                    Message = "Usuario o contraseña incorrectos",
                    Data = null
                };
            }

            // Validar contraseña
            var validPassword = await _userManager.CheckPasswordAsync(user, login.Password);
            if (!validPassword)
            {
                return new AuthResponseDTO
                {
                    Succeeded = false,
                    Message = "Usuario o contraseña incorrectos",
                    Data = null
                };
            }

            // 1. Claims del usuario
            var userClaims = await _userManager.GetClaimsAsync(user);

            // 2. Roles del usuario
            var roles = await _userManager.GetRolesAsync(user);

            // 3. Claims de roles
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

            // 4. Claims base del token
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName ?? ""),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email ?? "")
            };

            // 5. Agregar claims del usuario y del rol
            claims.AddRange(userClaims);
            claims.AddRange(roleClaims);

            // 6. Validar configuración JWT
            var jwtKey = _config["Jwt:Key"]
                ?? throw new InvalidOperationException("JWT Key is not configured in appsettings.json");
            var jwtIssuer = _config["Jwt:Issuer"]
                ?? throw new InvalidOperationException("JWT Issuer is not configured in appsettings.json");
            var expirationMinutes = _config.GetValue<int>("Jwt:AccessTokenExpirationMinutes", 30);

            // 7. Generar token de acceso
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expiresAt = DateTime.UtcNow.AddMinutes(expirationMinutes);

            var token = new JwtSecurityToken(
                issuer: jwtIssuer,
                audience: null,
                claims: claims,
                expires: expiresAt,
                signingCredentials: creds);

            var accessToken = new JwtSecurityTokenHandler().WriteToken(token);

            // 8. Generar refresh token
            var refreshToken = GenerateRefreshToken();

            // 9. Obtener el nombre completo del claim "fullName" si existe
            var fullNameClaim = userClaims.FirstOrDefault(c => c.Type == "fullName");
            var fullName = fullNameClaim?.Value ?? user.UserName ?? "";

            // 10. Construir respuesta
            return new AuthResponseDTO
            {
                Succeeded = true,
                Message = "Autenticación exitosa",
                Data = new AuthDataDTO
                {
                    Token = accessToken,
                    RefreshToken = refreshToken,
                    ExpiresAt = expiresAt.ToString("o"), // Formato ISO 8601
                    User = new UserInfoDTO
                    {
                        UserName = user.UserName ?? "",
                        Email = user.Email ?? "",
                        FullName = fullName,
                        Roles = roles.ToList()
                    }
                }
            };
        }

        /// <summary>
        /// Genera un refresh token aleatorio seguro.
        /// </summary>
        private static string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }
}
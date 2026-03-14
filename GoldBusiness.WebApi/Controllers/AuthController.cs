using GoldBusiness.Application.Interfaces;
using GoldBusiness.Domain.DTOs;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace GoldBusiness.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;
        private readonly UserManager<GoldBusiness.Domain.Entities.ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;

        public AuthController(
            IAuthService authService,
            ILogger<AuthController> logger,
            UserManager<GoldBusiness.Domain.Entities.ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration)
        {
            _authService = authService;
            _logger = logger;
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        /// <summary>
        /// Login de usuario
        /// </summary>
        [HttpPost("login")]
        [AllowAnonymous]
        [EnableRateLimiting("auth")]
        [ProducesResponseType(typeof(AuthResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        public async Task<IActionResult> Login([FromBody] LoginDTO login)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new AuthResponseDTO
                    {
                        Succeeded = false,
                        Message = "Datos de entrada inválidos",
                        Data = null
                    });
                }

                login.IpAddress = GetClientIpAddress();
                login.UserAgent = Request.Headers["User-Agent"].ToString();

                var result = await _authService.AuthenticateAsync(login);

                if (result == null || !result.Succeeded)
                {
                    return BadRequest(result);
                }

                SetRefreshTokenCookie(result.Data!.RefreshToken);

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en endpoint de login");
                return StatusCode(500, new AuthResponseDTO
                {
                    Succeeded = false,
                    Message = "Error interno del servidor",
                    Data = null
                });
            }
        }

        /// <summary>
        /// Iniciar autenticación con Google
        /// </summary>
        [HttpGet("google/login")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status302Found)]
        public IActionResult GoogleLogin([FromQuery] string? returnUrl = null)
        {
            var safeReturnUrl = ResolveReturnUrl(returnUrl);

            var properties = new AuthenticationProperties
            {
                // ✅ NO especificamos RedirectUri - se usará el CallbackPath de Program.cs automáticamente
                Items =
                {
                    ["returnUrl"] = safeReturnUrl
                }
            };

            _logger.LogInformation("🔐 Iniciando flujo de Google OAuth. ReturnUrl: {ReturnUrl}", safeReturnUrl);

            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }

        /// <summary>
        /// Callback de Google OAuth (ruta absoluta que coincide con CallbackPath en Program.cs)
        /// </summary>
        [HttpGet("/api/auth/google/callback")]  // ✅ Cambiado de /signin-google
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status302Found)]
        public async Task<IActionResult> GoogleCallback()
        {
            _logger.LogInformation("📥 Google callback recibido");

            var authResult = await HttpContext.AuthenticateAsync(IdentityConstants.ExternalScheme);
            if (!authResult.Succeeded || authResult.Principal == null)
            {
                _logger.LogWarning("❌ Google authentication failed");
                var defaultReturnUrl = ResolveReturnUrl(null);
                return Redirect(BuildErrorReturnUrl(defaultReturnUrl, "google_auth_failed"));
            }

            // ✅ Recuperar el returnUrl de las propiedades (con inicialización segura)
            string? returnUrl = null;
            authResult.Properties?.Items.TryGetValue("returnUrl", out returnUrl);
            var safeReturnUrl = ResolveReturnUrl(returnUrl);

            var email = authResult.Principal.FindFirstValue(ClaimTypes.Email)?.Trim();
            if (string.IsNullOrWhiteSpace(email))
            {
                _logger.LogWarning("❌ Email no encontrado en claims de Google");
                await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
                return Redirect(BuildErrorReturnUrl(safeReturnUrl, "google_email_not_found"));
            }

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                _logger.LogWarning("❌ Usuario no encontrado para email: {Email}", email);
                await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
                return Redirect(BuildErrorReturnUrl(safeReturnUrl, "google_user_not_found"));
            }

            var userClaims = await _userManager.GetClaimsAsync(user);
            var authProvider = userClaims.FirstOrDefault(c => c.Type == "authProvider")?.Value ?? "Local";
            if (!string.Equals(authProvider, "Google", StringComparison.OrdinalIgnoreCase))
            {
                _logger.LogWarning("❌ Usuario {Email} no está configurado para autenticación Google", email);
                await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
                return Redirect(BuildErrorReturnUrl(safeReturnUrl, "google_provider_not_allowed"));
            }

            var providerKey = authResult.Principal.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!string.IsNullOrWhiteSpace(providerKey))
            {
                var logins = await _userManager.GetLoginsAsync(user);
                var hasGoogleLogin = logins.Any(l =>
                    l.LoginProvider.Equals(GoogleDefaults.AuthenticationScheme, StringComparison.OrdinalIgnoreCase));

                if (!hasGoogleLogin)
                {
                    var addLoginResult = await _userManager.AddLoginAsync(user,
                        new UserLoginInfo(GoogleDefaults.AuthenticationScheme, providerKey, GoogleDefaults.AuthenticationScheme));

                    if (!addLoginResult.Succeeded)
                    {
                        _logger.LogError("❌ Error al vincular login de Google para {Email}", email);
                        await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
                        return Redirect(BuildErrorReturnUrl(safeReturnUrl, "google_link_failed"));
                    }

                    _logger.LogInformation("✅ Login de Google vinculado exitosamente para {Email}", email);
                }
            }

            user.EmailConfirmed = true;
            await _userManager.UpdateAsync(user);

            var result = await _authService.AuthenticateExternalUserAsync(
                user.Id,
                GetClientIpAddress(),
                Request.Headers["User-Agent"].ToString());

            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            if (result?.Succeeded != true || result.Data == null)
            {
                _logger.LogError("❌ Error al generar token JWT para usuario {Email}", email);
                return Redirect(BuildErrorReturnUrl(safeReturnUrl, "google_token_failed"));
            }

            _logger.LogInformation("✅ Login de Google exitoso para {Email}", email);

            var redirectUrl = BuildSuccessReturnUrl(safeReturnUrl, result.Data.Token, result.Data.RefreshToken, result.Data.ExpiresAt);
            return Redirect(redirectUrl);
        }

        /// <summary>
        /// Renovar access token usando refresh token
        /// </summary>
        [HttpPost("refresh")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(AuthResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            try
            {
                var refreshToken = request.RefreshToken ?? Request.Cookies["refreshToken"];

                if (string.IsNullOrEmpty(refreshToken))
                {
                    return BadRequest(new { message = "Refresh token requerido" });
                }

                var ipAddress = GetClientIpAddress();
                var userAgent = Request.Headers["User-Agent"].ToString();

                var result = await _authService.RefreshTokenAsync(refreshToken, ipAddress, userAgent);

                if (result == null || !result.Succeeded)
                {
                    return Unauthorized(new { message = "Refresh token inválido o expirado" });
                }

                SetRefreshTokenCookie(result.Data!.RefreshToken);

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en endpoint de refresh token");
                return StatusCode(500, new { message = "Error interno del servidor" });
            }
        }

        /// <summary>
        /// Revocar refresh token actual
        /// </summary>
        [HttpPost("revoke")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RevokeToken([FromBody] RefreshTokenRequest request)
        {
            try
            {
                var refreshToken = request.RefreshToken ?? Request.Cookies["refreshToken"];

                if (string.IsNullOrEmpty(refreshToken))
                {
                    return BadRequest(new { message = "Refresh token requerido" });
                }

                var ipAddress = GetClientIpAddress();
                var result = await _authService.RevokeTokenAsync(refreshToken, ipAddress);

                if (!result)
                {
                    return BadRequest(new { message = "Token inválido o ya revocado" });
                }

                Response.Cookies.Delete("refreshToken");

                return Ok(new { message = "Token revocado exitosamente" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en endpoint de revoke token");
                return StatusCode(500, new { message = "Error interno del servidor" });
            }
        }

        /// <summary>
        /// Revocar todos los tokens del usuario actual (cerrar todas las sesiones)
        /// </summary>
        [HttpPost("revoke-all")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> RevokeAllTokens()
        {
            try
            {
                var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(new { message = "Usuario no autenticado" });
                }

                var ipAddress = GetClientIpAddress();
                await _authService.RevokeAllUserTokensAsync(userId, ipAddress);

                Response.Cookies.Delete("refreshToken");

                return Ok(new { message = "Todas las sesiones han sido cerradas" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en endpoint de revoke all tokens");
                return StatusCode(500, new { message = "Error interno del servidor" });
            }
        }

        [HttpGet("users/paged")]
        [Authorize(Policy = "ERPAdminAccess")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetUsersPaged(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? term = null)
        {
            var safePage = Math.Max(1, page);
            var safePageSize = Math.Clamp(pageSize, 1, 200);
            var normalizedTerm = term?.Trim().ToLowerInvariant();

            var query = _userManager.Users.AsQueryable();

            if (!string.IsNullOrWhiteSpace(normalizedTerm))
            {
                query = query.Where(u =>
                    (u.UserName ?? string.Empty).ToLower().Contains(normalizedTerm) ||
                    (u.Email ?? string.Empty).ToLower().Contains(normalizedTerm));
            }

            var total = await query.CountAsync();
            var users = await query
                .OrderBy(u => u.UserName)
                .Skip((safePage - 1) * safePageSize)
                .Take(safePageSize)
                .ToListAsync();

            var result = new List<UserAdminDTO>(users.Count);
            foreach (var user in users)
            {
                result.Add(await MapUserToAdminDtoAsync(user));
            }

            return Ok(new PagedResultDTO<UserAdminDTO>
            {
                Items = result,
                Total = total
            });
        }

        [HttpGet("users/{id}")]
        [Authorize(Policy = "ERPAdminAccess")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetUserById(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound(new { message = "Usuario no encontrado" });
            }

            return Ok(await MapUserToAdminDtoAsync(user));
        }

        [HttpGet("users/roles")]
        [Authorize(Policy = "ERPAdminAccess")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetAvailableRoles()
        {
            var roles = _roleManager.Roles
                .OrderBy(r => r.Name)
                .Select(r => r.Name!)
                .Where(name => !string.IsNullOrWhiteSpace(name))
                .ToList();

            return Ok(roles);
        }

        [HttpGet("users/permissions")]
        [Authorize(Policy = "ERPAdminAccess")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetAvailablePermissions()
        {
            var permissions = new[]
            {
                "ERP:FullAccess",
                "ERP:AdminAccess",
                "ERP:FinanceAccess",
                "ERP:AccountingAccess"
            };

            return Ok(permissions);
        }

        [HttpPost("users")]
        [Authorize(Policy = "ERPAdminAccess")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateUser([FromBody] UserAdminUpsertDTO dto)
        {
            var normalizedProvider = NormalizeAuthProvider(dto.AuthProvider);
            if (normalizedProvider == null)
            {
                return BadRequest(new { message = "AuthProvider inválido. Valores permitidos: Local, Google." });
            }

            if (normalizedProvider == "Local" && string.IsNullOrWhiteSpace(dto.Password))
            {
                return BadRequest(new { message = "La contraseña es obligatoria para usuarios Local." });
            }

            var existingByUserName = await _userManager.FindByNameAsync(dto.UserName);
            if (existingByUserName != null)
            {
                return BadRequest(new { message = "El nombre de usuario ya existe." });
            }

            var existingByEmail = await _userManager.FindByEmailAsync(dto.Email);
            if (existingByEmail != null)
            {
                return BadRequest(new { message = "El correo electrónico ya existe." });
            }

            var user = new GoldBusiness.Domain.Entities.ApplicationUser
            {
                UserName = dto.UserName.Trim(),
                Email = dto.Email.Trim(),
                EmailConfirmed = normalizedProvider == "Google",
                IsActive = dto.IsActive
            };

            var createResult = normalizedProvider == "Local"
                ? await _userManager.CreateAsync(user, dto.Password!)
                : await _userManager.CreateAsync(user);

            if (!createResult.Succeeded)
            {
                return BadRequest(new { message = string.Join("; ", createResult.Errors.Select(e => e.Description)) });
            }

            var assignmentError = await AssignRolesAndClaimsAsync(user, dto.Roles, dto.Permissions, dto.AccessLevels, dto.FullName, normalizedProvider);
            if (assignmentError != null)
            {
                await _userManager.DeleteAsync(user);
                return BadRequest(new { message = assignmentError });
            }

            var result = await MapUserToAdminDtoAsync(user);
            return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, result);
        }

        [HttpPut("users/{id}")]
        [Authorize(Policy = "ERPAdminAccess")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateUser(string id, [FromBody] UserAdminUpsertDTO dto)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound(new { message = "Usuario no encontrado" });
            }

            var normalizedProvider = NormalizeAuthProvider(dto.AuthProvider);
            if (normalizedProvider == null)
            {
                return BadRequest(new { message = "AuthProvider inválido. Valores permitidos: Local, Google." });
            }

            var usernameExists = await _userManager.FindByNameAsync(dto.UserName);
            if (usernameExists != null && usernameExists.Id != user.Id)
            {
                return BadRequest(new { message = "El nombre de usuario ya está en uso." });
            }

            var emailExists = await _userManager.FindByEmailAsync(dto.Email);
            if (emailExists != null && emailExists.Id != user.Id)
            {
                return BadRequest(new { message = "El correo electrónico ya está en uso." });
            }

            user.UserName = dto.UserName.Trim();
            user.Email = dto.Email.Trim();
            user.IsActive = dto.IsActive;
            user.EmailConfirmed = normalizedProvider == "Google" || user.EmailConfirmed;

            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
            {
                return BadRequest(new { message = string.Join("; ", updateResult.Errors.Select(e => e.Description)) });
            }

            if (!string.IsNullOrWhiteSpace(dto.Password))
            {
                var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
                var passwordResult = await _userManager.ResetPasswordAsync(user, resetToken, dto.Password);
                if (!passwordResult.Succeeded)
                {
                    return BadRequest(new { message = string.Join("; ", passwordResult.Errors.Select(e => e.Description)) });
                }
            }

            var assignmentError = await AssignRolesAndClaimsAsync(user, dto.Roles, dto.Permissions, dto.AccessLevels, dto.FullName, normalizedProvider);
            if (assignmentError != null)
            {
                return BadRequest(new { message = assignmentError });
            }

            return Ok(await MapUserToAdminDtoAsync(user));
        }

        [HttpDelete("users/{id}")]
        [Authorize(Policy = "ERPAdminAccess")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound(new { message = "Usuario no encontrado" });
            }

            user.IsActive = false;
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                return BadRequest(new { message = string.Join("; ", result.Errors.Select(e => e.Description)) });
            }

            return NoContent();
        }

        #region Métodos Privados

        private async Task<UserAdminDTO> MapUserToAdminDtoAsync(GoldBusiness.Domain.Entities.ApplicationUser user)
        {
            var roles = await _userManager.GetRolesAsync(user);
            var claims = await _userManager.GetClaimsAsync(user);

            return new UserAdminDTO
            {
                Id = user.Id,
                UserName = user.UserName ?? string.Empty,
                Email = user.Email ?? string.Empty,
                FullName = claims.FirstOrDefault(c => c.Type == "fullName")?.Value ?? user.UserName ?? string.Empty,
                Roles = roles.ToList(),
                Permissions = claims
                    .Where(c => c.Type == "permission")
                    .Select(c => c.Value)
                    .Where(v => !string.IsNullOrWhiteSpace(v))
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .ToList(),
                AccessLevels = claims
                    .Where(c => c.Type == "accessLevel")
                    .Select(c => c.Value)
                    .Where(v => !string.IsNullOrWhiteSpace(v))
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .ToList(),
                AuthProvider = claims.FirstOrDefault(c => c.Type == "authProvider")?.Value ?? "Local",
                IsActive = user.IsActive
            };
        }

        private async Task<string?> AssignRolesAndClaimsAsync(
            GoldBusiness.Domain.Entities.ApplicationUser user,
            IReadOnlyCollection<string> roles,
            IReadOnlyCollection<string> permissions,
            IReadOnlyCollection<string> accessLevels,
            string fullName,
            string authProvider)
        {
            var normalizedRoles = roles
                .Where(r => !string.IsNullOrWhiteSpace(r))
                .Select(r => r.Trim())
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();

            foreach (var role in normalizedRoles)
            {
                if (!await _roleManager.RoleExistsAsync(role))
                {
                    return $"El rol '{role}' no existe.";
                }
            }

            var currentRoles = await _userManager.GetRolesAsync(user);
            if (currentRoles.Any())
            {
                var removeRolesResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);
                if (!removeRolesResult.Succeeded)
                {
                    return string.Join("; ", removeRolesResult.Errors.Select(e => e.Description));
                }
            }

            if (normalizedRoles.Any())
            {
                var addRolesResult = await _userManager.AddToRolesAsync(user, normalizedRoles);
                if (!addRolesResult.Succeeded)
                {
                    return string.Join("; ", addRolesResult.Errors.Select(e => e.Description));
                }
            }

            var normalizedPermissions = permissions
                .Where(p => !string.IsNullOrWhiteSpace(p))
                .Select(p => p.Trim())
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();

            var normalizedAccessLevels = accessLevels
                .Where(a => !string.IsNullOrWhiteSpace(a))
                .Select(a => a.Trim().ToUpperInvariant())
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();

            if (!normalizedAccessLevels.Any())
            {
                normalizedAccessLevels.Add("*");
            }

            var currentClaims = await _userManager.GetClaimsAsync(user);
            var claimsToRemove = currentClaims.Where(c =>
                c.Type == "fullName" ||
                c.Type == "permission" ||
                c.Type == "accessLevel" ||
                c.Type == "authProvider").ToList();

            foreach (var claim in claimsToRemove)
            {
                var removeClaimResult = await _userManager.RemoveClaimAsync(user, claim);
                if (!removeClaimResult.Succeeded)
                {
                    return string.Join("; ", removeClaimResult.Errors.Select(e => e.Description));
                }
            }

            var claimsToAdd = new List<Claim>
            {
                new("fullName", fullName?.Trim() ?? user.UserName ?? string.Empty),
                new("authProvider", authProvider)
            };

            claimsToAdd.AddRange(normalizedPermissions.Select(permission => new Claim("permission", permission)));
            claimsToAdd.AddRange(normalizedAccessLevels.Select(level => new Claim("accessLevel", level)));

            foreach (var claim in claimsToAdd)
            {
                var addClaimResult = await _userManager.AddClaimAsync(user, claim);
                if (!addClaimResult.Succeeded)
                {
                    return string.Join("; ", addClaimResult.Errors.Select(e => e.Description));
                }
            }

            return null;
        }

        private static string? NormalizeAuthProvider(string? provider)
        {
            if (string.IsNullOrWhiteSpace(provider))
            {
                return null;
            }

            return provider.Trim().ToLowerInvariant() switch
            {
                "local" => "Local",
                "google" => "Google",
                _ => null
            };
        }

        private string ResolveReturnUrl(string? returnUrl)
        {
            if (!string.IsNullOrWhiteSpace(returnUrl)
                && Uri.TryCreate(returnUrl, UriKind.Absolute, out var parsed)
                && (parsed.Scheme == Uri.UriSchemeHttp || parsed.Scheme == Uri.UriSchemeHttps))
            {
                return parsed.ToString();
            }

            return _configuration["Authentication:Google:DefaultReturnUrl"]
                ?? "https://goldbusinessstorage.z19.web.core.windows.net/login?returnUrl=%2Fdashboard";
        }

        private static string BuildSuccessReturnUrl(string baseReturnUrl, string token, string refreshToken, string expiresAt)
        {
            var fragment =
                $"token={Uri.EscapeDataString(token)}" +
                $"&refreshToken={Uri.EscapeDataString(refreshToken)}" +
                $"&expiresAt={Uri.EscapeDataString(expiresAt)}";

            return $"{baseReturnUrl}#{fragment}";
        }

        private static string BuildErrorReturnUrl(string baseReturnUrl, string error)
        {
            return $"{baseReturnUrl}#error={Uri.EscapeDataString(error)}";
        }

        private string GetClientIpAddress()
        {
            var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();

            if (Request.Headers.ContainsKey("X-Forwarded-For"))
            {
                ipAddress = Request.Headers["X-Forwarded-For"].ToString().Split(',').FirstOrDefault();
            }
            else if (Request.Headers.ContainsKey("X-Real-IP"))
            {
                ipAddress = Request.Headers["X-Real-IP"].ToString();
            }

            return ipAddress ?? "Unknown";
        }

        private void SetRefreshTokenCookie(string refreshToken)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Lax, // ✅ Cambiado de Strict a Lax para Google OAuth
                Expires = DateTime.UtcNow.AddDays(7)
            };

            Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);
        }

        #endregion
    }
}
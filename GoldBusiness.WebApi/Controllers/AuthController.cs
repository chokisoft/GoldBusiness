using GoldBusiness.Application.Interfaces;
using GoldBusiness.Domain.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace GoldBusiness.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IAuthService authService, ILogger<AuthController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        /// <summary>
        /// Login de usuario
        /// </summary>
        [HttpPost("login")]
        [AllowAnonymous]
        [EnableRateLimiting("auth")] // Limitar a 5 intentos por minuto
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

                // Agregar metadata de seguridad
                login.IpAddress = GetClientIpAddress();
                login.UserAgent = Request.Headers["User-Agent"].ToString();

                var result = await _authService.AuthenticateAsync(login);

                if (result == null || !result.Succeeded)
                {
                    return BadRequest(result);
                }

                // Opcional: Configurar cookie HttpOnly para refresh token (más seguro)
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
                // Intentar obtener token de cookie si no viene en body
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

                // Actualizar cookie
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

                // Limpiar cookie
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

        #region Métodos Privados

        private string GetClientIpAddress()
        {
            // Obtener IP real considerando proxies/load balancers
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
                HttpOnly = true,  // No accesible desde JavaScript
                Secure = true,    // Solo HTTPS
                SameSite = SameSiteMode.Strict, // Protección CSRF
                Expires = DateTime.UtcNow.AddDays(7)
            };

            Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);
        }

        #endregion
    }
}
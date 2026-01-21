using GoldBusiness.Application.Interfaces;
using GoldBusiness.Domain.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Localization;

namespace GoldBusiness.WebApi.Controllers
{
    /// <summary>
    /// Controlador de autenticación para gestionar login y tokens JWT.
    /// Version 2.0 con rate limiting y seguridad mejorada.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IStringLocalizer<GoldBusiness.Domain.Resources.ValidationMessages> _localizer;
        private readonly ILogger<AuthController> _logger;

        public AuthController(
            IAuthService authService,
            IStringLocalizer<GoldBusiness.Domain.Resources.ValidationMessages> localizer,
            ILogger<AuthController> logger)
        {
            _authService = authService;
            _localizer = localizer;
            _logger = logger;
        }

        /// <summary>
        /// Autentica a un usuario y devuelve un token JWT.
        /// Rate limited: 10 intentos por minuto por IP.
        /// </summary>
        /// <param name="login">Credenciales del usuario</param>
        /// <returns>Token JWT y fecha de expiración</returns>
        [HttpPost("login")]
        [EnableRateLimiting("auth")]
        public async Task<IActionResult> Login([FromBody] LoginDTO login)
        {
            var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
            
            // ✅ Solo registrar IP (no el username) para evitar exposición
            _logger.LogDebug("Login attempt from IP: {IpAddress}", ipAddress);
            
            var result = await _authService.AuthenticateAsync(login);
            if (result == null)
            {
                var errorMessage = _localizer["InvalidCredentials"].Value ?? "Credenciales inválidas";

                // ✅ En caso de fallo, solo registrar IP (sin username)
                _logger.LogWarning("Failed login attempt from IP: {IpAddress}", ipAddress);

                return Unauthorized(new
                {
                    Message = errorMessage,
                    Status = 401,
                    Timestamp = DateTime.UtcNow
                });
            }

            // ✅ En caso de éxito, SÍ registrar el usuario (para auditoría)
            _logger.LogInformation("Successful login - User: {Username}, IP: {IpAddress}", 
                login.Username, ipAddress);

            return Ok(result);
        }
    }
}
using GoldBusiness.Application.Interfaces;
using GoldBusiness.Domain.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Localization;
using System.Globalization;

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
            // 🔍 DEBUG: Ver qué cultura está activa
            var acceptLanguage = Request.Headers["Accept-Language"].ToString();
            var currentUICulture = CultureInfo.CurrentUICulture.Name;
            var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
            
            _logger.LogInformation("🔐 Login attempt - User: {Username}, IP: {IpAddress}, Culture: {Culture}", 
                login.Username, ipAddress, currentUICulture);
            
            var result = await _authService.AuthenticateAsync(login);
            if (result == null)
            {
                var errorMessage = _localizer["InvalidCredentials"].Value ?? "Credenciales inválidas";

                _logger.LogWarning("🔒 Login fallido - Usuario: {Username}, IP: {IpAddress}, Idioma: {Culture}", 
                    login.Username, ipAddress, currentUICulture);

                return Unauthorized(new
                {
                    Message = errorMessage,
                    Code = "AUTH_001",
                    Status = 401,
                    Culture = currentUICulture,
                    AcceptLanguage = acceptLanguage,
                    Timestamp = DateTime.UtcNow
                });
            }

            _logger.LogInformation("✅ Login exitoso - Usuario: {Username}, IP: {IpAddress}", 
                login.Username, ipAddress);

            return Ok(result);
        }
    }
}
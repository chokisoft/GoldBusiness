using GoldBusiness.Application.Interfaces;
using GoldBusiness.Domain.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.Globalization;

namespace GoldBusiness.WebApi.Controllers
{
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

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO login)
        {
            // 🔍 DEBUG: Ver qué está pasando
            var acceptLanguage = Request.Headers["Accept-Language"].ToString();
            var currentUICulture = CultureInfo.CurrentUICulture.Name;
            
            _logger.LogWarning("🔍 Accept-Language: {AcceptLanguage}", acceptLanguage);
            _logger.LogWarning("🔍 CurrentUICulture: {CurrentUICulture}", currentUICulture);
            
            var testMessage = _localizer["InvalidCredentials"];
            _logger.LogWarning("🔍 Mensaje localizado: '{Message}' (ResourceNotFound: {NotFound})", 
                testMessage.Value, testMessage.ResourceNotFound);
            
            var result = await _authService.AuthenticateAsync(login);
            if (result == null)
            {
                var errorMessage = _localizer["InvalidCredentials"].Value ?? "Credenciales inválidas";

                _logger.LogWarning("🔒 Login fallido - Usuario: {Username}, Idioma: {Culture}, Mensaje: {Message}", 
                    login.Username, currentUICulture, errorMessage);

                return Unauthorized(new
                {
                    Message = errorMessage,
                    Code = "AUTH_001",
                    Status = 401,
                    Culture = currentUICulture,
                    // 🔍 DEBUG: Info adicional
                    AcceptLanguage = acceptLanguage
                });
            }

            _logger.LogInformation("✅ Login exitoso para usuario: {Username}", login.Username);
            return Ok(result);
        }
    }
}
using GoldBusiness.Application.Interfaces;
using GoldBusiness.Domain.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Localization;

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
        [EnableRateLimiting("auth")]
        public async Task<IActionResult> Login([FromBody] LoginDTO login)
        {
            var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
            _logger.LogDebug("Login attempt from IP: {IpAddress}", ipAddress);

            var result = await _authService.AuthenticateAsync(login);

            if (result == null || !result.Succeeded)
            {
                var errorMessage = result?.Message ?? _localizer["InvalidCredentials"].Value ?? "Credenciales inválidas";
                _logger.LogWarning("Failed login attempt from IP: {IpAddress}", ipAddress);

                return Unauthorized(new AuthResponseDTO
                {
                    Succeeded = false,
                    Message = errorMessage,
                    Data = null
                });
            }

            _logger.LogInformation("Successful login - User: {Username}, IP: {IpAddress}",
                login.Username, ipAddress);

            return Ok(result);
        }
    }
}
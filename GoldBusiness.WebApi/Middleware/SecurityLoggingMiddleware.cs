using System.Security.Claims;

namespace GoldBusiness.WebApi.Middleware;

/// <summary>
/// Middleware para logging de eventos de seguridad.
/// Registra todos los accesos a la API incluyendo información del usuario e IP.
/// </summary>
public class SecurityLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<SecurityLoggingMiddleware> _logger;

    public SecurityLoggingMiddleware(RequestDelegate next, ILogger<SecurityLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var startTime = DateTime.UtcNow;

        try
        {
            await _next(context);
        }
        finally
        {
            var elapsedTime = DateTime.UtcNow - startTime;
            var userId = context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "anonymous";
            var userName = context.User?.Identity?.Name ?? "anonymous";
            var ipAddress = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";
            var statusCode = context.Response.StatusCode;
            var method = context.Request.Method;
            var path = context.Request.Path;
            var userAgent = context.Request.Headers["User-Agent"].ToString();

            // Log de acceso normal
            _logger.LogInformation(
                "Security: User={UserId} ({UserName}) from IP={IpAddress} - {Method} {Path} - Status={StatusCode} - Duration={Duration}ms - UserAgent={UserAgent}",
                userId, userName, ipAddress, method, path, statusCode, elapsedTime.TotalMilliseconds, userAgent);

            // Log de intentos de acceso no autorizado
            if (statusCode == 401)
            {
                _logger.LogWarning(
                    "🔒 UNAUTHORIZED ACCESS ATTEMPT: IP={IpAddress} - {Method} {Path} - UserAgent={UserAgent}",
                    ipAddress, method, path, userAgent);
            }

            // Log de accesos prohibidos (falta de permisos)
            if (statusCode == 403)
            {
                _logger.LogWarning(
                    "🚫 FORBIDDEN ACCESS ATTEMPT: User={UserId} ({UserName}) from IP={IpAddress} - {Method} {Path}",
                    userId, userName, ipAddress, method, path);
            }

            // Log de errores del servidor
            if (statusCode >= 500)
            {
                _logger.LogError(
                    "❌ SERVER ERROR: User={UserId} from IP={IpAddress} - {Method} {Path} - Status={StatusCode}",
                    userId, ipAddress, method, path, statusCode);
            }
        }
    }
}

/// <summary>
/// Extensión para registrar el middleware de security logging.
/// </summary>
public static class SecurityLoggingMiddlewareExtensions
{
    public static IApplicationBuilder UseSecurityLogging(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<SecurityLoggingMiddleware>();
    }
}
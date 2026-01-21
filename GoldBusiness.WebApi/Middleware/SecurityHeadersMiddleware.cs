namespace GoldBusiness.WebApi.Middleware;

/// <summary>
/// Middleware que agrega headers de seguridad a todas las respuestas.
/// Protege contra XSS, Clickjacking, MIME sniffing, etc.
/// </summary>
public class SecurityHeadersMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<SecurityHeadersMiddleware> _logger;

    public SecurityHeadersMiddleware(RequestDelegate next, ILogger<SecurityHeadersMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // X-Content-Type-Options: Previene MIME sniffing
        context.Response.Headers.Append("X-Content-Type-Options", "nosniff");
        
        // X-Frame-Options: Previene Clickjacking
        context.Response.Headers.Append("X-Frame-Options", "DENY");
        
        // X-XSS-Protection: Habilita filtro XSS del navegador
        context.Response.Headers.Append("X-XSS-Protection", "1; mode=block");
        
        // Referrer-Policy: Controla información de referrer
        context.Response.Headers.Append("Referrer-Policy", "no-referrer");
        
        // ✅ NUEVO: Strict-Transport-Security (HSTS)
        context.Response.Headers.Append("Strict-Transport-Security", "max-age=31536000; includeSubDomains");
        
        // Content-Security-Policy: Define fuentes de contenido permitidas
        context.Response.Headers.Append(
            "Content-Security-Policy",
            "default-src 'self'; " +
            "script-src 'self'; " +
            "style-src 'self' 'unsafe-inline'; " +
            "img-src 'self' data:; " +
            "font-src 'self'; " +
            "connect-src 'self'; " +
            "frame-ancestors 'none';");
        
        // Permissions-Policy: Controla características del navegador
        context.Response.Headers.Append(
            "Permissions-Policy",
            "geolocation=(), microphone=(), camera=()");

        // X-Permitted-Cross-Domain-Policies: Previene acceso desde Flash/PDF
        context.Response.Headers.Append("X-Permitted-Cross-Domain-Policies", "none");

        await _next(context);
    }
}

/// <summary>
/// Extensión para registrar el middleware de security headers.
/// </summary>
public static class SecurityHeadersMiddlewareExtensions
{
    public static IApplicationBuilder UseSecurityHeaders(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<SecurityHeadersMiddleware>();
    }
}
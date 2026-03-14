using GoldBusiness.Application.Interfaces;
using GoldBusiness.Application.Services;
using GoldBusiness.Domain.Entities;
using GoldBusiness.Domain.Translation;
using GoldBusiness.Infrastructure.Context;
using GoldBusiness.Infrastructure.Repositories;
using GoldBusiness.WebApi.Middleware;
using GoldBusiness.WebApi.Swagger;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Globalization;
using System.Security.Claims;
using System.Text;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

// ============================================
// 🗄️ BASE DE DATOS E IDENTITY
// ============================================

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    // 🔒 Política de contraseñas robusta
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredLength = 12;
    options.Password.RequiredUniqueChars = 4;

    // 🔒 Configuración de lockout para seguridad
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;

    // 🔒 Requerir email confirmado (producción)
    options.SignIn.RequireConfirmedEmail = false;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.SameSite = SameSiteMode.None; // ⬅️ NUEVO
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    options.Cookie.HttpOnly = true;

    options.Events.OnRedirectToLogin = context =>
    {
        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        return Task.CompletedTask;
    };
    options.Events.OnRedirectToAccessDenied = context =>
    {
        context.Response.StatusCode = StatusCodes.Status403Forbidden;
        return Task.CompletedTask;
    };
});

// ✅ NUEVO: Configurar cookies del ExternalScheme
builder.Services.ConfigureExternalCookie(options =>
{
    options.Cookie.SameSite = SameSiteMode.None;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// ============================================
// 🌍 LOCALIZACIÓN (MULTIIDIOMA)
// ============================================

builder.Services.AddLocalization();

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var supportedCultures = new[]
    {
        new CultureInfo("es"),
        new CultureInfo("en"),
        new CultureInfo("fr")
    };

    options.DefaultRequestCulture = new RequestCulture("es");
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;

    options.RequestCultureProviders.Clear();
    options.RequestCultureProviders.Add(new AcceptLanguageHeaderRequestCultureProvider());
    options.RequestCultureProviders.Add(new QueryStringRequestCultureProvider());
    options.RequestCultureProviders.Add(new CookieRequestCultureProvider());
});

builder.Services.ConfigureApplicationCookie(options =>
{
    options.Events.OnRedirectToLogin = context =>
    {
        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        return Task.CompletedTask;
    };
    options.Events.OnRedirectToAccessDenied = context =>
    {
        context.Response.StatusCode = StatusCodes.Status403Forbidden;
        return Task.CompletedTask;
    };
});

// ============================================
// 🔌 SERVICIOS PROPIOS (DI)
// ============================================

// Auth
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IDashboardRepository, DashboardRepository>();
builder.Services.AddScoped<IDashboardService, DashboardService>();

// Commons
builder.Services.AddScoped<IPaisRepository, PaisRepository>();
builder.Services.AddScoped<IPaisService, PaisService>();
builder.Services.AddScoped<IProvinciaRepository, ProvinciaRepository>();
builder.Services.AddScoped<IProvinciaService, ProvinciaService>();
builder.Services.AddScoped<IMunicipioRepository, MunicipioRepository>();
builder.Services.AddScoped<IMunicipioService, MunicipioService>();
builder.Services.AddScoped<ICodigoPostalRepository, CodigoPostalRepository>();
builder.Services.AddScoped<ICodigoPostalService, CodigoPostalService>();

// Plan de Cuentas
builder.Services.AddScoped<IGrupoCuentaRepository, GrupoCuentaRepository>();
builder.Services.AddScoped<IGrupoCuentaService, GrupoCuentaService>();
builder.Services.AddScoped<ISubGrupoCuentaRepository, SubGrupoCuentaRepository>();
builder.Services.AddScoped<ISubGrupoCuentaService, SubGrupoCuentaService>();
builder.Services.AddScoped<ICuentaRepository, CuentaRepository>();
builder.Services.AddScoped<ICuentaService, CuentaService>();

// Configuración del Sistema
builder.Services.AddScoped<ISystemConfigurationRepository, SystemConfigurationRepository>();
builder.Services.AddScoped<ISystemConfigurationService, SystemConfigurationService>();

// Inventario - Clasificación
builder.Services.AddScoped<ILineaRepository, LineaRepository>();
builder.Services.AddScoped<ILineaService, LineaService>();
builder.Services.AddScoped<ISubLineaRepository, SubLineaRepository>();
builder.Services.AddScoped<ISubLineaService, SubLineaService>();

// Moneda y Ajustes
builder.Services.AddScoped<IMonedaRepository, MonedaRepository>();
builder.Services.AddScoped<IMonedaService, MonedaService>();
builder.Services.AddScoped<IConceptoAjusteRepository, ConceptoAjusteRepository>();
builder.Services.AddScoped<IConceptoAjusteService, ConceptoAjusteService>();

// Externos
builder.Services.AddScoped<IClienteRepository, ClienteRepository>();
builder.Services.AddScoped<IClienteService, ClienteService>();
builder.Services.AddScoped<IProveedorRepository, ProveedorRepository>();
builder.Services.AddScoped<IProveedorService, ProveedorService>();

// Establecimiento y Localidad
builder.Services.AddScoped<IEstablecimientoRepository, EstablecimientoRepository>();
builder.Services.AddScoped<IEstablecimientoService, EstablecimientoService>();
builder.Services.AddScoped<ILocalidadRepository, LocalidadRepository>();
builder.Services.AddScoped<ILocalidadService, LocalidadService>();

// Transacciones
builder.Services.AddScoped<ITransaccionRepository, TransaccionRepository>();
builder.Services.AddScoped<ITransaccionService, TransaccionService>();

// Productos y Fichas
builder.Services.AddScoped<IProductoRepository, ProductoRepository>();
builder.Services.AddScoped<IProductoService, ProductoService>();
builder.Services.AddScoped<IFichaProductoRepository, FichaProductoRepository>();
builder.Services.AddScoped<IFichaProductoService, FichaProductoService>();
builder.Services.AddScoped<IUnidadMedidaRepository, UnidadMedidaRepository>();
builder.Services.AddScoped<IUnidadMedidaService, UnidadMedidaService>();

// Background Service para limpieza de tokens
builder.Services.AddHostedService<TokenCleanupService>();

// ============================================
// 🔐 JWT AUTHENTICATION
// ============================================

var jwtIssuer = builder.Configuration["Jwt:Issuer"];
var jwtKey = builder.Configuration["Jwt:Key"];

if (string.IsNullOrWhiteSpace(jwtIssuer) || string.IsNullOrWhiteSpace(jwtKey))
{
    throw new InvalidOperationException("⚠️ JWT configuration is missing. Set Jwt:Issuer and Jwt:Key in User Secrets (Development) or Environment Variables (Production).");
}

if (jwtKey.Contains("REEMPLAZAR") || jwtKey.Contains("CONFIGURAR") || jwtKey.Contains("USAR-USER-SECRETS"))
{
    throw new InvalidOperationException("⚠️ JWT Key not configured properly. Update User Secrets (Development) or Environment Variables (Production).");
}

if (jwtKey.Length < 32)
{
    throw new InvalidOperationException("⚠️ JWT Key must be at least 32 characters long for security.");
}

var googleClientId = builder.Configuration["Authentication:Google:ClientId"];
var googleClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];

var authenticationBuilder = builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtIssuer,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
        NameClaimType = ClaimTypes.Name,
        RoleClaimType = ClaimTypes.Role,
        ClockSkew = TimeSpan.Zero
    };

    options.Events = new JwtBearerEvents
    {
        OnAuthenticationFailed = context =>
        {
            var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<Program>>();

            // ✅ Detectar token expirado y agregar header personalizado
            if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
            {
                context.Response.Headers.Append("Token-Expired", "true");
                logger.LogWarning("⏰ Token expirado desde IP: {IpAddress}",
                    context.HttpContext.Connection.RemoteIpAddress);
            }
            else
            {
                logger.LogWarning(context.Exception, "❌ JWT authentication failed desde IP: {IpAddress}",
                    context.HttpContext.Connection.RemoteIpAddress);
            }

            return Task.CompletedTask;
        },
        OnChallenge = context =>
        {
            var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<Program>>();
            logger.LogWarning("🔐 JWT challenge: {Error} - {Description} desde IP: {IpAddress}",
                context.Error,
                context.ErrorDescription,
                context.HttpContext.Connection.RemoteIpAddress);
            return Task.CompletedTask;
        },
        // ✅ NUEVO: Log cuando token es validado exitosamente
        OnTokenValidated = context =>
        {
            var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<Program>>();
            var env = context.HttpContext.RequestServices.GetRequiredService<IWebHostEnvironment>();

            if (env.IsDevelopment())
            {
                var userName = context.Principal?.Identity?.Name;
                logger.LogDebug("✅ Token validado para usuario: {UserName}", userName);
            }

            return Task.CompletedTask;
        }
    };
});

if (!string.IsNullOrWhiteSpace(googleClientId) && !string.IsNullOrWhiteSpace(googleClientSecret))
{
    authenticationBuilder.AddGoogle("Google", options =>
    {
        options.ClientId = googleClientId;
        options.ClientSecret = googleClientSecret;
        options.CallbackPath = "/signin-google";
        options.SignInScheme = IdentityConstants.ExternalScheme;
        options.SaveTokens = true;

        // ✅ Configuración de cookies para compatibilidad cross-site y móvil
        options.CorrelationCookie.SameSite = SameSiteMode.None;
        options.CorrelationCookie.SecurePolicy = CookieSecurePolicy.Always;
        options.CorrelationCookie.HttpOnly = true;
        options.CorrelationCookie.IsEssential = true;

        // ✅ AGREGAR: Eventos para debugging
        options.Events.OnCreatingTicket = async context =>
        {
            var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<Program>>();
            logger.LogInformation("✅ Google ticket creado para usuario: {Email}",
                context.Principal?.FindFirst(ClaimTypes.Email)?.Value);
        };

        // ✅ NUEVO: Manejar el ticket recibido y redirigir al frontend
        options.Events.OnTicketReceived = async context =>
        {
            var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<Program>>();
            var userManager = context.HttpContext.RequestServices.GetRequiredService<UserManager<ApplicationUser>>();
            var authService = context.HttpContext.RequestServices.GetRequiredService<IAuthService>();
            var configuration = context.HttpContext.RequestServices.GetRequiredService<IConfiguration>();

            logger.LogInformation("🎫 OnTicketReceived - Procesando autenticación Google");

            try
            {
                // Obtener returnUrl de las propiedades
                string? returnUrl = null;
                context.Properties?.Items.TryGetValue("returnUrl", out returnUrl);

                var safeReturnUrl = !string.IsNullOrWhiteSpace(returnUrl) &&
                    Uri.TryCreate(returnUrl, UriKind.Absolute, out var parsed) &&
                    (parsed.Scheme == Uri.UriSchemeHttp || parsed.Scheme == Uri.UriSchemeHttps)
                    ? returnUrl
                    : configuration["Authentication:Google:DefaultReturnUrl"]
                      ?? "https://goldbusinessstorage.z19.web.core.windows.net/login?returnUrl=%2Fdashboard";

                var email = context.Principal?.FindFirst(ClaimTypes.Email)?.Value?.Trim();

                if (string.IsNullOrWhiteSpace(email))
                {
                    logger.LogWarning("❌ Email no encontrado en ticket de Google");
                    context.Response.Redirect($"{safeReturnUrl}#error=google_email_not_found");
                    context.HandleResponse();
                    return;
                }

                logger.LogInformation("👤 Procesando usuario: {Email}", email);

                var user = await userManager.FindByEmailAsync(email);
                if (user == null)
                {
                    logger.LogWarning("❌ Usuario no encontrado: {Email}", email);
                    context.Response.Redirect($"{safeReturnUrl}#error=google_user_not_found");
                    context.HandleResponse();
                    return;
                }

                // Verificar que el usuario esté configurado para Google
                var userClaims = await userManager.GetClaimsAsync(user);
                var authProvider = userClaims.FirstOrDefault(c => c.Type == "authProvider")?.Value ?? "Local";

                if (!string.Equals(authProvider, "Google", StringComparison.OrdinalIgnoreCase))
                {
                    logger.LogWarning("❌ Usuario {Email} no configurado para Google (Provider: {Provider})",
                        email, authProvider);
                    context.Response.Redirect($"{safeReturnUrl}#error=google_provider_not_allowed");
                    context.HandleResponse();
                    return;
                }

                // Vincular login de Google si no existe
                var providerKey = context.Principal?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (!string.IsNullOrWhiteSpace(providerKey))
                {
                    var logins = await userManager.GetLoginsAsync(user);
                    if (!logins.Any(l => l.LoginProvider.Equals(GoogleDefaults.AuthenticationScheme,
                        StringComparison.OrdinalIgnoreCase)))
                    {
                        await userManager.AddLoginAsync(user,
                            new UserLoginInfo(GoogleDefaults.AuthenticationScheme, providerKey,
                                GoogleDefaults.AuthenticationScheme));
                        logger.LogInformation("✅ Login de Google vinculado para {Email}", email);
                    }
                }

                user.EmailConfirmed = true;
                await userManager.UpdateAsync(user);

                // Generar JWT
                var ipAddress = context.HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown";
                var userAgent = context.HttpContext.Request.Headers["User-Agent"].ToString();

                var result = await authService.AuthenticateExternalUserAsync(user.Id, ipAddress, userAgent);

                if (result?.Succeeded != true || result.Data == null)
                {
                    logger.LogError("❌ Error generando token JWT para {Email}", email);
                    context.Response.Redirect($"{safeReturnUrl}#error=google_token_failed");
                    context.HandleResponse();
                    return;
                }

                logger.LogInformation("✅ Login exitoso para {Email}", email);

                // Construir URL de retorno con tokens
                var redirectUrl = $"{safeReturnUrl}#" +
                    $"token={Uri.EscapeDataString(result.Data.Token)}&" +
                    $"refreshToken={Uri.EscapeDataString(result.Data.RefreshToken)}&" +
                    $"expiresAt={Uri.EscapeDataString(result.Data.ExpiresAt)}";

                logger.LogInformation("🔀 Redirigiendo a: {Url}", safeReturnUrl);

                context.Response.Redirect(redirectUrl);
                context.HandleResponse();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "❌ Error en OnTicketReceived");
                var defaultUrl = configuration["Authentication:Google:DefaultReturnUrl"]
                    ?? "https://goldbusinessstorage.z19.web.core.windows.net/login";
                context.Response.Redirect($"{defaultUrl}#error=google_internal_error");
                context.HandleResponse();
            }
        };

        options.Events.OnRemoteFailure = context =>
        {
            var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<Program>>();
            logger.LogError(context.Failure, "❌ Error en autenticación Google");

            var errorUrl = "https://goldbusinessstorage.z19.web.core.windows.net/login#error=google_remote_failure";
            context.Response.Redirect(errorUrl);
            context.HandleResponse();
            return Task.CompletedTask;
        };
    });
}

// ============================================
// 🛡️ AUTORIZACIÓN (POLICIES)
// ============================================

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ERPFullAccess", policy =>
        policy.RequireClaim("permission", "ERP:FullAccess"));

    options.AddPolicy("ERPAdminAccess", policy =>
        policy.RequireClaim("permission", "ERP:AdminAccess"));

    // Uso recomendado: aceptar cualquiera de ambos permisos
    options.AddPolicy("ERPAdminOrFullAccess", policy =>
        policy.RequireClaim("permission", "ERP:FullAccess", "ERP:AdminAccess")
    );

    options.AddPolicy("ERPFinanceAccess", policy =>
        policy.RequireClaim("permission", "ERP:FinanceAccess"));

    options.AddPolicy("ERPAccountingAccess", policy =>
        policy.RequireClaim("permission", "ERP:AccountingAccess"));

    options.FallbackPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
});

// ============================================
// 🌐 CORS - CONFIGURACIÓN SEGURA POR ENTORNO
// ============================================

builder.Services.AddCors(options =>
{
    options.AddPolicy("Development", policy =>
    {
        var devOrigins = builder.Configuration.GetSection("Cors:Development:AllowedOrigins").Get<string[]>() ?? new[] { "http://localhost:5173" };
        policy.WithOrigins(devOrigins)
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials()
              .WithExposedHeaders("Token-Expired");
    });

    options.AddPolicy("Production", policy =>
    {
        var prodOrigins = builder.Configuration.GetSection("Cors:Production:AllowedOrigins").Get<string[]>() ?? Array.Empty<string>();
        policy.WithOrigins(prodOrigins)
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials()
              .WithExposedHeaders("Token-Expired");
    });
});

// ============================================
// 🚦 RATE LIMITING
// ============================================

var enableRateLimiting = builder.Configuration.GetValue<bool>("Security:EnableRateLimiting", true);

if (enableRateLimiting)
{
    builder.Services.AddRateLimiter(options =>
    {
        var authPermitLimit = builder.Configuration.GetValue<int>("RateLimiting:Auth:PermitLimit", 10);
        var authWindowMinutes = builder.Configuration.GetValue<int>("RateLimiting:Auth:WindowMinutes", 1);

        options.AddFixedWindowLimiter("auth", limiterOptions =>
        {
            limiterOptions.PermitLimit = authPermitLimit;
            limiterOptions.Window = TimeSpan.FromMinutes(authWindowMinutes);
            limiterOptions.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
            limiterOptions.QueueLimit = 0;
        });

        var apiPermitLimit = builder.Configuration.GetValue<int>("RateLimiting:Api:PermitLimit", 100);
        var apiWindowMinutes = builder.Configuration.GetValue<int>("RateLimiting:Api:WindowMinutes", 1);

        options.AddFixedWindowLimiter("api", limiterOptions =>
        {
            limiterOptions.PermitLimit = apiPermitLimit;
            limiterOptions.Window = TimeSpan.FromMinutes(apiWindowMinutes);
            limiterOptions.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
            limiterOptions.QueueLimit = 0;
        });

        options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
    });
}

// ============================================
// 📝 CONTROLLERS CON VALIDACIÓN LOCALIZADA
// ============================================

builder.Services.AddControllers()
    .AddDataAnnotationsLocalization(options =>
    {
        options.DataAnnotationLocalizerProvider = (type, factory) =>
            factory.Create(typeof(GoldBusiness.Domain.Resources.ValidationMessages));
    })
    .ConfigureApiBehaviorOptions(options =>
    {
        options.InvalidModelStateResponseFactory = context =>
        {
            var currentCulture = CultureInfo.CurrentUICulture.Name;

            var errors = context.ModelState
                .Where(e => e.Value?.Errors.Count > 0)
                .ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value?.Errors.Select(e => e.ErrorMessage).ToArray() ?? Array.Empty<string>()
                );

            var result = new
            {
                type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                title = "One or more validation errors occurred.",
                status = 400,
                culture = currentCulture,
                errors = errors,
                timestamp = DateTime.UtcNow,
                apiVersion = builder.Configuration["ApiVersion:Name"] ?? "v2.0"
            };

            return new BadRequestObjectResult(result);
        };
    });

builder.Services.AddEndpointsApiExplorer();

// ============================================
// 📚 SWAGGER CON JWT Y LOCALIZACIÓN
// ============================================

builder.Services.AddSwaggerGen(c =>
{
    var apiVersion = builder.Configuration["ApiVersion:Name"] ?? "v2.0";

    c.SwaggerDoc("v2", new OpenApiInfo
    {
        Title = "GoldBusiness API",
        Version = apiVersion,
        Description = "API REST para sistema ERP con soporte multiidioma (es, en, fr).",
        Contact = new OpenApiContact
        {
            Name = "Chokisoft Soluciones Tecnológicas",
            Email = "chokisoft@gmail.com",
            Url = new Uri("https://github.com/chokisoft/GoldBusiness")
        }
    });

    // ✅ Configuración de tags con prefijos numéricos (ocultos para Nomencladores y Configuracion)
    c.TagActionsBy(api =>
    {
        var controller = api.ActionDescriptor.RouteValues["controller"];

        var tagPrefix = controller switch
        {
            "ApiInfo" => "01",
            "Auth" => "02",
            "Nomencladores" => "10",
            "GrupoCuenta" => "11",
            "SubGrupoCuenta" => "12",
            "Cuenta" => "13",
            "Moneda" => "14",
            "ConceptoAjuste" => "15",
            "Transaccion" => "16",
            "Establecimiento" => "17",
            "Localidad" => "18",
            "Cliente" => "19",
            "Proveedor" => "20",
            "Linea" => "21",
            "SubLinea" => "22",
            "UnidadMedida" => "23",
            "Producto" => "24",
            "FichaProducto" => "25",
            "Configuracion" => "70",
            "SystemConfiguration" => "71",
            _ => "99"
        };

        var displayName = controller switch
        {
            "ApiInfo" => "📊 Información de la API",
            "Auth" => "🔐 Autenticación",
            "Nomencladores" => "🗂️ Nomencladores",
            "GrupoCuenta" => "Grupo de Cuentas",
            "SubGrupoCuenta" => "SubGrupo de Cuentas",
            "Cuenta" => "Cuentas",
            "Moneda" => "Monedas",
            "ConceptoAjuste" => "Conceptos de Ajuste",
            "Transaccion" => "Transacciones",
            "Establecimiento" => "Establecimientos",
            "Localidad" => "Localidades",
            "Cliente" => "Clientes",
            "Proveedor" => "Proveedores",
            "Linea" => "Líneas",
            "SubLinea" => "SubLíneas",
            "UnidadMedida" => "Unidades de Medida",
            "Producto" => "Productos",
            "FichaProducto" => "Fichas de Producto (BOM)",
            "Configuracion" => "⚙️ Configuración",
            "SystemConfiguration" => "Configuración del Sistema",
            _ => controller ?? "Otros"
        };

        // ✅ Ocultar prefijo numérico solo para "Nomencladores" y "Configuracion"
        var tagName = controller switch
        {
            "ApiInfo" => displayName,
            "Auth" => displayName,
            "Nomencladores" => displayName,
            "Configuracion" => displayName,
            _ => $"{tagPrefix}. {displayName}"
        };

        return new[] { tagName };
    });

    // ✅ Ordenar operaciones dentro de cada tag por método HTTP
    c.OrderActionsBy(apiDesc =>
    {
        var controllerName = apiDesc.ActionDescriptor.RouteValues["controller"] ?? "Unknown";
        var httpMethod = apiDesc.HttpMethod ?? "GET";

        var tagPrefix = controllerName switch
        {
            "ApiInfo" => "01",
            "Auth" => "02",
            "Nomencladores" => "10",
            "GrupoCuenta" => "11",
            "SubGrupoCuenta" => "12",
            "Cuenta" => "13",
            "Moneda" => "14",
            "ConceptoAjuste" => "15",
            "Transaccion" => "16",
            "Establecimiento" => "17",
            "Localidad" => "18",
            "Cliente" => "19",
            "Proveedor" => "20",
            "Linea" => "21",
            "SubLinea" => "22",
            "UnidadMedida" => "23",
            "Producto" => "24",
            "FichaProducto" => "25",
            "Configuracion" => "70",
            "SystemConfiguration" => "71",
            _ => "99"
        };

        var methodOrder = httpMethod switch
        {
            "GET" => "1",
            "POST" => "2",
            "PUT" => "3",
            "PATCH" => "4",
            "DELETE" => "5",
            _ => "9"
        };

        return $"{tagPrefix}_{methodOrder}_{apiDesc.RelativePath}";
    });

    c.OperationFilter<AcceptLanguageHeaderOperationFilter>();

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Autenticación JWT usando Bearer. Ejemplo: 'Bearer {token}'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });

    // ✅ Necesario para que Swagger genere correctamente los endpoints con IFormFile
    c.MapType<IFormFile>(() => new OpenApiSchema { Type = "string", Format = "binary" });

});

// ============================================
// 📊 HEALTH CHECKS
// ============================================

builder.Services.AddHealthChecks()
    .AddDbContextCheck<ApplicationDbContext>("database");

// ============================================
// 🏗️ CONSTRUIR LA APLICACIÓN
// ============================================

var app = builder.Build();

// ============================================
// 🌱 SEED INICIAL (ROLES, USUARIOS, TRADUCCIONES)
// ============================================

using (var scope = app.Services.CreateScope())
{
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    try
    {
        logger.LogInformation("🌱 Iniciando seed de base de datos...");

        // ✅ Crear roles
        var rolesToEnsure = new[] { "DESARROLLADOR", "ADMINISTRADOR", "ECONOMICO", "CONTADOR" };
        foreach (var roleName in rolesToEnsure)
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                logger.LogInformation("Creando rol: {RoleName}", roleName);
                var roleResult = await roleManager.CreateAsync(new IdentityRole(roleName));
                if (!roleResult.Succeeded)
                {
                    logger.LogError("❌ Error creando rol {Role}: {Errors}", roleName,
                        string.Join(", ", roleResult.Errors.Select(e => e.Description)));
                }
                else
                {
                    logger.LogInformation("✅ Rol creado: {RoleName}", roleName);
                }
            }
        }

        // ✅ Asignar claims a roles
        var roleClaimsMap = new Dictionary<string, string>
        {
            { "DESARROLLADOR", "ERP:FullAccess" },
            { "ADMINISTRADOR", "ERP:AdminAccess" },
            { "ECONOMICO", "ERP:FinanceAccess" },
            { "CONTADOR", "ERP:AccountingAccess" }
        };

        foreach (var kvp in roleClaimsMap)
        {
            var role = await roleManager.FindByNameAsync(kvp.Key);
            if (role != null)
            {
                var claims = await roleManager.GetClaimsAsync(role);
                if (!claims.Any(c => c.Type == "permission" && c.Value == kvp.Value))
                {
                    await roleManager.AddClaimAsync(role, new Claim("permission", kvp.Value));
                    logger.LogInformation("✅ Claim '{Claim}' agregado al rol {Role}", kvp.Value, kvp.Key);
                }
            }
        }

        // ✅ Crear usuario de desarrollo
        var defaultUsername = builder.Configuration["Seed:DefaultUser:Username"];
        var defaultEmail = builder.Configuration["Seed:DefaultUser:Email"];
        var defaultPassword = builder.Configuration["Seed:DefaultUser:Password"];
        var defaultFullName = builder.Configuration["Seed:DefaultUser:FullName"];

        if (!string.IsNullOrEmpty(defaultUsername) && !string.IsNullOrEmpty(defaultPassword))
        {
            var user = await userManager.FindByNameAsync(defaultUsername);
            if (user == null)
            {
                logger.LogInformation("Creando usuario de desarrollo: {Username}", defaultUsername);

                var newUser = new ApplicationUser
                {
                    UserName = defaultUsername,
                    Email = defaultEmail ?? $"{defaultUsername}@example.com",
                    EmailConfirmed = true,
                    IsActive = true
                };

                var result = await userManager.CreateAsync(newUser, defaultPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRolesAsync(newUser, new[] { "DESARROLLADOR" });
                    await userManager.AddClaimAsync(newUser, new Claim("fullName", defaultFullName ?? "Usuario Desarrollo"));
                    await userManager.AddClaimAsync(newUser, new Claim("permission", "ERP:FullAccess"));
                    await userManager.AddClaimAsync(newUser, new Claim("accessLevel", "*"));

                    logger.LogInformation("✅ Usuario '{Username}' creado y configurado", newUser.UserName);
                }
                else
                {
                    logger.LogError("❌ Error creando usuario: {Errors}",
                        string.Join(", ", result.Errors.Select(e => e.Description)));
                }
            }
            else
            {
                logger.LogInformation("✅ Usuario '{Username}' ya existe", user.UserName);
            }
        }
        else
        {
            logger.LogWarning("⚠️ Usuario de seed no configurado en secrets.json");
        }

        // ✅ Crear usuario administrador por defecto
        var adminUsername = builder.Configuration["Seed:DefaultAdmin:Username"];
        var adminEmail = builder.Configuration["Seed:DefaultAdmin:Email"];
        var adminPassword = builder.Configuration["Seed:DefaultAdmin:Password"];
        var adminFullName = builder.Configuration["Seed:DefaultAdmin:FullName"];

        if (!string.IsNullOrEmpty(adminUsername) && !string.IsNullOrEmpty(adminPassword))
        {
            var adminUser = await userManager.FindByNameAsync(adminUsername);
            if (adminUser == null)
            {
                logger.LogInformation("Creando usuario administrador por defecto: {Username}", adminUsername);

                var newAdmin = new ApplicationUser
                {
                    UserName = adminUsername,
                    Email = adminEmail ?? $"{adminUsername}@example.com",
                    EmailConfirmed = true,
                    IsActive = true
                };

                var adminResult = await userManager.CreateAsync(newAdmin, adminPassword);
                if (adminResult.Succeeded)
                {
                    await userManager.AddToRolesAsync(newAdmin, new[] { "ADMINISTRADOR" });
                    await userManager.AddClaimAsync(newAdmin, new Claim("fullName", adminFullName ?? "Administrador"));
                    await userManager.AddClaimAsync(newAdmin, new Claim("permission", "ERP:AdminAccess"));
                    await userManager.AddClaimAsync(newAdmin, new Claim("accessLevel", "*"));

                    logger.LogInformation("✅ Administrador '{Username}' creado y configurado", newAdmin.UserName);
                }
                else
                {
                    logger.LogError("❌ Error creando usuario administrador: {Errors}",
                        string.Join(", ", adminResult.Errors.Select(e => e.Description)));
                }
            }
            else
            {
                logger.LogInformation("✅ Usuario administrador '{Username}' ya existe", adminUser.UserName);
            }
        }
        else
        {
            logger.LogInformation("⚠️ Usuario administrador de seed no configurado en secrets.json");
        }

        // ✅ Seed de traducciones automáticas
        logger.LogInformation("🌍 Verificando traducciones...");

        var traduccionesAgregadas = 0;

        // Pais
        var paisIds = await db.Pais.Select(x => x.Id).ToListAsync();
        var paisConTradIds = await db.PaisTranslation
            .Select(t => t.PaisId)
            .Distinct()
            .ToListAsync();
        var paisSinTradIds = paisIds.Except(paisConTradIds).ToList();

        if (paisSinTradIds.Any())
        {
            var paisSinTrad = await db.Pais
                .Where(x => paisSinTradIds.Contains(x.Id))
                .ToListAsync();

            foreach (var item in paisSinTrad)
            {
                db.PaisTranslation.Add(new PaisTranslation(item.Id, "es", item.Descripcion, "system"));
                traduccionesAgregadas++;
            }
        }

        // Provincia
        var provinciaIds = await db.Pais.Select(x => x.Id).ToListAsync();
        var provinciaConTradIds = await db.ProvinciaTranslation
            .Select(t => t.ProvinciaId)
            .Distinct()
            .ToListAsync();
        var provinciaSinTradIds = provinciaIds.Except(provinciaConTradIds).ToList();

        if (provinciaSinTradIds.Any())
        {
            var provinciaSinTrad = await db.Provincia
                .Where(x => provinciaSinTradIds.Contains(x.Id))
                .ToListAsync();

            foreach (var item in provinciaSinTrad)
            {
                db.ProvinciaTranslation.Add(new ProvinciaTranslation(item.Id, "es", item.Descripcion, "system"));
                traduccionesAgregadas++;
            }
        }

        // Municipio
        var municipioIds = await db.Municipio.Select(x => x.Id).ToListAsync();
        var municipioConTradIds = await db.MunicipioTranslation
            .Select(t => t.MunicipioId)
            .Distinct()
            .ToListAsync();
        var municipioSinTradIds = municipioIds.Except(municipioConTradIds).ToList();

        if (municipioSinTradIds.Any())
        {
            var municipioSinTrad = await db.Municipio
                .Where(x => municipioSinTradIds.Contains(x.Id))
                .ToListAsync();

            foreach (var item in municipioSinTrad)
            {
                db.MunicipioTranslation.Add(new MunicipioTranslation(item.Id, "es", item.Descripcion, "system"));
                traduccionesAgregadas++;
            }
        }

/*
        // Codigo Postal
        var codigoPostalIds = await db.CodigoPostal.Select(x => x.Id).ToListAsync();
        var codigoPostalConTradIds = await db.CodigoPostalTranslation
            .Select(t => t.CodigoPostalId)
            .Distinct()
            .ToListAsync();
        var codigoPostalSinTradIds = codigoPostalIds.Except(codigoPostalConTradIds).ToList();

        if (codigoPostalSinTradIds.Any())
        {
            var codigoPostalSinTrad = await db.CodigoPostal
                .Where(x => codigoPostalSinTradIds.Contains(x.Id))
                .ToListAsync();

            foreach (var item in codigoPostalSinTrad)
            {
                db.CodigoPostalTranslation.Add(new CodigoPostalTranslation(item.Id, "es", item.Descripcion, "system"));
                traduccionesAgregadas++;
            }
        }
*/

        // SystemConfiguration
        var systemConfigIds = await db.SystemConfiguration.Select(x => x.Id).ToListAsync();
        var systemConfigConTradIds = await db.SystemConfigurationTranslation
            .Select(t => t.ConfiguracionId)
            .Distinct()
            .ToListAsync();
        var systemConfigSinTradIds = systemConfigIds.Except(systemConfigConTradIds).ToList();

        if (systemConfigSinTradIds.Any())
        {
            var systemConfigSinTrad = await db.SystemConfiguration
                .Where(x => systemConfigSinTradIds.Contains(x.Id))
                .ToListAsync();

            foreach (var item in systemConfigSinTrad)
            {
                db.SystemConfigurationTranslation.Add(
                    new SystemConfigurationTranslation(item.Id, "es", item.NombreNegocio, item.Direccion, item.GetMunicipio("es"), item.GetProvincia("es"), "system"));
                traduccionesAgregadas++;
            }
        }

        // Establecimientos
        var establecimientoIds = await db.Establecimiento.Select(x => x.Id).ToListAsync();
        var establecimientoConTradIds = await db.EstablecimientoTranslation
            .Select(t => t.EstablecimientoId)
            .Distinct()
            .ToListAsync();
        var establecimientoSinTradIds = establecimientoIds.Except(establecimientoConTradIds).ToList();

        if (establecimientoSinTradIds.Any())
        {
            var establecimientoSinTrad = await db.Establecimiento
                .Where(x => establecimientoSinTradIds.Contains(x.Id))
                .ToListAsync();

            foreach (var item in establecimientoSinTrad)
            {
                db.EstablecimientoTranslation.Add(
                    new EstablecimientoTranslation(item.Id, "es", item.Descripcion, "system"));
                traduccionesAgregadas++;
            }
        }

        // GrupoCuenta
        var grupoIds = await db.GrupoCuenta.Select(x => x.Id).ToListAsync();
        var grupoConTradIds = await db.GrupoCuentaTranslation
            .Select(t => t.GrupoCuentaId)
            .Distinct()
            .ToListAsync();
        var grupoSinTradIds = grupoIds.Except(grupoConTradIds).ToList();

        if (grupoSinTradIds.Any())
        {
            var gruposSinTrad = await db.GrupoCuenta
                .Where(x => grupoSinTradIds.Contains(x.Id))
                .ToListAsync();

            foreach (var item in gruposSinTrad)
            {
                db.GrupoCuentaTranslation.Add(new GrupoCuentaTranslation(item.Id, "es", item.Descripcion, "system"));
                traduccionesAgregadas++;
            }
        }

        // SubGrupoCuenta
        var subgrupoIds = await db.SubGrupoCuenta.Select(x => x.Id).ToListAsync();
        var subgrupoConTradIds = await db.SubGrupoCuentaTranslation
            .Select(t => t.SubGrupoCuentaId)
            .Distinct()
            .ToListAsync();
        var subgrupoSinTradIds = subgrupoIds.Except(subgrupoConTradIds).ToList();

        if (subgrupoSinTradIds.Any())
        {
            var subgruposSinTrad = await db.SubGrupoCuenta
                .Where(x => subgrupoSinTradIds.Contains(x.Id))
                .ToListAsync();

            foreach (var item in subgruposSinTrad)
            {
                db.SubGrupoCuentaTranslation.Add(new SubGrupoCuentaTranslation(item.Id, "es", item.Descripcion, "system"));
                traduccionesAgregadas++;
            }
        }

        // Cuenta
        var cuentaIds = await db.Cuenta.Select(x => x.Id).ToListAsync();
        var cuentaConTradIds = await db.CuentaTranslation
            .Select(t => t.CuentaId)
            .Distinct()
            .ToListAsync();
        var cuentaSinTradIds = cuentaIds.Except(cuentaConTradIds).ToList();

        if (cuentaSinTradIds.Any())
        {
            var cuentasSinTrad = await db.Cuenta
                .Where(x => cuentaSinTradIds.Contains(x.Id))
                .ToListAsync();

            foreach (var item in cuentasSinTrad)
            {
                db.CuentaTranslation.Add(new CuentaTranslation(item.Id, "es", item.Descripcion, "system"));
                traduccionesAgregadas++;
            }
        }

        // Localidad
        var localidadIds = await db.Localidad.Select(x => x.Id).ToListAsync();
        var localidadConTradIds = await db.LocalidadTranslation
            .Select(t => t.LocalidadId)
            .Distinct()
            .ToListAsync();
        var localidadSinTradIds = localidadIds.Except(localidadConTradIds).ToList();

        if (localidadSinTradIds.Any())
        {
            var localidadSinTrad = await db.Localidad
                .Where(x => localidadSinTradIds.Contains(x.Id))
                .ToListAsync();

            foreach (var item in localidadSinTrad)
            {
                db.LocalidadTranslation.Add(
                    new LocalidadTranslation(item.Id, "es", item.Descripcion, "system"));
                traduccionesAgregadas++;
            }
        }

        // Linea
        var lineaIds = await db.Linea.Select(x => x.Id).ToListAsync();
        var lineaConTradIds = await db.LineaTranslation
            .Select(t => t.LineaId)
            .Distinct()
            .ToListAsync();
        var lineaSinTradIds = lineaIds.Except(lineaConTradIds).ToList();

        if (lineaSinTradIds.Any())
        {
            var lineasSinTrad = await db.Linea
                .Where(x => lineaSinTradIds.Contains(x.Id))
                .ToListAsync();

            foreach (var item in lineasSinTrad)
            {
                db.LineaTranslation.Add(new LineaTranslation(item.Id, "es", item.Descripcion, "system"));
                traduccionesAgregadas++;
            }
        }

        // SubLinea
        var sublineaIds = await db.SubLinea.Select(x => x.Id).ToListAsync();
        var sublineaConTradIds = await db.SubLineaTranslation
            .Select(t => t.SubLineaId)
            .Distinct()
            .ToListAsync();
        var sublineaSinTradIds = sublineaIds.Except(sublineaConTradIds).ToList();

        if (sublineaSinTradIds.Any())
        {
            var sublineasSinTrad = await db.SubLinea
                .Where(x => sublineaSinTradIds.Contains(x.Id))
                .ToListAsync();

            foreach (var item in sublineasSinTrad)
            {
                db.SubLineaTranslation.Add(new SubLineaTranslation(item.Id, "es", item.Descripcion, "system"));
                traduccionesAgregadas++;
            }
        }

        // Moneda
        var monedaIds = await db.Moneda.Select(x => x.Id).ToListAsync();
        var monedaConTradIds = await db.MonedaTranslation
            .Select(t => t.MonedaId)
            .Distinct()
            .ToListAsync();
        var monedaSinTradIds = monedaIds.Except(monedaConTradIds).ToList();

        if (monedaSinTradIds.Any())
        {
            var monedasSinTrad = await db.Moneda
                .Where(x => monedaSinTradIds.Contains(x.Id))
                .ToListAsync();

            foreach (var item in monedasSinTrad)
            {
                db.MonedaTranslation.Add(new MonedaTranslation(item.Id, "es", item.Descripcion, "system"));
                traduccionesAgregadas++;
            }
        }

        // ConceptoAjuste
        var conceptoIds = await db.ConceptoAjuste.Select(x => x.Id).ToListAsync();
        var conceptoConTradIds = await db.ConceptoAjusteTranslation
            .Select(t => t.ConceptoAjusteId)
            .Distinct()
            .ToListAsync();
        var conceptoSinTradIds = conceptoIds.Except(conceptoConTradIds).ToList();

        if (conceptoSinTradIds.Any())
        {
            var conceptosSinTrad = await db.ConceptoAjuste
                .Where(x => conceptoSinTradIds.Contains(x.Id))
                .ToListAsync();

            foreach (var item in conceptosSinTrad)
            {
                db.ConceptoAjusteTranslation.Add(new ConceptoAjusteTranslation(item.Id, "es", item.Descripcion, "system"));
                traduccionesAgregadas++;
            }
        }

        // UnidadMedida
        var unidadMedidaIds = await db.UnidadMedida.Select(x => x.Id).ToListAsync();
        var unidadMedidaConTradIds = await db.UnidadMedidaTranslation
            .Select(t => t.UnidadMedidaId)
            .Distinct()
            .ToListAsync();
        var unidadMedidaSinTradIds = unidadMedidaIds.Except(unidadMedidaConTradIds).ToList();

        if (unidadMedidaSinTradIds.Any())
        {
            var unidadMedidaSinTrad = await db.UnidadMedida
                .Where(x => unidadMedidaSinTradIds.Contains(x.Id))
                .ToListAsync();

            foreach (var item in unidadMedidaSinTrad)
            {
                db.UnidadMedidaTranslation.Add(new UnidadMedidaTranslation(item.Id, "es", item.Descripcion, "system"));
                traduccionesAgregadas++;
            }
        }

        // Transaccion
        var transaccionIds = await db.Transaccion.Select(x => x.Id).ToListAsync();
        var transaccionConTradIds = await db.TransaccionTranslation
            .Select(t => t.TransaccionId)
            .Distinct()
            .ToListAsync();
        var transaccionSinTradIds = transaccionIds.Except(transaccionConTradIds).ToList();

        if (transaccionSinTradIds.Any())
        {
            var transaccionSinTrad = await db.Transaccion
                .Where(x => transaccionSinTradIds.Contains(x.Id))
                .ToListAsync();

            foreach (var item in transaccionSinTrad)
            {
                db.TransaccionTranslation.Add(new TransaccionTranslation(item.Id, "es", item.Descripcion, "system"));
                traduccionesAgregadas++;
            }
        }

        if (traduccionesAgregadas > 0)
        {
            await db.SaveChangesAsync();
            logger.LogInformation("✅ {Count} traducciones agregadas", traduccionesAgregadas);
        }
        else
        {
            logger.LogInformation("✅ Todas las traducciones están actualizadas");
        }

        // ✅ Llamar al DbInitializer para seeds completos
        await GoldBusiness.Infrastructure.Data.DbInitializer.InitializeAsync(db, logger);

        logger.LogInformation("✅ Seed de base de datos completado exitosamente!");
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "❌ Error durante el seed de base de datos");
        throw;
    }
}

// ============================================
// 🌐 MIDDLEWARE PIPELINE
// ============================================

if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseStaticFiles();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v2/swagger.json", "GoldBusiness API v2");
        c.InjectStylesheet("/swagger-ui/swagger.css?v=2");
        c.DisplayRequestDuration();
        c.EnableDeepLinking();
        c.DefaultModelsExpandDepth(-1);
    });
}

if (app.Environment.IsProduction())
{
    app.UseHsts();
}

app.UseHttpsRedirection();

if (app.Environment.IsDevelopment())
{
    app.UseCors("Development");
}
else
{
    app.UseCors("Production");
}

var localizationOptions = app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value;
app.UseRequestLocalization(localizationOptions);

if (builder.Configuration.GetValue<bool>("Security:EnableSecurityHeaders", true))
{
    app.UseSecurityHeaders();
}

if (enableRateLimiting)
{
    app.UseRateLimiter();
}

if (builder.Configuration.GetValue<bool>("Security:EnableSecurityLogging", true))
{
    app.UseSecurityLogging();
}

app.UseAuthentication();
app.UseAuthorization();

app.UseExceptionHandler(appBuilder =>
{
    appBuilder.Run(async context =>
    {
        var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
        var exception = context.Features.Get<Microsoft.AspNetCore.Diagnostics.IExceptionHandlerFeature>()?.Error;
        var env = context.RequestServices.GetRequiredService<IWebHostEnvironment>();

        logger.LogError(exception, "Error no manejado en la aplicación");

        context.Response.StatusCode = 500;
        context.Response.ContentType = "application/json";

        var response = new
        {
            error = "Internal Server Error",
            message = env.IsDevelopment()
                ? exception?.Message
                : "An unexpected error occurred.",
            requestId = env.IsDevelopment() ? context.TraceIdentifier : null,
            timestamp = DateTime.UtcNow
        };

        await context.Response.WriteAsJsonAsync(response);
    });
});

app.MapControllers();
app.MapHealthChecks("/health");

app.Run();

// ============================================
// 🧹 BACKGROUND SERVICE PARA LIMPIEZA DE TOKENS
// ============================================

/// <summary>
/// Servicio en segundo plano que limpia tokens expirados cada 24 horas
/// </summary>
public class TokenCleanupService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<TokenCleanupService> _logger;
    private readonly TimeSpan _interval = TimeSpan.FromHours(24);

    public TokenCleanupService(IServiceProvider serviceProvider, ILogger<TokenCleanupService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("🧹 Token Cleanup Service iniciado");

        // Esperar 1 minuto antes de la primera ejecución
        await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var authService = scope.ServiceProvider.GetRequiredService<IAuthService>();

                _logger.LogInformation("🔄 Ejecutando limpieza de tokens expirados...");
                var deletedCount = await authService.CleanExpiredTokensAsync();

                _logger.LogInformation("✅ Limpieza completada. Tokens eliminados: {Count}", deletedCount);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Error durante limpieza de tokens");
            }

            await Task.Delay(_interval, stoppingToken);
        }

        _logger.LogInformation("🛑 Token Cleanup Service detenido");
    }
}
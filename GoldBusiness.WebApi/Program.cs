using GoldBusiness.Application.Interfaces;
using GoldBusiness.Application.Services;
using GoldBusiness.Domain.Entities;
using GoldBusiness.Domain.Translation;
using GoldBusiness.Infrastructure.Context;
using GoldBusiness.Infrastructure.Repositories;
using GoldBusiness.WebApi.Middleware;
using GoldBusiness.WebApi.Swagger;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
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
    options.Password.RequireUppercase = true;  // ✅ Ahora requerida
    options.Password.RequireNonAlphanumeric = true;  // ✅ Ahora requerida
    options.Password.RequiredLength = 12;  // ✅ Aumentado de 6 a 12
    options.Password.RequiredUniqueChars = 4;  // ✅ Nuevo: mínimo 4 caracteres únicos

    // 🔒 Configuración de lockout para seguridad
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);  // ✅ Aumentado de 5 a 15 minutos
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;

    // 🔒 Requerir email confirmado (producción)
    options.SignIn.RequireConfirmedEmail = false;  // ✅ Cambiar a true en producción cuando tengas email configurado
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

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

    // ✅ CONFIGURAR PROVIDERS EN EL ORDEN CORRECTO
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

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ICuentaRepository, CuentaRepository>();
builder.Services.AddScoped<ICuentaService, CuentaService>();
builder.Services.AddScoped<IGrupoCuentaRepository, GrupoCuentaRepository>();
builder.Services.AddScoped<IGrupoCuentaService, GrupoCuentaService>();
builder.Services.AddScoped<ISubGrupoCuentaRepository, SubGrupoCuentaRepository>();
builder.Services.AddScoped<ISubGrupoCuentaService, SubGrupoCuentaService>();
builder.Services.AddScoped<ILineaRepository, LineaRepository>();
builder.Services.AddScoped<ILineaService, LineaService>();

// ============================================
// 🔐 JWT AUTHENTICATION
// ============================================

var jwtIssuer = builder.Configuration["Jwt:Issuer"];
var jwtKey = builder.Configuration["Jwt:Key"];

// ⚠️ VALIDACIÓN TEMPRANA DE CONFIGURACIÓN JWT
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

builder.Services.AddAuthentication(options =>
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
        ClockSkew = TimeSpan.FromMinutes(2)
    };

    options.Events = new JwtBearerEvents
    {
        OnAuthenticationFailed = context =>
        {
            var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<Program>>();
            logger.LogWarning(context.Exception, "JWT authentication failed.");
            return Task.CompletedTask;
        },
        OnChallenge = context =>
        {
            var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<Program>>();
            logger.LogWarning("JWT challenge: {Error} - {Description}", context.Error, context.ErrorDescription);
            return Task.CompletedTask;
        }
    };
});

// ============================================
// 🛡️ AUTORIZACIÓN (POLICIES)
// ============================================

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ERPFullAccess", policy =>
        policy.RequireClaim("permission", "ERP:FullAccess"));

    options.AddPolicy("ERPAdminAccess", policy =>
        policy.RequireClaim("permission", "ERP:AdminAccess"));

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
    // Política para desarrollo
    options.AddPolicy("Development", policy =>
    {
        var allowedOrigins = builder.Configuration.GetSection("Cors:Development:AllowedOrigins").Get<string[]>()
            ?? new[] { "http://localhost:5173" };

        policy.WithOrigins(allowedOrigins)
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });

    // Política para producción
    options.AddPolicy("Production", policy =>
    {
        var allowedOrigins = builder.Configuration.GetSection("Cors:Production:AllowedOrigins").Get<string[]>()
            ?? new[] { "https://goldbusiness.com" };

        policy.WithOrigins(allowedOrigins)
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
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
        Description = "API REST para sistema ERP con soporte multiidioma (es, en, fr)"
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
        logger.LogInformation("Iniciando seed de base de datos...");

        // Crear roles
        var rolesToEnsure = new[] { "DESARROLLADOR", "ADMINISTRADOR", "ECONOMICO", "CONTADOR" };
        foreach (var roleName in rolesToEnsure)
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                logger.LogInformation("Creando rol: {RoleName}", roleName);
                var roleResult = await roleManager.CreateAsync(new IdentityRole(roleName));
                if (!roleResult.Succeeded)
                {
                    logger.LogError("Error creando rol {Role}: {Errors}", roleName,
                        string.Join(", ", roleResult.Errors.Select(e => e.Description)));
                }
                else
                {
                    logger.LogInformation("Rol creado exitosamente: {RoleName}", roleName);
                }
            }
        }

        // Asignar claims a roles
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
                    logger.LogInformation("Claim agregado al rol {Role}: {Claim}", kvp.Key, kvp.Value);
                }
            }
        }

        // ✅ Crear usuario de desarrollo desde configuración (más seguro)
        var defaultUsername = builder.Configuration["Seed:DefaultUser:Username"];
        var defaultEmail = builder.Configuration["Seed:DefaultUser:Email"];
        var defaultPassword = builder.Configuration["Seed:DefaultUser:Password"];
        var defaultFullName = builder.Configuration["Seed:DefaultUser:FullName"];

        // Solo crear usuario si está configurado en secrets.json
        if (!string.IsNullOrEmpty(defaultUsername) && !string.IsNullOrEmpty(defaultPassword))
        {
            var user = await userManager.FindByNameAsync(defaultUsername);
            if (user == null)
            {
                logger.LogInformation("Creando usuario de desarrollo...");

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
                    logger.LogInformation("Usuario creado: {UserName}", newUser.UserName);

                    var addRolesResult = await userManager.AddToRolesAsync(newUser, new[] { "DESARROLLADOR" });

                    if (addRolesResult.Succeeded)
                    {
                        await userManager.AddClaimAsync(newUser, new Claim("fullName", defaultFullName ?? "Usuario Desarrollo"));
                        await userManager.AddClaimAsync(newUser, new Claim("permission", "ERP:FullAccess"));
                        await userManager.AddClaimAsync(newUser, new Claim("accessLevel", "*"));

                        logger.LogInformation("Roles y claims asignados al usuario {UserName}", newUser.UserName);
                    }
                    else
                    {
                        logger.LogError("Error asignando roles al usuario: {Errors}",
                            string.Join(", ", addRolesResult.Errors.Select(e => e.Description)));
                    }
                }
                else
                {
                    logger.LogError("Error creando usuario: {Errors}",
                        string.Join(", ", result.Errors.Select(e => e.Description)));
                }
            }
            else
            {
                logger.LogInformation("Usuario encontrado: {UserName}", user.UserName);

                if (!await userManager.IsInRoleAsync(user, "DESARROLLADOR"))
                {
                    logger.LogInformation("Agregando rol DESARROLLADOR al usuario existente...");

                    var addRoleResult = await userManager.AddToRoleAsync(user, "DESARROLLADOR");

                    if (addRoleResult.Succeeded)
                    {
                        await userManager.AddClaimAsync(user, new Claim("fullName", defaultFullName ?? "Usuario Desarrollo"));
                        await userManager.AddClaimAsync(user, new Claim("permission", "ERP:FullAccess"));

                        logger.LogInformation("Rol y claims agregados al usuario existente");
                    }
                    else
                    {
                        logger.LogError("Error agregando rol al usuario existente: {Errors}",
                            string.Join(", ", addRoleResult.Errors.Select(e => e.Description)));
                    }
                }
            }
        }
        else
        {
            logger.LogWarning("Usuario de seed no configurado en secrets.json. Omitiendo creación de usuario de desarrollo.");
        }

        // Seed de traducciones iniciales
        logger.LogInformation("Iniciando seed de traducciones...");

        // ✅ GrupoCuenta
        var gruposSinTraduccion = db.GrupoCuenta.Where(x => !x.Translations.Any()).ToList();
        foreach (var g in gruposSinTraduccion)
        {
            db.GrupoCuentaTranslation.Add(new GrupoCuentaTranslation(g.Id, "es", g.Descripcion, "system"));
        }
        if (gruposSinTraduccion.Any())
            logger.LogInformation("{Count} traducciones de GrupoCuenta agregadas", gruposSinTraduccion.Count);

        // ✅ SubGrupoCuenta
        var subgruposSinTraduccion = db.SubGrupoCuenta.Where(x => !x.Translations.Any()).ToList();
        foreach (var s in subgruposSinTraduccion)
        {
            db.SubGrupoCuentaTranslation.Add(new SubGrupoCuentaTranslation(s.Id, "es", s.Descripcion, "system"));
        }
        if (subgruposSinTraduccion.Any())
            logger.LogInformation("{Count} traducciones de SubGrupoCuenta agregadas", subgruposSinTraduccion.Count);

        // ✅ Cuenta
        var cuentasSinTraduccion = db.Cuenta.Where(x => !x.Translations.Any()).ToList();
        foreach (var c in cuentasSinTraduccion)
        {
            db.CuentaTranslation.Add(new CuentaTranslation(c.Id, "es", c.Descripcion, "system"));
        }
        if (cuentasSinTraduccion.Any())
            logger.LogInformation("{Count} traducciones de Cuenta agregadas", cuentasSinTraduccion.Count);

        // ✅ Linea (NUEVO)
        var lineasSinTraduccion = db.Linea.Where(x => !x.Translations.Any()).ToList();
        foreach (var l in lineasSinTraduccion)
        {
            db.LineaTranslation.Add(new LineaTranslation(l.Id, "es", l.Descripcion, "system"));
        }
        if (lineasSinTraduccion.Any())
            logger.LogInformation("{Count} traducciones de Linea agregadas", lineasSinTraduccion.Count);

        // ✅ SubLinea (NUEVO)
        var sublineasSinTraduccion = db.SubLinea.Where(x => !x.Translations.Any()).ToList();
        foreach (var sl in sublineasSinTraduccion)
        {
            db.SubLineaTranslation.Add(new SubLineaTranslation(sl.Id, "es", sl.Descripcion, "system"));
        }
        if (sublineasSinTraduccion.Any())
            logger.LogInformation("{Count} traducciones de SubLinea agregadas", sublineasSinTraduccion.Count);

        // ✅ Guardar cambios si hay traducciones nuevas
        if (gruposSinTraduccion.Any() || subgruposSinTraduccion.Any() || cuentasSinTraduccion.Any() || lineasSinTraduccion.Any() || sublineasSinTraduccion.Any())
        {
            await db.SaveChangesAsync();
            logger.LogInformation("Traducciones guardadas en base de datos");
        }
        else
        {
            logger.LogInformation("No se encontraron registros sin traducciones");
        }

        logger.LogInformation("Seed de base de datos completado exitosamente!");
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Error durante el seed de base de datos");
        throw;
    }
}

// ============================================
// 🌐 MIDDLEWARE PIPELINE
// ============================================

// ✅ Swagger solo en desarrollo
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

// ✅ HSTS para producción
if (app.Environment.IsProduction())
{
    app.UseHsts();
}

app.UseHttpsRedirection();

// ✅ CORS según entorno
if (app.Environment.IsDevelopment())
{
    app.UseCors("Development");
}
else
{
    app.UseCors("Production");
}

// ✅ CRÍTICO: UseRequestLocalization ANTES de UseRouting
var localizationOptions = app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value;
app.UseRequestLocalization(localizationOptions);

// ✅ Security headers middleware
if (builder.Configuration.GetValue<bool>("Security:EnableSecurityHeaders", true))
{
    app.UseSecurityHeaders();
}

// ✅ Rate limiting
if (enableRateLimiting)
{
    app.UseRateLimiter();
}

// ✅ Security logging middleware
if (builder.Configuration.GetValue<bool>("Security:EnableSecurityLogging", true))
{
    app.UseSecurityLogging();
}

app.UseAuthentication();
app.UseAuthorization();

// ✅ Middleware para manejar excepciones (mejorado)
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

        // ✅ Solo mostrar detalles en desarrollo
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

// ✅ Health check endpoint
app.MapHealthChecks("/health");

// ✅ Información de la API
app.MapGet("/api-info", () => Results.Json(new
{
    name = "GoldBusiness API",
    version = builder.Configuration["ApiVersion:Name"] ?? "v2.0",
    description = "Sistema ERP con soporte multiidioma",
    supportedLanguages = new[] { "es", "en", "fr" },
    authentication = "JWT Bearer Token",
    documentation = "/swagger",
    environment = app.Environment.EnvironmentName
})).AllowAnonymous();

app.Run();
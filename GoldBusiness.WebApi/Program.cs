using GoldBusiness.Application.Interfaces;
using GoldBusiness.Application.Services;
using GoldBusiness.Domain.Entities;
using GoldBusiness.Domain.Translation;
using GoldBusiness.Infrastructure.Context;
using GoldBusiness.Infrastructure.Repositories;
using GoldBusiness.WebApi.Swagger;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Globalization;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// ============================================
// 🗄️ BASE DE DATOS E IDENTITY
// ============================================

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 6;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

// ============================================
// 🌍 LOCALIZACIÓN (MULTIIDIOMA)
// ============================================

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

    // ✅ AGREGAR LOGGING PARA DEBUG
    options.RequestCultureProviders.Insert(0, new CustomRequestCultureProvider(async context =>
    {
        var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();

        // Obtener el header Accept-Language
        var acceptLanguage = context.Request.Headers["Accept-Language"].ToString();
        logger.LogWarning("🌍 Accept-Language header: {AcceptLanguage}", acceptLanguage);

        // Detectar cultura
        string culture = "es"; // por defecto

        if (!string.IsNullOrEmpty(acceptLanguage))
        {
            var languages = acceptLanguage.Split(',');
            if (languages.Length > 0)
            {
                var lang = languages[0].Trim().Split(';')[0].Trim();
                var parts = lang.Split('-');
                culture = parts[0].ToLowerInvariant();
            }
        }

        // Verificar si es soportado
        if (!supportedCultures.Any(c => c.TwoLetterISOLanguageName == culture))
        {
            culture = "es";
        }

        logger.LogWarning("🌍 Cultura detectada: {Culture}", culture);
        logger.LogWarning("🌍 CurrentCulture: {CurrentCulture}", CultureInfo.CurrentCulture.Name);
        logger.LogWarning("🌍 CurrentUICulture: {CurrentUICulture}", CultureInfo.CurrentUICulture.Name);

        return new ProviderCultureResult(culture, culture);
    }));
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

// ============================================
// 🔐 JWT AUTHENTICATION
// ============================================

var jwtIssuer = builder.Configuration["Jwt:Issuer"];
var jwtKey = builder.Configuration["Jwt:Key"];
if (string.IsNullOrWhiteSpace(jwtIssuer) || string.IsNullOrWhiteSpace(jwtKey))
{
    throw new InvalidOperationException("JWT configuration is missing. Set Jwt:Issuer and Jwt:Key.");
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
// 🌐 CORS
// ============================================

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// ============================================
// 📝 CONTROLLERS CON VALIDACIÓN LOCALIZADA
// ============================================

// 🔧 CORRECCIÓN: Eliminar el CreateLogger que causa el error
builder.Services.AddControllers()
    .AddDataAnnotationsLocalization(options =>
    {
        options.DataAnnotationLocalizerProvider = (type, factory) =>
        {
            var localizer = factory.Create(typeof(GoldBusiness.Domain.Resources.ValidationMessages));

            // ✅ CORREGIDO: Solo crear el localizador, sin intentar crear logger
            // El logging se manejará en otros lugares (middleware o servicios)

            return localizer;
        };
    })
    .ConfigureApiBehaviorOptions(options =>
    {
        options.InvalidModelStateResponseFactory = context =>
        {
            var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<Program>>();

            // Obtener información de localización actual
            var localizationOptions = context.HttpContext.RequestServices
                .GetRequiredService<IOptions<RequestLocalizationOptions>>().Value;
            var currentCulture = CultureInfo.CurrentUICulture.Name;

            logger.LogWarning("🌍 InvalidModelState - Cultura actual: {Culture}", currentCulture);
            logger.LogWarning("🌍 Supported Cultures: {Cultures}",
                string.Join(", ", localizationOptions.SupportedUICultures.Select(c => c.Name)));

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
                timestamp = DateTime.UtcNow
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
    c.SwaggerDoc("v2", new OpenApiInfo
    {
        Title = "GoldBusiness API",
        Version = "v2",
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
        logger.LogInformation("🚀 Iniciando seed de base de datos...");

        // Crear roles
        var rolesToEnsure = new[] { "DESARROLLADOR", "ADMINISTRADDR", "ECONOMICO", "CONTADOR" };
        foreach (var roleName in rolesToEnsure)
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                logger.LogInformation("👥 Creando rol: {RoleName}", roleName);
                var roleResult = await roleManager.CreateAsync(new IdentityRole(roleName));
                if (!roleResult.Succeeded)
                {
                    logger.LogError("❌ Error creando rol {Role}: {Errors}", roleName,
                        string.Join(", ", roleResult.Errors.Select(e => e.Description)));
                }
                else
                {
                    logger.LogInformation("✅ Rol creado exitosamente: {RoleName}", roleName);
                }
            }
        }

        // Asignar claims a roles
        var roleClaimsMap = new Dictionary<string, string>
        {
            { "DESARROLLADOR", "ERP:FullAccess" },
            { "ADMINISTRADDR", "ERP:AdminAccess" },
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
                    logger.LogInformation("🔑 Claim agregado al rol {Role}: {Claim}", kvp.Key, kvp.Value);
                }
            }
        }

        // Crear usuario de desarrollo
        var user = await userManager.FindByNameAsync("fragela");
        if (user == null)
        {
            logger.LogInformation("👤 Creando usuario de desarrollo...");

            var newUser = new ApplicationUser
            {
                UserName = "fragela",
                Email = "chokisoft@gmail.com",
                EmailConfirmed = true,
                IsActive = true
            };

            var result = await userManager.CreateAsync(newUser, "choki1972,.");
            if (result.Succeeded)
            {
                logger.LogInformation("✅ Usuario creado: {UserName}", newUser.UserName);

                var addRolesResult = await userManager.AddToRolesAsync(newUser, new[] { "DESARROLLADOR" });

                if (addRolesResult.Succeeded)
                {
                    await userManager.AddClaimAsync(newUser, new Claim("fullName", "Rolando Fragela Herva"));
                    await userManager.AddClaimAsync(newUser, new Claim("permission", "ERP:FullAccess"));
                    await userManager.AddClaimAsync(newUser, new Claim("accessLevel", "*"));

                    logger.LogInformation("✅ Roles y claims asignados al usuario {UserName}", newUser.UserName);
                }
                else
                {
                    logger.LogError("❌ Error asignando roles al usuario: {Errors}",
                        string.Join(", ", addRolesResult.Errors.Select(e => e.Description)));
                }
            }
            else
            {
                logger.LogError("❌ Error creando usuario: {Errors}",
                    string.Join(", ", result.Errors.Select(e => e.Description)));
            }
        }
        else
        {
            logger.LogInformation("👤 Usuario encontrado: {UserName}", user.UserName);

            if (!await userManager.IsInRoleAsync(user, "DESARROLLADOR"))
            {
                logger.LogInformation("➕ Agregando rol DESARROLLADOR al usuario existente...");

                var addRoleResult = await userManager.AddToRoleAsync(user, "DESARROLLADOR");

                if (addRoleResult.Succeeded)
                {
                    await userManager.AddClaimAsync(user, new Claim("FullName", "Rolando Fragela Herva"));
                    await userManager.AddClaimAsync(user, new Claim("permission", "ERP:FullAccess"));

                    logger.LogInformation("✅ Rol y claims agregados al usuario existente");
                }
                else
                {
                    logger.LogError("❌ Error agregando rol al usuario existente: {Errors}",
                        string.Join(", ", addRoleResult.Errors.Select(e => e.Description)));
                }
            }
        }

        // Seed de traducciones iniciales
        logger.LogInformation("🌍 Iniciando seed de traducciones...");

        var gruposSinTraduccion = db.GrupoCuenta.Where(x => !x.Translations.Any()).ToList();
        foreach (var g in gruposSinTraduccion)
        {
            db.GrupoCuentaTranslation.Add(new GrupoCuentaTranslation(g.Id, "es", g.Descripcion, "system"));
        }
        if (gruposSinTraduccion.Any())
            logger.LogInformation("✅ {Count} traducciones de GrupoCuenta agregadas", gruposSinTraduccion.Count);

        var subgruposSinTraduccion = db.SubGrupoCuenta.Where(x => !x.Translations.Any()).ToList();
        foreach (var s in subgruposSinTraduccion)
        {
            db.SubGrupoCuentaTranslation.Add(new SubGrupoCuentaTranslation(s.Id, "es", s.Descripcion, "system"));
        }
        if (subgruposSinTraduccion.Any())
            logger.LogInformation("✅ {Count} traducciones de SubGrupoCuenta agregadas", subgruposSinTraduccion.Count);

        var cuentasSinTraduccion = db.Cuenta.Where(x => !x.Translations.Any()).ToList();
        foreach (var c in cuentasSinTraduccion)
        {
            db.CuentaTranslation.Add(new CuentaTranslation(c.Id, "es", c.Descripcion, "system"));
        }
        if (cuentasSinTraduccion.Any())
            logger.LogInformation("✅ {Count} traducciones de Cuenta agregadas", cuentasSinTraduccion.Count);

        if (gruposSinTraduccion.Any() || subgruposSinTraduccion.Any() || cuentasSinTraduccion.Any())
        {
            await db.SaveChangesAsync();
            logger.LogInformation("💾 Traducciones guardadas en base de datos");
        }
        else
        {
            logger.LogInformation("📝 No se encontraron registros sin traducciones");
        }

        logger.LogInformation("🎉 Seed de base de datos completado exitosamente!");
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
        c.InjectStylesheet("/swagger-ui/swagger.css?v=1");
        c.DisplayRequestDuration();
        c.EnableDeepLinking();
        c.DefaultModelsExpandDepth(-1); // Ocultar schemas por defecto
    });
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");

// ✅ CRÍTICO: UseRequestLocalization ANTES de UseRouting
var localizationOptions = app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value;
app.UseRequestLocalization(localizationOptions);

// ✅ MIDDLEWARE para logging de cultura en cada request
app.Use(async (context, next) =>
{
    var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();

    // Log de información de la request
    logger.LogInformation("🌐 REQUEST - Method: {Method}, Path: {Path}",
        context.Request.Method, context.Request.Path);

    // Log de headers de localización
    var acceptLanguage = context.Request.Headers["Accept-Language"].ToString();
    if (!string.IsNullOrEmpty(acceptLanguage))
    {
        logger.LogInformation("🌍 Accept-Language: {AcceptLanguage}", acceptLanguage);
    }

    // Log de cultura actual
    logger.LogInformation("🌍 Cultura actual - Culture: {Culture}, UICulture: {UICulture}",
        CultureInfo.CurrentCulture.Name,
        CultureInfo.CurrentUICulture.Name);

    // Verificar si tenemos un localizador funcionando
    try
    {
        var localizerFactory = context.RequestServices.GetRequiredService<IStringLocalizerFactory>();
        var validationLocalizer = localizerFactory.Create(typeof(GoldBusiness.Domain.Resources.ValidationMessages));
        var testTranslation = validationLocalizer["Required"];

        logger.LogInformation("✅ Localizador funcionando: {TestTranslation}", testTranslation);
    }
    catch (Exception ex)
    {
        logger.LogWarning(ex, "⚠️ Problema con el localizador");
    }

    await next();
});

app.UseAuthentication();
app.UseAuthorization();

// ✅ Middleware para manejar excepciones de localización
app.UseExceptionHandler(appBuilder =>
{
    appBuilder.Run(async context =>
    {
        var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
        var exception = context.Features.Get<Microsoft.AspNetCore.Diagnostics.IExceptionHandlerFeature>()?.Error;

        logger.LogError(exception, "🔥 Error no manejado en la aplicación");

        context.Response.StatusCode = 500;
        context.Response.ContentType = "application/json";

        await context.Response.WriteAsJsonAsync(new
        {
            error = "Internal Server Error",
            message = "An unexpected error occurred.",
            requestId = context.TraceIdentifier,
            timestamp = DateTime.UtcNow
        });
    });
});

app.MapControllers();

// ✅ Health check endpoint
app.MapGet("/health", () => Results.Json(new
{
    status = "Healthy",
    timestamp = DateTime.UtcNow,
    localization = new
    {
        currentCulture = CultureInfo.CurrentCulture.Name,
        currentUICulture = CultureInfo.CurrentUICulture.Name,
        defaultCulture = localizationOptions.DefaultRequestCulture.Culture.Name
    }
}));

// ✅ Información de la API
app.MapGet("/api-info", () => Results.Json(new
{
    name = "GoldBusiness API",
    version = "v2",
    description = "Sistema ERP con soporte multiidioma",
    supportedLanguages = new[] { "es", "en", "fr" },
    authentication = "JWT Bearer Token",
    documentation = "/swagger"
}));

app.Run();
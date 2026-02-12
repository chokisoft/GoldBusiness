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
    options.AddPolicy("Development", policy =>
    {
        var allowedOrigins = builder.Configuration.GetSection("Cors:Development:AllowedOrigins").Get<string[]>()
            ?? new[] { "http://localhost:5173" };

        policy.WithOrigins(allowedOrigins)
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });

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
        Description = "API REST para sistema ERP con soporte multiidioma (es, en, fr).",
        Contact = new OpenApiContact
        {
            Name = "Chokisoft Soluciones Tecnológicas",
            Email = "chokisoft@gmail.com",
            Url = new Uri("https://github.com/chokisoft/GoldBusiness")
        }
    });

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

        return new[] { $"{tagPrefix}. {displayName}" };
    });

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

        // ✅ Seed de traducciones automáticas
        logger.LogInformation("🌍 Verificando traducciones...");

        var traduccionesAgregadas = 0;

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
                    new SystemConfigurationTranslation(item.Id, "es", item.NombreNegocio, item.Direccion, item.Municipio, item.Provincia, "system"));
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

        // Establecimientos
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
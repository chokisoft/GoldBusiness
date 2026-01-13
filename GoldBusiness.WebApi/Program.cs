using GoldBusiness.Application.Interfaces;
using GoldBusiness.Application.Services;
using GoldBusiness.Domain.Entities;
using GoldBusiness.Infrastructure.Context;
using GoldBusiness.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// SQL Server + Identity con ApplicationUser
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

// Servicios propios
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ICuentaRepository, CuentaRepository>();
builder.Services.AddScoped<ICuentaService, CuentaService>();
builder.Services.AddScoped<IGrupoCuentaRepository, GrupoCuentaRepository>();
builder.Services.AddScoped<IGrupoCuentaService, GrupoCuentaService>();
builder.Services.AddScoped<ISubGrupoCuentaRepository, SubGrupoCuentaRepository>();
builder.Services.AddScoped<ISubGrupoCuentaService, SubGrupoCuentaService>();

// Read JWT key and issuer from configuration
var jwtIssuer = builder.Configuration["Jwt:Issuer"];
var jwtKey = builder.Configuration["Jwt:Key"];
if (string.IsNullOrWhiteSpace(jwtIssuer) || string.IsNullOrWhiteSpace(jwtKey))
{
    throw new InvalidOperationException("JWT configuration is missing. Set Jwt:Issuer and Jwt:Key.");
}

// JWT - explicit default schemes and claim types
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

// Autorización global
builder.Services.AddAuthorizationBuilder()
    .SetFallbackPolicy(new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build());

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Swagger con soporte JWT
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v2", new OpenApiInfo { Title = "GoldBusiness API", Version = "v2" });

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
            Array.Empty<string>() // 👈 más limpio que new List<string>()
        }
    });
});

var app = builder.Build();

// Seed inicial
using (var scope = app.Services.CreateScope())
{
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    var rolesToEnsure = new[] { "DESARROLLADOR", "ADMINISTRADDR", "ECONOMICO", "CONTADOR" };
    foreach (var roleName in rolesToEnsure)
    {
        if (!await roleManager.RoleExistsAsync(roleName))
        {
            var roleResult = await roleManager.CreateAsync(new IdentityRole(roleName));
            if (!roleResult.Succeeded)
            {
                logger.LogError("Error creating role {Role}: {Errors}", roleName, string.Join(", ", roleResult.Errors.Select(e => e.Description)));
            }
        }
    }

    var user = await userManager.FindByNameAsync("fragela");
    if (user == null)
    {
        var newUser = new ApplicationUser
        {
            UserName = "fragela",
            Email = "fragela@empresa.com",
            EmailConfirmed = true,
            IsActive = true
        };

        var result = await userManager.CreateAsync(newUser, "choki1972,.");
        if (result.Succeeded)
        {
            var addRolesResult = await userManager.AddToRolesAsync(newUser, new[] { "DESARROLLADOR" });
            if (!addRolesResult.Succeeded)
            {
                logger.LogError("Error assigning roles to new user: {Errors}", string.Join(", ", addRolesResult.Errors.Select(e => e.Description)));
            }
        }
        else
        {
            logger.LogError("Error creating user fragela: {Errors}", string.Join(", ", result.Errors.Select(e => e.Description)));
        }
    }
    else
    {
        if (!await userManager.IsInRoleAsync(user, "DESARROLLADOR"))
        {
            var addRoleResult = await userManager.AddToRoleAsync(user, "DESARROLLADOR");
            if (!addRoleResult.Succeeded)
            {
                logger.LogError("Error assigning Desarrollador to existing user: {Errors}", string.Join(", ", addRoleResult.Errors.Select(e => e.Description)));
            }
        }
    }

    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    // GrupoCuenta: si no tiene traducciones, copiar Descripcion -> 'es'
    foreach (var g in db.GrupoCuenta.Where(x => !x.Translations.Any()))
    {
        db.GrupoCuentaTranslation.Add(new GrupoCuentaTranslation(g.Id, "es", g.Descripcion, "system"));
    }

    // SubGrupoCuenta
    foreach (var s in db.SubGrupoCuenta.Where(x => !x.Translations.Any()))
    {
        db.SubGrupoCuentaTranslation.Add(new SubGrupoCuentaTranslation(s.Id, "es", s.Descripcion, "system"));
    }

    // Cuenta
    foreach (var c in db.Cuenta.Where(x => !x.Translations.Any()))
    {
        db.CuentaTranslation.Add(new CuentaTranslation(c.Id, "es", c.Descripcion, "system"));
    }

    await db.SaveChangesAsync();
}

if (app.Environment.IsDevelopment())
{
    app.UseStaticFiles(); // sirve wwwroot
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v2/swagger.json", "GoldBusiness API v2");
        // añadir versión para evitar caché del navegador
        c.InjectStylesheet("/swagger-ui/swagger.css?v=1");
    });
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
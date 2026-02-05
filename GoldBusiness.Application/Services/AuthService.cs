using GoldBusiness.Application.Interfaces;
using GoldBusiness.Domain.DTOs;
using GoldBusiness.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace GoldBusiness.Application.Services
{
    public class AuthService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration config) : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly RoleManager<IdentityRole> _roleManager = roleManager;
        private readonly IConfiguration _config = config;

        public async Task<AuthResponseDTO?> AuthenticateAsync(LoginDTO login)
        {
            // Validar usuario
            var user = await _userManager.FindByNameAsync(login.Username);
            if (user == null || !user.IsActive) return null;

            // Validar contraseña
            var validPassword = await _userManager.CheckPasswordAsync(user, login.Password);
            if (!validPassword) return null;

            // 1. Claims del usuario
            var userClaims = await _userManager.GetClaimsAsync(user);

            // 2. Roles del usuario
            var roles = await _userManager.GetRolesAsync(user);

            // 3. Claims de roles
            var roleClaims = new List<Claim>();
            foreach (var roleName in roles)
            {
                roleClaims.Add(new Claim(ClaimTypes.Role, roleName));

                var roleObj = await _roleManager.FindByNameAsync(roleName);
                if (roleObj != null)
                {
                    var roleClaimsFromDb = await _roleManager.GetClaimsAsync(roleObj);
                    roleClaims.AddRange(roleClaimsFromDb);
                }
            }

            // 4. Claims base del token
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName ?? ""),
                new Claim(ClaimTypes.NameIdentifier, user.Id)
            };

            // 5. Agregar claims del usuario y del rol
            claims.AddRange(userClaims);
            claims.AddRange(roleClaims);

            // 6. Validar configuración JWT
            var jwtKey = _config["Jwt:Key"] 
                ?? throw new InvalidOperationException("JWT Key is not configured in appsettings.json");
            var jwtIssuer = _config["Jwt:Issuer"] 
                ?? throw new InvalidOperationException("JWT Issuer is not configured in appsettings.json");
            var expirationMinutes = _config.GetValue<int>("Jwt:AccessTokenExpirationMinutes", 15);

            // 7. Generar token
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: jwtIssuer,
                audience: null,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expirationMinutes),
                signingCredentials: creds);

            return new AuthResponseDTO
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = token.ValidTo
            };
        }
    }
}
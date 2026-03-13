using GoldBusiness.Domain.DTOs;

namespace GoldBusiness.Application.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponseDTO?> AuthenticateAsync(LoginDTO login);
        Task<AuthResponseDTO?> AuthenticateExternalUserAsync(string userId, string? ipAddress, string? userAgent);
        
        // ✅ NUEVO: Métodos de refresh token
        Task<AuthResponseDTO?> RefreshTokenAsync(string refreshToken, string? ipAddress, string? userAgent);
        
        // ✅ NUEVO: Métodos de revocación
        Task<bool> RevokeTokenAsync(string token, string? ipAddress);
        Task<bool> RevokeAllUserTokensAsync(string userId, string? ipAddress);
        
        // ✅ NUEVO: Limpieza de tokens expirados
        Task<int> CleanExpiredTokensAsync();
    }
}
using GoldBusiness.Domain.DTOs;

namespace GoldBusiness.Application.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponseDTO?> AuthenticateAsync(LoginDTO login);
    }
}

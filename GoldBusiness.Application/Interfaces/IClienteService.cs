using GoldBusiness.Domain.DTOs;

namespace GoldBusiness.Application.Interfaces
{
    public interface IClienteService
    {
        Task<IEnumerable<ClienteDTO>> GetAllAsync(string lang = "es");
        Task<ClienteDTO?> GetByIdAsync(int id, string lang = "es");
        Task<ClienteDTO> CreateAsync(ClienteDTO dto, string user, string lang = "es");
        Task<ClienteDTO> UpdateAsync(int id, ClienteDTO dto, string user, string lang = "es");
        Task<ClienteDTO?> SoftDeleteAsync(int id, string user);

        Task AddOrUpdateTranslationAsync(int id, string lang, string descripcion, string user);
    }
}
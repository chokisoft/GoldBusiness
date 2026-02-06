using GoldBusiness.Domain.DTOs;

namespace GoldBusiness.Application.Interfaces
{
    public interface IMonedaService
    {
        Task<IEnumerable<MonedaDTO>> GetAllAsync(string lang = "es");
        Task<MonedaDTO?> GetByIdAsync(int id, string lang = "es");
        Task<MonedaDTO> CreateAsync(MonedaDTO dto, string user, string lang = "es");
        Task<MonedaDTO> UpdateAsync(int id, MonedaDTO dto, string user, string lang = "es");
        Task<MonedaDTO?> SoftDeleteAsync(int id, string user);

        Task AddOrUpdateTranslationAsync(int id, string lang, string descripcion, string user);
    }
}
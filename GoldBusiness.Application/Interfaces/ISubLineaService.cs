using GoldBusiness.Domain.DTOs;

namespace GoldBusiness.Application.Interfaces
{
    public interface ISubLineaService
    {
        Task<IEnumerable<SubLineaDTO>> GetAllAsync(string lang = "es");
        Task<SubLineaDTO?> GetByIdAsync(int id, string lang = "es");
        Task<SubLineaDTO> CreateAsync(SubLineaDTO dto, string user, string lang = "es");
        Task<SubLineaDTO> UpdateAsync(int id, SubLineaDTO dto, string user, string lang = "es");
        Task<SubLineaDTO?> SoftDeleteAsync(int id, string user);

        Task AddOrUpdateTranslationAsync(int id, string lang, string descripcion, string user);
    }
}
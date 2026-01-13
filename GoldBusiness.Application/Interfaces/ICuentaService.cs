using GoldBusiness.Domain.DTOs;

namespace GoldBusiness.Application.Interfaces
{
    public interface ICuentaService
    {
        Task<IEnumerable<CuentaDTO>> GetAllAsync(string lang = "es");
        Task<CuentaDTO?> GetByIdAsync(int id, string lang = "es");
        Task<CuentaDTO> CreateAsync(CuentaDTO dto, string user, string lang = "es");
        Task<CuentaDTO> UpdateAsync(int id, CuentaDTO dto, string user, string lang = "es");
        Task<CuentaDTO?> SoftDeleteAsync(int id, string user);

        Task AddOrUpdateTranslationAsync(int id, string lang, string descripcion, string user);
    }
}
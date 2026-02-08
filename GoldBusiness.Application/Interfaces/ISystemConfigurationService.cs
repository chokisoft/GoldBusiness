using GoldBusiness.Domain.DTOs;

namespace GoldBusiness.Application.Interfaces
{
    public interface ISystemConfigurationService
    {
        Task<IEnumerable<SystemConfigurationDTO>> GetAllAsync(string lang = "es");
        Task<SystemConfigurationDTO?> GetByIdAsync(int id, string lang = "es");
        Task<SystemConfigurationDTO> CreateAsync(SystemConfigurationDTO dto, string user, string lang = "es");
        Task<SystemConfigurationDTO> UpdateAsync(int id, SystemConfigurationDTO dto, string user, string lang = "es");

        Task AddOrUpdateTranslationAsync(int id, string lang, string nombreNegocio, string? direccion, string user);
    }
}
using GoldBusiness.Domain.DTOs;

namespace GoldBusiness.Application.Interfaces
{
    public interface ILocalidadService
    {
        Task<IEnumerable<LocalidadDTO>> GetAllAsync(string lang = "es");
        Task<IEnumerable<LocalidadDTO>> GetByEstablecimientoIdAsync(int establecimientoId, string lang = "es");
        Task<LocalidadDTO?> GetByIdAsync(int id, string lang = "es");
        Task<LocalidadDTO> CreateAsync(LocalidadDTO dto, string user, string lang = "es");
        Task<LocalidadDTO> UpdateAsync(int id, LocalidadDTO dto, string user, string lang = "es");
        Task<LocalidadDTO?> SoftDeleteAsync(int id, string user);
        Task AddOrUpdateTranslationAsync(int id, string lang, string descripcion, string user);
    }
}
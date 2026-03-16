using GoldBusiness.Domain.DTOs;

namespace GoldBusiness.Application.Interfaces
{
    public interface IProveedorService
    {
        Task<IEnumerable<ProveedorDTO>> GetAllAsync(string lang = "es");
        Task<(IEnumerable<ProveedorDTO> Items, int Total)> GetPagedAsync(int page, int pageSize, string? search, string lang = "es");
        Task<ProveedorDTO?> GetByIdAsync(int id, string lang = "es");
        Task<ProveedorDTO> CreateAsync(ProveedorDTO dto, string user, string lang = "es");
        Task<ProveedorDTO> UpdateAsync(int id, ProveedorDTO dto, string user, string lang = "es");
        Task<ProveedorDTO?> SoftDeleteAsync(int id, string user);

        Task AddOrUpdateTranslationAsync(int id, string lang, string descripcion, string user);
    }
}
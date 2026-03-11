using GoldBusiness.Domain.DTOs;

namespace GoldBusiness.Application.Interfaces
{
    public interface ILineaService
    {
        Task<IEnumerable<LineaDTO>> GetAllAsync(string lang = "es");
        Task<(IEnumerable<LineaDTO> Items, int Total)> GetPagedAsync(int page, int pageSize, string? termino = null, string lang = "es");
        Task<LineaDTO?> GetByIdAsync(int id, string lang = "es");
        Task<LineaDTO> CreateAsync(LineaDTO dto, string user, string lang = "es");
        Task<LineaDTO> UpdateAsync(int id, LineaDTO dto, string user, string lang = "es");
        Task<LineaDTO?> SoftDeleteAsync(int id, string user);
        Task AddOrUpdateTranslationAsync(int id, string lang, string descripcion, string user);
    }
}
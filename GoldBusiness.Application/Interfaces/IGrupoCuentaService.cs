using GoldBusiness.Domain.DTOs;

namespace GoldBusiness.Application.Interfaces
{
    public interface IGrupoCuentaService
    {
        Task<IEnumerable<GrupoCuentaDTO>> GetAllAsync(string lang = "es");
        Task<(IEnumerable<GrupoCuentaDTO> Items, int Total)> GetPagedAsync(int page, int pageSize, string? termino = null, string lang = "es");
        Task<GrupoCuentaDTO?> GetByIdAsync(int id, string lang = "es");
        Task<GrupoCuentaDTO> CreateAsync(GrupoCuentaDTO dto, string user, string lang = "es");
        Task<GrupoCuentaDTO> UpdateAsync(int id, GrupoCuentaDTO dto, string user, string lang = "es");
        Task<GrupoCuentaDTO?> SoftDeleteAsync(int id, string user);

        Task AddOrUpdateTranslationAsync(int id, string lang, string descripcion, string user);
    }
}
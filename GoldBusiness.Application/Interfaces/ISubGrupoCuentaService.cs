using GoldBusiness.Domain.DTOs;

namespace GoldBusiness.Application.Interfaces
{
    public interface ISubGrupoCuentaService
    {
        Task<IEnumerable<SubGrupoCuentaDTO>> GetAllAsync(string lang = "es");
        Task<(IEnumerable<SubGrupoCuentaDTO> Items, int Total)> GetPagedAsync(int page, int pageSize, string? termino = null, int? grupoCuentaId = null, string lang = "es");
        Task<SubGrupoCuentaDTO?> GetByIdAsync(int id, string lang = "es");
        Task<SubGrupoCuentaDTO> CreateAsync(SubGrupoCuentaDTO dto, string user, string lang = "es");
        Task<SubGrupoCuentaDTO> UpdateAsync(int id, SubGrupoCuentaDTO dto, string user, string lang = "es");
        Task<SubGrupoCuentaDTO?> SoftDeleteAsync(int id, string user);
        Task AddOrUpdateTranslationAsync(int id, string lang, string descripcion, string user);
    }
}
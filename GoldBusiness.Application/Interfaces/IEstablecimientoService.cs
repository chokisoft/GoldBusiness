using GoldBusiness.Domain.DTOs;

namespace GoldBusiness.Application.Interfaces
{
    public interface IEstablecimientoService
    {
        Task<IEnumerable<EstablecimientoDTO>> GetAllAsync(string lang = "es");
        Task<(IEnumerable<EstablecimientoDTO> Items, int Total)> GetPagedAsync(int page, int pageSize, string? termino = null, int? negocioId = null, string lang = "es");
        Task<EstablecimientoDTO?> GetByIdAsync(int id, string lang = "es");
        Task<EstablecimientoDTO> CreateAsync(EstablecimientoDTO dto, string user, string lang = "es");
        Task<EstablecimientoDTO> UpdateAsync(int id, EstablecimientoDTO dto, string user, string lang = "es");
        Task<EstablecimientoDTO?> SoftDeleteAsync(int id, string user);
        Task AddOrUpdateTranslationAsync(int id, string lang, string descripcion, string user);
    }
}
using GoldBusiness.Domain.DTOs;

namespace GoldBusiness.Application.Interfaces
{
    public interface ICodigoPostalService
    {
        Task<IEnumerable<CodigoPostalDTO>> GetAllAsync();
        Task<(IEnumerable<CodigoPostalDTO> Items, int Total)> GetPagedAsync(int page, int pageSize, string? termino = null, int? municipioId = null, string lang = "es");
        Task<IEnumerable<CodigoPostalDTO>> GetByMunicipioIdAsync(int municipioId, string lang = "es");
        Task<CodigoPostalDTO?> GetByIdAsync(int id, string lang = "es");
        Task<CodigoPostalDTO> CreateAsync(CodigoPostalDTO dto, string lang, string creadoPor);
        Task<CodigoPostalDTO> UpdateAsync(int id, CodigoPostalDTO dto, string lang, string creadoPor);
        Task<CodigoPostalDTO?> SoftDeleteAsync(int id, string user);
        Task<IEnumerable<CodigoPostalDTO>> BuscarAsync(string termino, int? municipioId = null, string lang = "es");
    }
}
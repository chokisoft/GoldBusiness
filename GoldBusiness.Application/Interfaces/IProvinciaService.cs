using GoldBusiness.Domain.DTOs;

namespace GoldBusiness.Application.Interfaces
{
    public interface IProvinciaService
    {
        Task<IEnumerable<ProvinciaDTO>> GetAllAsync(string lang = "es");
        Task<(IEnumerable<ProvinciaDTO> Items, int Total)> GetPagedAsync(int page, int pageSize, string? termino = null, int? paisId = null, string lang = "es");
        Task<IEnumerable<ProvinciaDTO>> GetByProvinciaIdAsync(int paisId, string lang = "es");
        Task<IEnumerable<ProvinciaDTO>> GetByPaisIdAsync(int paisId, string lang = "es");
        Task<ProvinciaDTO?> GetByIdAsync(int id, string lang = "es");
        Task<IEnumerable<ProvinciaDTO>> BuscarAsync(string termino, int? paisId = null, string lang = "es");
    }
}
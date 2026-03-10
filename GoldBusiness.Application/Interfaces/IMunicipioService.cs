using GoldBusiness.Domain.DTOs;

namespace GoldBusiness.Application.Interfaces
{
    public interface IMunicipioService
    {
        Task<IEnumerable<MunicipioDTO>> GetAllAsync(string lang = "es");
        Task<(IEnumerable<MunicipioDTO> Items, int Total)> GetPagedAsync(int page, int pageSize, string? termino = null, int? provinciaId = null, string lang = "es");
        Task<IEnumerable<MunicipioDTO>> GetByProvinciaIdAsync(int provinciaId, string lang = "es");
        Task<MunicipioDTO?> GetByIdAsync(int id, string lang = "es");
        Task<IEnumerable<MunicipioDTO>> BuscarAsync(string termino, int? provinciaId = null, string lang = "es");
    }
}
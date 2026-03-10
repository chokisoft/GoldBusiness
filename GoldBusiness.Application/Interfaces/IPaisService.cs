using GoldBusiness.Domain.DTOs;

namespace GoldBusiness.Application.Interfaces
{
    public interface IPaisService
    {
        Task<IEnumerable<PaisDTO>> GetAllAsync(string lang = "es");
        Task<(IEnumerable<PaisDTO> Items, int Total)> GetPagedAsync(int page, int pageSize, string? termino = null, string lang = "es");
        Task<PaisDTO?> GetByIdAsync(int id, string lang = "es");
        Task<PaisDTO?> GetByCodigoAsync(string codigo, string lang = "es");
    }
}
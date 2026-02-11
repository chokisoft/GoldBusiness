using GoldBusiness.Domain.DTOs;

namespace GoldBusiness.Application.Interfaces
{
    public interface IFichaProductoService
    {
        Task<IEnumerable<FichaProductoDTO>> GetAllAsync(string lang = "es");
        Task<IEnumerable<FichaProductoDTO>> GetByProductoIdAsync(int productoId, string lang = "es");
        Task<IEnumerable<FichaProductoDTO>> GetByLocalidadIdAsync(int localidadId, string lang = "es");
        Task<FichaProductoDTO?> GetByIdAsync(int id, string lang = "es");
        Task<FichaProductoDTO> CreateAsync(FichaProductoDTO dto, string user, string lang = "es");
        Task<FichaProductoDTO> UpdateAsync(int id, FichaProductoDTO dto, string user, string lang = "es");
        Task<FichaProductoDTO?> SoftDeleteAsync(int id, string user);
    }
}
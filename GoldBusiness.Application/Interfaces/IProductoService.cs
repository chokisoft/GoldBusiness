using GoldBusiness.Domain.DTOs;

namespace GoldBusiness.Application.Interfaces
{
    public interface IProductoService
    {
        Task<IEnumerable<ProductoDTO>> GetAllAsync(string lang = "es");
        Task<IEnumerable<ProductoDTO>> GetByEstablecimientoIdAsync(int establecimientoId, string lang = "es");
        Task<IEnumerable<ProductoDTO>> GetByProveedorIdAsync(int proveedorId, string lang = "es");
        Task<IEnumerable<ProductoDTO>> GetBySubLineaIdAsync(int subLineaId, string lang = "es");
        Task<ProductoDTO?> GetByIdAsync(int id, string lang = "es");
        Task<ProductoDTO> CreateAsync(ProductoDTO dto, string user, string lang = "es");
        Task<ProductoDTO> UpdateAsync(int id, ProductoDTO dto, string user, string lang = "es");
        Task<ProductoDTO?> SoftDeleteAsync(int id, string user);
        Task AddOrUpdateTranslationAsync(int id, string lang, string descripcion, string user);
    }
}
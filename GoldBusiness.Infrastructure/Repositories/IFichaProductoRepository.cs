using GoldBusiness.Domain.Entities;

namespace GoldBusiness.Infrastructure.Repositories
{
    public interface IFichaProductoRepository
    {
        Task<IEnumerable<FichaProducto>> GetAllAsync();
        Task<IEnumerable<FichaProducto>> GetByProductoIdAsync(int productoId);
        Task<IEnumerable<FichaProducto>> GetByLocalidadIdAsync(int localidadId);
        Task<FichaProducto?> GetByIdAsync(int id);
        Task<FichaProducto?> GetByComposicionAsync(int productoId, int componenteId, int localidadId);
        Task<bool> ExistsComposicionAsync(int productoId, int componenteId, int localidadId, int? excludeId = null);
        Task<FichaProducto> AddAsync(FichaProducto entity);
        Task UpdateAsync(FichaProducto entity);
    }
}
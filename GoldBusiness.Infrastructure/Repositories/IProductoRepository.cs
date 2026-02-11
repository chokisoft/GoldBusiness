using GoldBusiness.Domain.Entities;

namespace GoldBusiness.Infrastructure.Repositories
{
    public interface IProductoRepository : IBaseRepositoryWithCode<Producto>
    {
        Task<IEnumerable<Producto>> GetAllAsync();
        Task<IEnumerable<Producto>> GetByEstablecimientoIdAsync(int establecimientoId);
        Task<IEnumerable<Producto>> GetByProveedorIdAsync(int proveedorId);
        Task<IEnumerable<Producto>> GetBySubLineaIdAsync(int subLineaId);
        Task<Producto?> GetByIdAsync(int id);
        Task<Producto> AddAsync(Producto entity);
        Task UpdateAsync(Producto entity);
    }
}
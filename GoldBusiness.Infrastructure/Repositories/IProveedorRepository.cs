using GoldBusiness.Domain.Entities;

namespace GoldBusiness.Infrastructure.Repositories
{
    public interface IProveedorRepository : IBaseRepositoryWithCode<Proveedor>
    {
        Task<IEnumerable<Proveedor>> GetAllAsync();
        Task<Proveedor?> GetByIdAsync(int id);
        Task AddAsync(Proveedor entity);
        Task UpdateAsync(Proveedor entity);
    }
}
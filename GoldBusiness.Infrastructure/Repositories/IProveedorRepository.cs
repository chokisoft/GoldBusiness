using GoldBusiness.Domain.Entities;
using System.Linq;

namespace GoldBusiness.Infrastructure.Repositories
{
    public interface IProveedorRepository : IBaseRepositoryWithCode<Proveedor>
    {
        Task<IEnumerable<Proveedor>> GetAllAsync();
        Task<(IEnumerable<Proveedor> Items, int Total)> GetPagedAsync(int page, int pageSize, string? termino = null);
        Task<Proveedor?> GetByIdAsync(int id);
        Task AddAsync(Proveedor entity);
        Task UpdateAsync(Proveedor entity);
    }
}
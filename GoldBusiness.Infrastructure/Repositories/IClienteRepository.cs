using GoldBusiness.Domain.Entities;
using System.Linq;

namespace GoldBusiness.Infrastructure.Repositories
{
    public interface IClienteRepository : IBaseRepositoryWithCode<Cliente>
    {
        Task<IEnumerable<Cliente>> GetAllAsync();
        Task<(IEnumerable<Cliente> Items, int Total)> GetPagedAsync(int page, int pageSize, string? search = null);
        Task<Cliente?> GetByIdAsync(int id);
        Task AddAsync(Cliente entity);
        Task UpdateAsync(Cliente entity);

    }
}
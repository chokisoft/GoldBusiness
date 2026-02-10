using GoldBusiness.Domain.Entities;

namespace GoldBusiness.Infrastructure.Repositories
{
    public interface IClienteRepository : IBaseRepositoryWithCode<Cliente>
    {
        Task<IEnumerable<Cliente>> GetAllAsync();
        Task<Cliente?> GetByIdAsync(int id);
        Task AddAsync(Cliente entity);
        Task UpdateAsync(Cliente entity);
    }
}
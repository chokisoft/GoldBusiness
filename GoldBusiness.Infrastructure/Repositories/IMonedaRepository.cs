using GoldBusiness.Domain.Entities;

namespace GoldBusiness.Infrastructure.Repositories
{
    public interface IMonedaRepository : IBaseRepositoryWithCode<Moneda>
    {
        Task<IEnumerable<Moneda>> GetAllAsync();
        Task<Moneda?> GetByIdAsync(int id);
        Task AddAsync(Moneda entity);
        Task UpdateAsync(Moneda entity);
    }
}
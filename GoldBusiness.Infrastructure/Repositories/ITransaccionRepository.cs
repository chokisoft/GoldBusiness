using GoldBusiness.Domain.Entities;

namespace GoldBusiness.Infrastructure.Repositories
{
    public interface ITransaccionRepository : IBaseRepositoryWithCode<Transaccion>
    {
        Task<IEnumerable<Transaccion>> GetAllAsync();
        Task<Transaccion?> GetByIdAsync(int id);
        Task<Transaccion> AddAsync(Transaccion entity);
        Task UpdateAsync(Transaccion entity);
    }
}
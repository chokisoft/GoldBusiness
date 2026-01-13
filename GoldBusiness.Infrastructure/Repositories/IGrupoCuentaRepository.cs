using GoldBusiness.Domain.Entities;

namespace GoldBusiness.Infrastructure.Repositories
{
    public interface IGrupoCuentaRepository
    {
        Task<IEnumerable<GrupoCuenta>> GetAllAsync();
        Task<GrupoCuenta?> GetByIdAsync(int id);
        Task AddAsync(GrupoCuenta entity);
        Task UpdateAsync(GrupoCuenta entity);
    }
}

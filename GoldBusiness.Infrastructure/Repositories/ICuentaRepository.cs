using GoldBusiness.Domain.Entities;

namespace GoldBusiness.Infrastructure.Repositories
{
    public interface ICuentaRepository : IBaseRepositoryWithCode<Cuenta>
    {
        Task<IEnumerable<Cuenta>> GetAllAsync();
        Task<Cuenta?> GetByIdAsync(int id);
        Task AddAsync(Cuenta entity);
        Task UpdateAsync(Cuenta entity);
    }
}
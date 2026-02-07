using GoldBusiness.Domain.Entities;

namespace GoldBusiness.Infrastructure.Repositories
{
    public interface ISubGrupoCuentaRepository : IBaseRepositoryWithCode<SubGrupoCuenta>
    {
        Task<IEnumerable<SubGrupoCuenta>> GetAllAsync();
        Task<SubGrupoCuenta?> GetByIdAsync(int id);
        Task AddAsync(SubGrupoCuenta entity);
        Task UpdateAsync(SubGrupoCuenta entity);
    }
}
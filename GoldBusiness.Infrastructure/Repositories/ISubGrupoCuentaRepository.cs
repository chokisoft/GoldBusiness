using GoldBusiness.Domain.Entities;

namespace GoldBusiness.Infrastructure.Repositories
{
    public interface ISubGrupoCuentaRepository : IBaseRepositoryWithCode<SubGrupoCuenta>
    {
        Task<IEnumerable<SubGrupoCuenta>> GetAllAsync();
        Task<(IEnumerable<SubGrupoCuenta> Items, int Total)> GetPagedAsync(int page, int pageSize, string? termino = null, int? grupoCuentaId = null);
        Task<SubGrupoCuenta?> GetByIdAsync(int id);
        Task AddAsync(SubGrupoCuenta entity);
        Task UpdateAsync(SubGrupoCuenta entity);
    }
}
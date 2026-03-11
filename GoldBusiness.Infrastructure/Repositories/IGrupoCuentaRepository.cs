using GoldBusiness.Domain.Entities;

namespace GoldBusiness.Infrastructure.Repositories
{
    public interface IGrupoCuentaRepository : IBaseRepositoryWithCode<GrupoCuenta>
    {
        Task<IEnumerable<GrupoCuenta>> GetAllAsync();
        Task<(IEnumerable<GrupoCuenta> Items, int Total)> GetPagedAsync(int page, int pageSize, string? termino = null);
        Task<GrupoCuenta?> GetByIdAsync(int id);
        Task AddAsync(GrupoCuenta entity);
        Task UpdateAsync(GrupoCuenta entity);
    }
}

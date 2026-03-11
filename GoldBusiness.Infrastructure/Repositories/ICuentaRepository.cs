using GoldBusiness.Domain.Entities;

namespace GoldBusiness.Infrastructure.Repositories
{
    public interface ICuentaRepository : IBaseRepositoryWithCode<Cuenta>
    {
        Task<IEnumerable<Cuenta>> GetAllAsync();
        Task<(IEnumerable<Cuenta> Items, int Total)> GetPagedAsync(int page, int pageSize, string? termino = null, int? subGrupoCuentaId = null);
        Task<Cuenta?> GetByIdAsync(int id);
        Task AddAsync(Cuenta entity);
        Task UpdateAsync(Cuenta entity);
    }
}
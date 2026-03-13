using GoldBusiness.Domain.Entities;

namespace GoldBusiness.Infrastructure.Repositories
{
    public interface ICuentaRepository : IBaseRepositoryWithCode<Cuenta>
    {
        Task<IEnumerable<Cuenta>> GetAllAsync(IReadOnlyCollection<string>? accessLevels = null);
        Task<(IEnumerable<Cuenta> Items, int Total)> GetPagedAsync(int page, int pageSize, string? termino = null, int? subGrupoCuentaId = null, IReadOnlyCollection<string>? accessLevels = null);
        Task<Cuenta?> GetByIdAsync(int id, IReadOnlyCollection<string>? accessLevels = null);
        Task AddAsync(Cuenta entity);
        Task UpdateAsync(Cuenta entity);
    }
}
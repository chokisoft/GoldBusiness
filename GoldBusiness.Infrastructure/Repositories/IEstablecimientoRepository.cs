using GoldBusiness.Domain.Entities;

namespace GoldBusiness.Infrastructure.Repositories
{
    public interface IEstablecimientoRepository : IBaseRepositoryWithCode<Establecimiento>
    {
        Task<IEnumerable<Establecimiento>> GetAllAsync();
        Task<(IEnumerable<Establecimiento> Items, int Total)> GetPagedAsync(int page, int pageSize, string? termino = null, int? negocioId = null);
        Task<IEnumerable<Establecimiento>> GetByNegocioIdAsync(int negocioId);
        Task<Establecimiento?> GetByIdAsync(int id);
        Task<Establecimiento> AddAsync(Establecimiento entity);
        Task UpdateAsync(Establecimiento entity);
    }
}
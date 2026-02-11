using GoldBusiness.Domain.Entities;

namespace GoldBusiness.Infrastructure.Repositories
{
    public interface IEstablecimientoRepository : IBaseRepositoryWithCode<Establecimiento>
    {
        Task<IEnumerable<Establecimiento>> GetAllAsync();
        Task<Establecimiento?> GetByIdAsync(int id);
        Task<Establecimiento> AddAsync(Establecimiento entity);
        Task UpdateAsync(Establecimiento entity);
    }
}
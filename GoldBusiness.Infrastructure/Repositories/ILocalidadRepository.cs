using GoldBusiness.Domain.Entities;

namespace GoldBusiness.Infrastructure.Repositories
{
    public interface ILocalidadRepository : IBaseRepositoryWithCode<Localidad>
    {
        Task<IEnumerable<Localidad>> GetAllAsync();
        Task<IEnumerable<Localidad>> GetByEstablecimientoIdAsync(int establecimientoId);
        Task<Localidad?> GetByIdAsync(int id);
        Task<Localidad> AddAsync(Localidad entity);
        Task UpdateAsync(Localidad entity);
    }
}
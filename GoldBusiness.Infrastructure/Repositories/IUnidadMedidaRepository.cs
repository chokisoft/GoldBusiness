using GoldBusiness.Domain.Entities;

namespace GoldBusiness.Infrastructure.Repositories
{
    public interface IUnidadMedidaRepository : IBaseRepositoryWithCode<UnidadMedida>
    {
        Task<IEnumerable<UnidadMedida>> GetAllAsync();
        Task<UnidadMedida?> GetByIdAsync(int id);
        Task<UnidadMedida> AddAsync(UnidadMedida entity);
        Task UpdateAsync(UnidadMedida entity);
    }
}
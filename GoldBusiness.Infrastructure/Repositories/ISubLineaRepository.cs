using GoldBusiness.Domain.Entities;

namespace GoldBusiness.Infrastructure.Repositories
{
    public interface ISubLineaRepository : IBaseRepositoryWithCode<SubLinea>
    {
        Task<IEnumerable<SubLinea>> GetAllAsync();
        Task<SubLinea?> GetByIdAsync(int id);
        Task AddAsync(SubLinea entity);
        Task UpdateAsync(SubLinea entity);
    }
}
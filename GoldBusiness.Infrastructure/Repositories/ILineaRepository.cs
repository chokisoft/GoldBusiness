using GoldBusiness.Domain.Entities;

namespace GoldBusiness.Infrastructure.Repositories
{
    public interface ILineaRepository
    {
        Task<IEnumerable<Linea>> GetAllAsync();
        Task<Linea?> GetByIdAsync(int id);
        Task AddAsync(Linea entity);
        Task UpdateAsync(Linea entity);
    }
}
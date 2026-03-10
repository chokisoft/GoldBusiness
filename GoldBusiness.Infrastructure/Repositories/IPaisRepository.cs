using GoldBusiness.Domain.Entities;

namespace GoldBusiness.Infrastructure.Repositories
{
    public interface IPaisRepository
    {
        Task<IEnumerable<Pais>> GetAllAsync();
        Task<(IEnumerable<Pais> Items, int Total)> GetPagedAsync(int page, int pageSize, string? termino = null);
        Task<Pais?> GetByIdAsync(int id);
        Task<Pais?> GetByCodigoAsync(string codigo);
        Task<Pais> AddAsync(Pais entity);
        Task UpdateAsync(Pais entity);
    }
}
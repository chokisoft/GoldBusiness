using GoldBusiness.Domain.Entities;

namespace GoldBusiness.Infrastructure.Repositories
{
    public interface IPaisRepository
    {
        Task<IEnumerable<Pais>> GetAllAsync();
        Task<Pais?> GetByIdAsync(int id);
        Task<Pais?> GetByCodigoAsync(string codigo);
    }
}
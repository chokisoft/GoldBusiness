using GoldBusiness.Domain.Entities;

namespace GoldBusiness.Infrastructure.Repositories
{
    public interface ICuentaRepository
    {
        Task<IEnumerable<Cuenta>> GetAllAsync();
        Task<Cuenta?> GetByIdAsync(int id);
        Task AddAsync(Cuenta entity);
        Task UpdateAsync(Cuenta entity);
    }
}
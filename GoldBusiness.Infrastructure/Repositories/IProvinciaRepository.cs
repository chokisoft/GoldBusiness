using GoldBusiness.Domain.Entities;

namespace GoldBusiness.Infrastructure.Repositories
{
    public interface IProvinciaRepository
    {
        Task<IEnumerable<Provincia>> GetAllAsync();
        Task<IEnumerable<Provincia>> GetByPaisIdAsync(int paisId);
        Task<Provincia?> GetByIdAsync(int id);
        Task<IEnumerable<Provincia>> BuscarAsync(string termino, int? paisId = null);
    }
}
using GoldBusiness.Domain.Entities;

namespace GoldBusiness.Infrastructure.Repositories
{
    public interface IProvinciaRepository
    {
        Task<IEnumerable<Provincia>> GetAllAsync();
        Task<(IEnumerable<Provincia> Items, int Total)> GetPagedAsync(int page, int pageSize, string? termino = null, int? paisId = null);
        Task<IEnumerable<Provincia>> GetByPaisIdAsync(int paisId);
        Task<Provincia?> GetByIdAsync(int id);
        Task<IEnumerable<Provincia>> BuscarAsync(string termino, int? paisId = null);
        Task<Provincia> AddAsync(Provincia entity);
        Task UpdateAsync(Provincia entity);
    }
}
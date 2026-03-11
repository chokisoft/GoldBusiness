using GoldBusiness.Domain.Entities;

namespace GoldBusiness.Infrastructure.Repositories
{
    public interface ILineaRepository : IBaseRepositoryWithCode<Linea>
    {
        Task<IEnumerable<Linea>> GetAllAsync();
        Task<(IEnumerable<Linea> Items, int Total)> GetPagedAsync(int page, int pageSize, string? termino = null);
        Task<Linea?> GetByIdAsync(int id);
        Task AddAsync(Linea entity);
        Task UpdateAsync(Linea entity);
    }
}
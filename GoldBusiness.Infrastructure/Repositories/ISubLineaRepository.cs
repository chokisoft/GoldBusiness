using GoldBusiness.Domain.Entities;

namespace GoldBusiness.Infrastructure.Repositories
{
    public interface ISubLineaRepository : IBaseRepositoryWithCode<SubLinea>
    {
        Task<IEnumerable<SubLinea>> GetAllAsync();
        Task<(IEnumerable<SubLinea> Items, int Total)> GetPagedAsync(int page, int pageSize, string? termino = null, int? lineaId = null);
        Task<SubLinea?> GetByIdAsync(int id);
        Task AddAsync(SubLinea entity);
        Task UpdateAsync(SubLinea entity);
    }
}
using GoldBusiness.Domain.Entities;

namespace GoldBusiness.Infrastructure.Repositories
{
    public interface IConceptoAjusteRepository : IBaseRepositoryWithCode<ConceptoAjuste>
    {
        Task<IEnumerable<ConceptoAjuste>> GetAllAsync();
        Task<ConceptoAjuste?> GetByIdAsync(int id);
        Task AddAsync(ConceptoAjuste entity);
        Task UpdateAsync(ConceptoAjuste entity);
    }
}
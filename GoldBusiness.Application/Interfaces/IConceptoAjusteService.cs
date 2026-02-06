using GoldBusiness.Domain.DTOs;

namespace GoldBusiness.Application.Interfaces
{
    public interface IConceptoAjusteService
    {
        Task<IEnumerable<ConceptoAjusteDTO>> GetAllAsync(string lang = "es");
        Task<ConceptoAjusteDTO?> GetByIdAsync(int id, string lang = "es");
        Task<ConceptoAjusteDTO> CreateAsync(ConceptoAjusteDTO dto, string user, string lang = "es");
        Task<ConceptoAjusteDTO> UpdateAsync(int id, ConceptoAjusteDTO dto, string user, string lang = "es");
        Task<ConceptoAjusteDTO?> SoftDeleteAsync(int id, string user);

        Task AddOrUpdateTranslationAsync(int id, string lang, string descripcion, string user);
    }
}
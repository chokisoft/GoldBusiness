using GoldBusiness.Domain.DTOs;
using GoldBusiness.Infrastructure.Repositories;

namespace GoldBusiness.Application.Interfaces
{
    public interface IUnidadMedidaService
    {
        Task<IEnumerable<UnidadMedidaDTO>> GetAllAsync(string lang = "es");
        Task<UnidadMedidaDTO?> GetByIdAsync(int id, string lang = "es");
        Task<UnidadMedidaDTO> CreateAsync(UnidadMedidaDTO dto, string user, string lang = "es");
        Task<UnidadMedidaDTO> UpdateAsync(int id, UnidadMedidaDTO dto, string user, string lang = "es");
        Task<UnidadMedidaDTO?> SoftDeleteAsync(int id, string user);
        Task AddOrUpdateTranslationAsync(int id, string lang, string descripcion, string user);
    }
}
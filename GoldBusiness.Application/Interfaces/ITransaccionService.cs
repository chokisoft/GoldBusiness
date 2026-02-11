using GoldBusiness.Domain.DTOs;

namespace GoldBusiness.Application.Interfaces
{
    public interface ITransaccionService
    {
        Task<IEnumerable<TransaccionDTO>> GetAllAsync(string lang = "es");
        Task<TransaccionDTO?> GetByIdAsync(int id, string lang = "es");
        Task<TransaccionDTO> CreateAsync(TransaccionDTO dto, string user, string lang = "es");
        Task<TransaccionDTO> UpdateAsync(int id, TransaccionDTO dto, string user, string lang = "es");
        Task AddOrUpdateTranslationAsync(int id, string lang, string descripcion, string user);
    }
}
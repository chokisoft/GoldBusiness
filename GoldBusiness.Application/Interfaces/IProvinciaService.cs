using GoldBusiness.Domain.DTOs;

namespace GoldBusiness.Application.Interfaces
{
    public interface IProvinciaService
    {
        Task<IEnumerable<ProvinciaDTO>> GetAllAsync(string lang = "es");
        Task<IEnumerable<ProvinciaDTO>> GetByPaisIdAsync(int paisId, string lang = "es");
        Task<ProvinciaDTO?> GetByIdAsync(int id, string lang = "es");
    }
}
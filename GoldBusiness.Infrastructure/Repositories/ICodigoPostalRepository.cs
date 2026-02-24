using GoldBusiness.Domain.Entities;

namespace GoldBusiness.Infrastructure.Repositories
{
    public interface ICodigoPostalRepository
    {
        Task<IEnumerable<CodigoPostal>> GetAllAsync();
        Task<IEnumerable<CodigoPostal>> GetByMunicipioIdAsync(int municipioId);
        Task<CodigoPostal?> GetByIdAsync(int id);
        Task<CodigoPostal?> GetByCodigoAsync(string codigo, bool includeCanceled = false);
        Task<IEnumerable<CodigoPostal>> BuscarAsync(string termino, int? municipioId = null);
        Task<CodigoPostal> AddAsync(CodigoPostal entity);
        Task UpdateAsync(CodigoPostal entity);
    }
}
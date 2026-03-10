using GoldBusiness.Domain.Entities;

namespace GoldBusiness.Infrastructure.Repositories
{
    public interface ICodigoPostalRepository
    {
        Task<IEnumerable<CodigoPostal>> GetAllAsync();
        Task<(IEnumerable<CodigoPostal> Items, int Total)> GetPagedAsync(int page, int pageSize, string termino = null, int? municipioId = null);
        Task<IEnumerable<CodigoPostal>> GetByMunicipioIdAsync(int municipioId);
        Task<CodigoPostal?> GetByIdAsync(int id);
        Task<CodigoPostal?> GetByCodigoAsync(string codigo, bool includeCanceled = false);
        Task<CodigoPostal> AddAsync(CodigoPostal entity);
        Task<IEnumerable<CodigoPostal>> BuscarAsync(string termino, int? municipioId = null);
        Task UpdateAsync(CodigoPostal entity);
    }
}
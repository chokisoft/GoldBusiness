using GoldBusiness.Domain.Entities;

namespace GoldBusiness.Infrastructure.Repositories
{
    public interface IMunicipioRepository
    {
        Task<IEnumerable<Municipio>> GetAllAsync();
        Task<IEnumerable<Municipio>> GetByProvinciaIdAsync(int provinciaId);
        Task<Municipio?> GetByIdAsync(int id);
        Task<IEnumerable<Municipio>> BuscarAsync(string termino, int? paisId = null);
    }
}
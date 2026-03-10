using GoldBusiness.Domain.Entities;

namespace GoldBusiness.Infrastructure.Repositories
{
    public interface IMunicipioRepository
    {
        Task<IEnumerable<Municipio>> GetAllAsync();
        Task<(IEnumerable<Municipio> Items, int Total)> GetPagedAsync(int page, int pageSize, string? termino = null, int? provinciaId = null);
        Task<IEnumerable<Municipio>> GetByProvinciaIdAsync(int provinciaId);
        Task<Municipio?> GetByIdAsync(int id);
        Task<IEnumerable<Municipio>> BuscarAsync(string termino, int? paisId = null);
        Task<Municipio> AddAsync(Municipio entity);
        Task UpdateAsync(Municipio entity);
    }
}
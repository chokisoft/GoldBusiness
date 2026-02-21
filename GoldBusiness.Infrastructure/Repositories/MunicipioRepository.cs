using GoldBusiness.Domain.Entities;
using GoldBusiness.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace GoldBusiness.Infrastructure.Repositories
{
    public class MunicipioRepository : IMunicipioRepository
    {
        private readonly ApplicationDbContext _context;

        public MunicipioRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Municipio>> GetAllAsync()
            => await _context.Municipio
                .Where(m => !m.Cancelado)
                .Include(m => m.Provincia)
                    .ThenInclude(p => p.Translations)
                .Include(m => m.Provincia)
                    .ThenInclude(p => p.Pais)
                .Include(m => m.Translations)
                .OrderBy(m => m.Descripcion)
                .ToListAsync();

        public async Task<IEnumerable<Municipio>> GetByProvinciaIdAsync(int provinciaId)
            => await _context.Municipio
                .Where(m => m.ProvinciaId == provinciaId && !m.Cancelado)
                .Include(m => m.Provincia)
                    .ThenInclude(p => p.Translations)
                .Include(m => m.Translations)
                .OrderBy(m => m.Descripcion)
                .ToListAsync();

        public async Task<Municipio?> GetByIdAsync(int id)
            => await _context.Municipio
                .Include(m => m.Provincia)
                    .ThenInclude(p => p.Translations)
                .Include(m => m.Provincia)
                    .ThenInclude(p => p.Pais)
                .Include(m => m.Translations)
                .FirstOrDefaultAsync(m => m.Id == id && !m.Cancelado);

        public async Task<IEnumerable<Municipio>> BuscarAsync(string termino, int? paisId = null)
        {
            var query = _context.Municipio
                .Where(m => !m.Cancelado && m.Descripcion.Contains(termino));

            if (paisId.HasValue)
                query = query.Where(m => m.Provincia.PaisId == paisId.Value);

            return await query
                .Include(m => m.Provincia)
                    .ThenInclude(p => p.Translations)
                .Include(m => m.Translations)
                .Take(20)
                .ToListAsync();
        }
    }
}
using GoldBusiness.Domain.Entities;
using GoldBusiness.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace GoldBusiness.Infrastructure.Repositories
{
    public class EstablecimientoRepository : IEstablecimientoRepository
    {
        private readonly ApplicationDbContext _context;

        public EstablecimientoRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Establecimiento>> GetAllAsync()
            => await _context.Establecimiento
                .Where(e => !e.Cancelado)
                .Include(e => e.Translations)
                .Include(e => e.Negocio)
                    .ThenInclude(n => n.Translations)
                .ToListAsync();

        public async Task<Establecimiento?> GetByIdAsync(int id)
            => await _context.Establecimiento
                .Include(e => e.Translations)
                .Include(e => e.Negocio)
                    .ThenInclude(n => n.Translations)
                .FirstOrDefaultAsync(e => e.Id == id);

        public async Task<Establecimiento?> GetByCodigoAsync(string codigo, bool includeCanceled = false)
        {
            var query = _context.Establecimiento
                .Include(e => e.Translations)
                .Include(e => e.Negocio)
                    .ThenInclude(n => n.Translations)
                .Where(e => e.Codigo == codigo);

            if (!includeCanceled)
                query = query.Where(e => !e.Cancelado);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<bool> ExistsByCodigoAsync(string codigo, int? excludeId = null, bool onlyActive = true)
        {
            var query = _context.Establecimiento.Where(e => e.Codigo == codigo);

            if (onlyActive)
                query = query.Where(e => !e.Cancelado);

            if (excludeId.HasValue)
                query = query.Where(e => e.Id != excludeId.Value);

            return await query.AnyAsync();
        }

        public async Task<Establecimiento> AddAsync(Establecimiento entity)
        {
            await _context.Establecimiento.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task UpdateAsync(Establecimiento entity)
        {
            _context.Establecimiento.Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}
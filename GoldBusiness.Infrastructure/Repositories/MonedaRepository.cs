using GoldBusiness.Domain.Entities;
using GoldBusiness.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace GoldBusiness.Infrastructure.Repositories
{
    public class MonedaRepository : IMonedaRepository
    {
        private readonly ApplicationDbContext _context;

        public MonedaRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Moneda>> GetAllAsync()
            => await _context.Moneda
                .Where(g => !g.Cancelado)
                .Include(g => g.Translations)
                .ToListAsync();

        public async Task<Moneda?> GetByIdAsync(int id)
            => await _context.Moneda
                .Include(g => g.Translations)
                .FirstOrDefaultAsync(g => g.Id == id);

        public async Task<Moneda?> GetByCodigoAsync(string codigo, bool includeCanceled = false)
        {
            var query = _context.Moneda
                .Include(g => g.Translations)
                .Where(g => g.Codigo == codigo);

            if (!includeCanceled)
                query = query.Where(g => !g.Cancelado);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<bool> ExistsByCodigoAsync(string codigo, int? excludeId = null, bool onlyActive = true)
        {
            var query = _context.Moneda.Where(g => g.Codigo == codigo);

            if (onlyActive)
                query = query.Where(g => !g.Cancelado);

            if (excludeId.HasValue)
                query = query.Where(g => g.Id != excludeId.Value);

            return await query.AnyAsync();
        }

        public async Task AddAsync(Moneda entity)
        {
            _context.Moneda.Add(entity);
            await _context.SaveChangesAsync();

            // Cargar traducciones (y sus traducciones) para la entidad en memoria
            await _context.Entry(entity)
                .Collection(e => e.Translations)
                .LoadAsync();
        }

        public async Task UpdateAsync(Moneda entity)
        {
            _context.Moneda.Update(entity);
            await _context.SaveChangesAsync();

            // Asegurar que las traducciones quedan cargadas
            await _context.Entry(entity)
                .Collection(e => e.Translations)
                .LoadAsync();
        }
    }
}
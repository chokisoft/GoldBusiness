using GoldBusiness.Domain.Entities;
using GoldBusiness.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace GoldBusiness.Infrastructure.Repositories
{
    public class TransaccionRepository : ITransaccionRepository
    {
        private readonly ApplicationDbContext _context;

        public TransaccionRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Transaccion>> GetAllAsync()
            => await _context.Transaccion
                .Include(t => t.Translations)
                .ToListAsync();

        public async Task<Transaccion?> GetByIdAsync(int id)
            => await _context.Transaccion
                .Include(t => t.Translations)
                .FirstOrDefaultAsync(t => t.Id == id);

        public async Task<Transaccion?> GetByCodigoAsync(string codigo, bool includeCanceled = false)
        {
            // Transaccion no tiene propiedad Cancelado, así que ignoramos el parámetro
            var query = _context.Transaccion
                .Include(t => t.Translations)
                .Where(t => t.Codigo == codigo);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<bool> ExistsByCodigoAsync(string codigo, int? excludeId = null, bool onlyActive = true)
        {
            // Transaccion no tiene propiedad Cancelado, así que ignoramos onlyActive
            var query = _context.Transaccion.Where(t => t.Codigo == codigo);

            if (excludeId.HasValue)
                query = query.Where(t => t.Id != excludeId.Value);

            return await query.AnyAsync();
        }

        public async Task<Transaccion> AddAsync(Transaccion entity)
        {
            await _context.Transaccion.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task UpdateAsync(Transaccion entity)
        {
            _context.Transaccion.Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}
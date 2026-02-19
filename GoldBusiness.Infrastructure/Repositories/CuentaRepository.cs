using GoldBusiness.Domain.Entities;
using GoldBusiness.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace GoldBusiness.Infrastructure.Repositories
{
    public class CuentaRepository(ApplicationDbContext context) : ICuentaRepository
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<IEnumerable<Cuenta>> GetAllAsync()
            => await _context.Cuenta
                .Where(c => !c.Cancelado)
                .Include(c => c.Translations)
                .Include(c => c.SubGrupoCuenta)
                    .ThenInclude(s => s.Translations)
                .Include(c => c.SubGrupoCuenta)
                    .ThenInclude(s => s.GrupoCuenta)
                        .ThenInclude(g => g.Translations)
                .OrderBy(c => c.Codigo)
                .ToListAsync();

        public async Task<Cuenta?> GetByIdAsync(int id)
            => await _context.Cuenta
                .Include(c => c.Translations)
                .Include(c => c.SubGrupoCuenta!) // ✅ AGREGAR !
                    .ThenInclude(s => s!.Translations) // ✅ AGREGAR !
                .Include(c => c.SubGrupoCuenta!)
                    .ThenInclude(s => s!.GrupoCuenta!) // ✅ AGREGAR !
                        .ThenInclude(g => g!.Translations) // ✅ AGREGAR !
                .FirstOrDefaultAsync(c => c.Id == id && !c.Cancelado);

        public async Task<Cuenta?> GetByCodigoAsync(string codigo, bool includeCanceled = false)
        {
            var query = _context.Cuenta
                .Include(c => c.Translations)
                .Include(c => c.SubGrupoCuenta)
                    .ThenInclude(s => s.Translations)
                .Where(c => c.Codigo == codigo);

            if (!includeCanceled)
                query = query.Where(c => !c.Cancelado);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<bool> ExistsByCodigoAsync(string codigo, int? excludeId = null, bool onlyActive = true)
        {
            var query = _context.Cuenta.Where(c => c.Codigo == codigo);

            if (onlyActive)
                query = query.Where(c => !c.Cancelado);

            if (excludeId.HasValue)
                query = query.Where(c => c.Id != excludeId.Value);

            return await query.AnyAsync();
        }

        public async Task AddAsync(Cuenta entity)
        {
            _context.Cuenta.Add(entity);
            await _context.SaveChangesAsync();

            await _context.Entry(entity)
                .Reference(e => e.SubGrupoCuenta)
                .Query()
                .Include(s => s.Translations)
                .Include(s => s.GrupoCuenta)
                    .ThenInclude(g => g.Translations)
                .LoadAsync();

            await _context.Entry(entity)
                .Collection(e => e.Translations)
                .LoadAsync();
        }

        public async Task UpdateAsync(Cuenta entity)
        {
            _context.Cuenta.Update(entity);
            await _context.SaveChangesAsync();

            await _context.Entry(entity)
                .Reference(e => e.SubGrupoCuenta)
                .Query()
                .Include(s => s.Translations)
                .Include(s => s.GrupoCuenta)
                    .ThenInclude(g => g.Translations)
                .LoadAsync();

            await _context.Entry(entity)
                .Collection(e => e.Translations)
                .LoadAsync();
        }
    }
}
using GoldBusiness.Domain.Entities;
using GoldBusiness.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace GoldBusiness.Infrastructure.Repositories
{
    public class SubGrupoCuentaRepository(ApplicationDbContext context) : ISubGrupoCuentaRepository
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<IEnumerable<SubGrupoCuenta>> GetAllAsync()
            => await _context.SubGrupoCuenta
                .Where(s => !s.Cancelado)
                .Include(s => s.Translations)
                .Include(s => s.GrupoCuenta)
                    .ThenInclude(g => g.Translations)
                .OrderBy(s => s.Codigo)
                .ToListAsync();

        public async Task<SubGrupoCuenta?> GetByIdAsync(int id)
            => await _context.SubGrupoCuenta
                .Include(s => s.Translations)
                .Include(s => s.GrupoCuenta) // ✅ DEBE EXISTIR
                    .ThenInclude(g => g.Translations)
                .FirstOrDefaultAsync(s => s.Id == id);

        public async Task<SubGrupoCuenta?> GetByCodigoAsync(string codigo, bool includeCanceled = false)
        {
            var query = _context.SubGrupoCuenta
                .Include(s => s.Translations)
                .Include(s => s.GrupoCuenta)  // ✅ CAMBIO: Cuenta → GrupoCuenta
                    .ThenInclude(g => g.Translations)
                .Where(s => s.Codigo == codigo);

            if (!includeCanceled)
                query = query.Where(s => !s.Cancelado);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<bool> ExistsByCodigoAsync(string codigo, int? excludeId = null, bool onlyActive = true)
        {
            var query = _context.SubGrupoCuenta.Where(g => g.Codigo == codigo);

            if (onlyActive)
                query = query.Where(g => !g.Cancelado);

            if (excludeId.HasValue)
                query = query.Where(g => g.Id != excludeId.Value);

            return await query.AnyAsync();
        }

        public async Task AddAsync(SubGrupoCuenta entity)
        {
            _context.SubGrupoCuenta.Add(entity);
            await _context.SaveChangesAsync();

            // Cargar navegación a GrupoCuenta y traducciones
            await _context.Entry(entity)
                .Reference(e => e.GrupoCuenta)
                .Query()
                .Include(g => g.Translations)
                .LoadAsync();

            await _context.Entry(entity)
                .Collection(e => e.Translations)
                .LoadAsync();
        }

        public async Task UpdateAsync(SubGrupoCuenta entity)
        {
            _context.SubGrupoCuenta.Update(entity);
            await _context.SaveChangesAsync();

            await _context.Entry(entity)
                .Reference(e => e.GrupoCuenta)
                .Query()
                .Include(g => g.Translations)
                .LoadAsync();

            await _context.Entry(entity)
                .Collection(e => e.Translations)
                .LoadAsync();
        }
    }
}
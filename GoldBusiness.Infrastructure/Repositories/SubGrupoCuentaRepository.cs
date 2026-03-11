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

        public async Task<(IEnumerable<SubGrupoCuenta> Items, int Total)> GetPagedAsync(int page, int pageSize, string? termino = null, int? grupoCuentaId = null)
        {
            var query = _context.SubGrupoCuenta
                .AsNoTracking()
                .Where(s => !s.Cancelado);

            // Búsqueda en Código, Descripción y GrupoCuenta
            if (!string.IsNullOrWhiteSpace(termino))
            {
                var lowerTerm = termino.ToLower();
                query = query.Where(s =>
                    s.Codigo.ToLower().Contains(lowerTerm) ||
                    s.Descripcion.ToLower().Contains(lowerTerm) ||
                    s.GrupoCuenta!.Descripcion.ToLower().Contains(lowerTerm)
                );
            }

            // Filtro por GrupoCuenta (opcional)
            if (grupoCuentaId.HasValue)
                query = query.Where(s => s.GrupoCuentaId == grupoCuentaId.Value);

            var total = await query.CountAsync();

            var items = await query
                .Include(s => s.Translations)
                .Include(s => s.GrupoCuenta)
                    .ThenInclude(g => g!.Translations)
                .OrderBy(s => s.Codigo)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, total);
        }

        public async Task<SubGrupoCuenta?> GetByIdAsync(int id)
            => await _context.SubGrupoCuenta
                .Include(s => s.Translations)
                .Include(s => s.GrupoCuenta)
                    .ThenInclude(g => g.Translations)
                .FirstOrDefaultAsync(s => s.Id == id);

        public async Task<SubGrupoCuenta?> GetByCodigoAsync(string codigo, bool includeCanceled = false)
        {
            var query = _context.SubGrupoCuenta
                .Include(s => s.Translations)
                .Include(s => s.GrupoCuenta)
                    .ThenInclude(g => g.Translations)
                .Where(s => s.Codigo == codigo);

            if (!includeCanceled)
                query = query.Where(s => !s.Cancelado);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<bool> ExistsByCodigoAsync(string codigo, int? excludeId = null, bool onlyActive = true)
        {
            var query = _context.SubGrupoCuenta.Where(s => s.Codigo == codigo);

            if (onlyActive)
                query = query.Where(s => !s.Cancelado);

            if (excludeId.HasValue)
                query = query.Where(s => s.Id != excludeId.Value);

            return await query.AnyAsync();
        }

        public async Task AddAsync(SubGrupoCuenta entity)
        {
            _context.SubGrupoCuenta.Add(entity);
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
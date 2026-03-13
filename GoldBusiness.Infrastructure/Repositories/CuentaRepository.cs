using GoldBusiness.Domain.Entities;
using GoldBusiness.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace GoldBusiness.Infrastructure.Repositories
{
    public class CuentaRepository(ApplicationDbContext context) : ICuentaRepository
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<IEnumerable<Cuenta>> GetAllAsync(IReadOnlyCollection<string>? accessLevels = null)
            => await ApplyAccessLevelFilter(_context.Cuenta.Where(c => !c.Cancelado), accessLevels)
                .Include(c => c.Translations)
                .Include(c => c.SubGrupoCuenta)
                    .ThenInclude(s => s.Translations)
                .Include(c => c.SubGrupoCuenta!.GrupoCuenta)
                    .ThenInclude(g => g.Translations)
                .OrderBy(c => c.Codigo)
                .ToListAsync();

        // ✅ AGREGADO: Paginación del servidor con búsqueda en cascada
        public async Task<(IEnumerable<Cuenta> Items, int Total)> GetPagedAsync(int page, int pageSize, string? termino = null, int? subGrupoCuentaId = null, IReadOnlyCollection<string>? accessLevels = null)
        {
            var query = ApplyAccessLevelFilter(_context.Cuenta
                .AsNoTracking()
                .Where(c => !c.Cancelado), accessLevels);

            // Búsqueda en Código, Descripción, SubGrupoCuenta y GrupoCuenta
            if (!string.IsNullOrWhiteSpace(termino))
            {
                var lowerTerm = termino.ToLower();
                query = query.Where(c =>
                    c.Codigo.ToLower().Contains(lowerTerm) ||
                    c.Descripcion.ToLower().Contains(lowerTerm) ||
                    c.SubGrupoCuenta!.Descripcion.ToLower().Contains(lowerTerm) ||
                    c.SubGrupoCuenta!.GrupoCuenta!.Descripcion.ToLower().Contains(lowerTerm)
                );
            }

            // Filtro por SubGrupoCuenta (opcional)
            if (subGrupoCuentaId.HasValue)
                query = query.Where(c => c.SubGrupoCuentaId == subGrupoCuentaId.Value);

            var total = await query.CountAsync();

            var items = await query
                .Include(c => c.Translations)
                .Include(c => c.SubGrupoCuenta)
                    .ThenInclude(s => s!.Translations)
                .Include(c => c.SubGrupoCuenta!.GrupoCuenta)
                    .ThenInclude(g => g!.Translations)
                .OrderBy(c => c.Codigo)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, total);
        }

        public async Task<Cuenta?> GetByIdAsync(int id, IReadOnlyCollection<string>? accessLevels = null)
            => await ApplyAccessLevelFilter(_context.Cuenta, accessLevels)
                .Include(c => c.Translations)
                .Include(c => c.SubGrupoCuenta)
                    .ThenInclude(s => s.Translations)
                .Include(c => c.SubGrupoCuenta!.GrupoCuenta)
                    .ThenInclude(g => g.Translations)
                .FirstOrDefaultAsync(c => c.Id == id);

        public async Task<Cuenta?> GetByCodigoAsync(string codigo, bool includeCanceled = false)
        {
            var query = _context.Cuenta
                .Include(c => c.Translations)
                .Include(c => c.SubGrupoCuenta)
                    .ThenInclude(s => s.Translations)
                .Include(c => c.SubGrupoCuenta!.GrupoCuenta)
                    .ThenInclude(g => g.Translations)
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

        private static IQueryable<Cuenta> ApplyAccessLevelFilter(IQueryable<Cuenta> query, IReadOnlyCollection<string>? accessLevels)
        {
            if (accessLevels == null || accessLevels.Count == 0)
            {
                return query.Where(_ => false);
            }

            var normalized = accessLevels
                .Select(level => level?.Trim().ToUpperInvariant())
                .Where(level => !string.IsNullOrWhiteSpace(level))
                .Distinct()
                .ToList();

            if (normalized.Count == 0)
            {
                return query.Where(_ => false);
            }

            if (normalized.Contains("*"))
            {
                return query;
            }

            IQueryable<Cuenta> filteredQuery = query.Where(_ => false);

            foreach (var prefix in normalized)
            {
                var safePrefix = prefix!;
                filteredQuery = filteredQuery.Concat(query.Where(c => c.Codigo.ToUpper().StartsWith(safePrefix)));
            }

            return filteredQuery.Distinct();
        }
    }
}
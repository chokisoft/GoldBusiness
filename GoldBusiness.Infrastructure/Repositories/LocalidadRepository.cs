using GoldBusiness.Domain.Entities;
using GoldBusiness.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace GoldBusiness.Infrastructure.Repositories
{
    public class LocalidadRepository : ILocalidadRepository
    {
        private readonly ApplicationDbContext _context;

        public LocalidadRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Localidad>> GetAllAsync()
            => await _context.Localidad
                .Where(l => !l.Cancelado)
                .Include(l => l.Translations)
                .Include(l => l.Establecimiento)
                    .ThenInclude(e => e.Translations)
                .Include(l => l.CuentaInventario)
                    .ThenInclude(c => c.Translations)
                .Include(l => l.CuentaCosto)
                    .ThenInclude(c => c.Translations)
                .Include(l => l.CuentaVenta)
                    .ThenInclude(c => c.Translations)
                .Include(l => l.CuentaDevolucion)
                    .ThenInclude(c => c.Translations)
                .ToListAsync();

        public async Task<(IEnumerable<Localidad> Items, int Total)> GetPagedAsync(int page, int pageSize, string? termino = null, int? establecimientoId = null)
        {
            var query = _context.Localidad
                .AsNoTracking()
                .Where(e => !e.Cancelado);

            // Búsqueda en Código, Descripción y Negocio
            if (!string.IsNullOrWhiteSpace(termino))
            {
                var lowerTerm = termino.ToLower();
                query = query.Where(e =>
                    e.Codigo.ToLower().Contains(lowerTerm) ||
                    e.Descripcion.ToLower().Contains(lowerTerm) ||
                    e.Establecimiento!.Descripcion.ToLower().Contains(lowerTerm)
                );
            }

            // Filtro por Negocio (opcional)
            if (establecimientoId.HasValue)
                query = query.Where(e => e.EstablecimientoId == establecimientoId.Value);

            var total = await query.CountAsync();

            var items = await query
                .Include(e => e.Translations)
                .Include(e => e.Establecimiento)
                    .ThenInclude(n => n!.Translations)
                .OrderBy(e => e.Codigo)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, total);
        }

        public async Task<IEnumerable<Localidad>> GetByEstablecimientoIdAsync(int establecimientoId)
            => await _context.Localidad
                .Where(l => l.EstablecimientoId == establecimientoId && !l.Cancelado)
                .Include(l => l.Translations)
                .Include(l => l.Establecimiento)
                    .ThenInclude(e => e.Translations)
                .Include(l => l.CuentaInventario)
                    .ThenInclude(c => c.Translations)
                .Include(l => l.CuentaCosto)
                    .ThenInclude(c => c.Translations)
                .Include(l => l.CuentaVenta)
                    .ThenInclude(c => c.Translations)
                .Include(l => l.CuentaDevolucion)
                    .ThenInclude(c => c.Translations)
                .ToListAsync();

        public async Task<Localidad?> GetByIdAsync(int id)
            => await _context.Localidad
                .Include(l => l.Translations)
                .Include(l => l.Establecimiento)
                    .ThenInclude(e => e.Translations)
                .Include(l => l.CuentaInventario)
                    .ThenInclude(c => c.Translations)
                .Include(l => l.CuentaCosto)
                    .ThenInclude(c => c.Translations)
                .Include(l => l.CuentaVenta)
                    .ThenInclude(c => c.Translations)
                .Include(l => l.CuentaDevolucion)
                    .ThenInclude(c => c.Translations)
                .FirstOrDefaultAsync(l => l.Id == id);

        public async Task<Localidad?> GetByCodigoAsync(string codigo, bool includeCanceled = false)
        {
            var query = _context.Localidad
                .Include(l => l.Translations)
                .Include(l => l.Establecimiento)
                    .ThenInclude(e => e.Translations)
                .Include(l => l.CuentaInventario)
                    .ThenInclude(c => c.Translations)
                .Include(l => l.CuentaCosto)
                    .ThenInclude(c => c.Translations)
                .Include(l => l.CuentaVenta)
                    .ThenInclude(c => c.Translations)
                .Include(l => l.CuentaDevolucion)
                    .ThenInclude(c => c.Translations)
                .Where(l => l.Codigo == codigo);

            if (!includeCanceled)
                query = query.Where(l => !l.Cancelado);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<bool> ExistsByCodigoAsync(string codigo, int? excludeId = null, bool onlyActive = true)
        {
            var query = _context.Localidad.Where(l => l.Codigo == codigo);

            if (onlyActive)
                query = query.Where(l => !l.Cancelado);

            if (excludeId.HasValue)
                query = query.Where(l => l.Id != excludeId.Value);

            return await query.AnyAsync();
        }

        public async Task<Localidad> AddAsync(Localidad entity)
        {
            await _context.Localidad.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task UpdateAsync(Localidad entity)
        {
            _context.Localidad.Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}
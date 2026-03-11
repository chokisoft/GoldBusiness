using GoldBusiness.Domain.Entities;
using GoldBusiness.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace GoldBusiness.Infrastructure.Repositories
{
    public class LineaRepository(ApplicationDbContext context) : ILineaRepository
    {
        private readonly ApplicationDbContext _context = context;

        public async Task<IEnumerable<Linea>> GetAllAsync()
            => await _context.Linea
                .Where(l => !l.Cancelado)
                .Include(l => l.Translations)
                .OrderBy(l => l.Codigo)
                .ToListAsync();

        // ✅ AGREGADO: Paginación del servidor
        public async Task<(IEnumerable<Linea> Items, int Total)> GetPagedAsync(int page, int pageSize, string? termino = null)
        {
            var query = _context.Linea
                .AsNoTracking()
                .Where(l => !l.Cancelado);

            // Búsqueda en Código y Descripción
            if (!string.IsNullOrWhiteSpace(termino))
            {
                var lowerTerm = termino.ToLower();
                query = query.Where(l =>
                    l.Codigo.ToLower().Contains(lowerTerm) ||
                    l.Descripcion.ToLower().Contains(lowerTerm)
                );
            }

            var total = await query.CountAsync();

            var items = await query
                .Include(l => l.Translations)
                .OrderBy(l => l.Codigo)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, total);
        }

        public async Task<Linea?> GetByIdAsync(int id)
            => await _context.Linea
                .Include(l => l.Translations)
                .FirstOrDefaultAsync(l => l.Id == id);

        public async Task<Linea?> GetByCodigoAsync(string codigo, bool includeCanceled = false)
        {
            var query = _context.Linea
                .Include(l => l.Translations)
                .Where(l => l.Codigo == codigo);

            if (!includeCanceled)
                query = query.Where(l => !l.Cancelado);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<bool> ExistsByCodigoAsync(string codigo, int? excludeId = null, bool onlyActive = true)
        {
            var query = _context.Linea.Where(l => l.Codigo == codigo);

            if (onlyActive)
                query = query.Where(l => !l.Cancelado);

            if (excludeId.HasValue)
                query = query.Where(l => l.Id != excludeId.Value);

            return await query.AnyAsync();
        }

        public async Task AddAsync(Linea entity)
        {
            _context.Linea.Add(entity);
            await _context.SaveChangesAsync();

            await _context.Entry(entity)
                .Collection(e => e.Translations)
                .LoadAsync();
        }

        public async Task UpdateAsync(Linea entity)
        {
            _context.Linea.Update(entity);
            await _context.SaveChangesAsync();

            await _context.Entry(entity)
                .Collection(e => e.Translations)
                .LoadAsync();
        }
    }
}
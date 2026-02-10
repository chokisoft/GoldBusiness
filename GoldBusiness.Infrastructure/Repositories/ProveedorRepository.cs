using GoldBusiness.Domain.Entities;
using GoldBusiness.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace GoldBusiness.Infrastructure.Repositories
{
    public class ProveedorRepository : IProveedorRepository
    {
        private readonly ApplicationDbContext _context;

        public ProveedorRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Proveedor>> GetAllAsync()
            => await _context.Proveedor
                .Where(g => !g.Cancelado)
                .Include(g => g.Translations)
                .Include(g => g.CuentasCobrarPagar)
                .ToListAsync();

        public async Task<Proveedor?> GetByIdAsync(int id)
            => await _context.Proveedor
                .Include(g => g.Translations)
                .Include(g => g.CuentasCobrarPagar)
                .FirstOrDefaultAsync(g => g.Id == id);

        public async Task<Proveedor?> GetByCodigoAsync(string codigo, bool includeCanceled = false)
        {
            var query = _context.Proveedor
                .Include(g => g.Translations)
                .Include(g => g.CuentasCobrarPagar)
                .Where(g => g.Codigo == codigo);

            if (!includeCanceled)
                query = query.Where(g => !g.Cancelado);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<bool> ExistsByCodigoAsync(string codigo, int? excludeId = null, bool onlyActive = true)
        {
            var query = _context.Proveedor.Where(g => g.Codigo == codigo);

            if (onlyActive)
                query = query.Where(g => !g.Cancelado);

            if (excludeId.HasValue)
                query = query.Where(g => g.Id != excludeId.Value);

            return await query.AnyAsync();
        }

        public async Task AddAsync(Proveedor entity)
        {
            _context.Proveedor.Add(entity);
            await _context.SaveChangesAsync();

            // Cargar traducciones y sublineas (y sus traducciones) para la entidad en memoria
            await _context.Entry(entity)
                .Collection(e => e.Translations)
                .LoadAsync();
        }

        public async Task UpdateAsync(Proveedor entity)
        {
            _context.Proveedor.Update(entity);
            await _context.SaveChangesAsync();

            // Asegurar que las traducciones y sublineas quedan cargadas
            await _context.Entry(entity)
                .Collection(e => e.Translations)
                .LoadAsync();
        }
    }
}
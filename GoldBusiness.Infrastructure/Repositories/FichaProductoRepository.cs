using GoldBusiness.Domain.Entities;
using GoldBusiness.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace GoldBusiness.Infrastructure.Repositories
{
    public class FichaProductoRepository : IFichaProductoRepository
    {
        private readonly ApplicationDbContext _context;

        public FichaProductoRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<FichaProducto>> GetAllAsync()
            => await _context.FichaProducto
                .Where(f => !f.Cancelado)
                .Include(f => f.Producto)
                    .ThenInclude(p => p.Translations)
                .Include(f => f.Componente)
                    .ThenInclude(c => c.Translations)
                .Include(f => f.Localidad)
                    .ThenInclude(l => l.Translations)
                .ToListAsync();

        public async Task<IEnumerable<FichaProducto>> GetByProductoIdAsync(int productoId)
            => await _context.FichaProducto
                .Where(f => f.ProductoId == productoId && !f.Cancelado)
                .Include(f => f.Producto)
                    .ThenInclude(p => p.Translations)
                .Include(f => f.Componente)
                    .ThenInclude(c => c.Translations)
                .Include(f => f.Localidad)
                    .ThenInclude(l => l.Translations)
                .ToListAsync();

        public async Task<IEnumerable<FichaProducto>> GetByLocalidadIdAsync(int localidadId)
            => await _context.FichaProducto
                .Where(f => f.LocalidadId == localidadId && !f.Cancelado)
                .Include(f => f.Producto)
                    .ThenInclude(p => p.Translations)
                .Include(f => f.Componente)
                    .ThenInclude(c => c.Translations)
                .Include(f => f.Localidad)
                    .ThenInclude(l => l.Translations)
                .ToListAsync();

        public async Task<FichaProducto?> GetByIdAsync(int id)
            => await _context.FichaProducto
                .Include(f => f.Producto)
                    .ThenInclude(p => p.Translations)
                .Include(f => f.Componente)
                    .ThenInclude(c => c.Translations)
                .Include(f => f.Localidad)
                    .ThenInclude(l => l.Translations)
                .FirstOrDefaultAsync(f => f.Id == id);

        public async Task<FichaProducto?> GetByComposicionAsync(int productoId, int componenteId, int localidadId)
            => await _context.FichaProducto
                .Include(f => f.Producto)
                    .ThenInclude(p => p.Translations)
                .Include(f => f.Componente)
                    .ThenInclude(c => c.Translations)
                .Include(f => f.Localidad)
                    .ThenInclude(l => l.Translations)
                .FirstOrDefaultAsync(f => f.ProductoId == productoId
                    && f.ComponenteId == componenteId
                    && f.LocalidadId == localidadId);

        public async Task<bool> ExistsComposicionAsync(int productoId, int componenteId, int localidadId, int? excludeId = null)
        {
            var query = _context.FichaProducto
                .Where(f => f.ProductoId == productoId
                    && f.ComponenteId == componenteId
                    && f.LocalidadId == localidadId
                    && !f.Cancelado);

            if (excludeId.HasValue)
                query = query.Where(f => f.Id != excludeId.Value);

            return await query.AnyAsync();
        }

        public async Task<FichaProducto> AddAsync(FichaProducto entity)
        {
            await _context.FichaProducto.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task UpdateAsync(FichaProducto entity)
        {
            _context.FichaProducto.Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}
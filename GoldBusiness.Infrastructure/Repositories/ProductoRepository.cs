using GoldBusiness.Domain.Entities;
using GoldBusiness.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace GoldBusiness.Infrastructure.Repositories
{
    public class ProductoRepository : IProductoRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductoRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Producto>> GetAllAsync()
            => await _context.Producto
                .Where(p => !p.Cancelado)
                .Include(p => p.Translations)
                .Include(p => p.Establecimiento)
                    .ThenInclude(e => e.Translations)
                .Include(p => p.Proveedor)
                    .ThenInclude(pr => pr.Translations)
                .Include(p => p.SubLinea)
                    .ThenInclude(sl => sl.Translations)
                .Include(p => p.SubLinea.Linea)
                    .ThenInclude(l => l.Translations)
                .Include(p => p.UnidadMedida)
                    .ThenInclude(um => um.Translations)
                .ToListAsync();

        public async Task<IEnumerable<Producto>> GetByEstablecimientoIdAsync(int establecimientoId)
            => await _context.Producto
                .Where(p => p.EstablecimientoId == establecimientoId && !p.Cancelado)
                .Include(p => p.Translations)
                .Include(p => p.Establecimiento)
                    .ThenInclude(e => e.Translations)
                .Include(p => p.Proveedor)
                    .ThenInclude(pr => pr.Translations)
                .Include(p => p.SubLinea)
                    .ThenInclude(sl => sl.Translations)
                .Include(p => p.SubLinea.Linea)
                    .ThenInclude(l => l.Translations)
                .Include(p => p.UnidadMedida)
                    .ThenInclude(um => um.Translations)
                .ToListAsync();

        public async Task<IEnumerable<Producto>> GetByProveedorIdAsync(int proveedorId)
            => await _context.Producto
                .Where(p => p.ProveedorId == proveedorId && !p.Cancelado)
                .Include(p => p.Translations)
                .Include(p => p.Establecimiento)
                    .ThenInclude(e => e.Translations)
                .Include(p => p.Proveedor)
                    .ThenInclude(pr => pr.Translations)
                .Include(p => p.SubLinea)
                    .ThenInclude(sl => sl.Translations)
                .Include(p => p.UnidadMedida)
                    .ThenInclude(um => um.Translations)
                .ToListAsync();

        public async Task<IEnumerable<Producto>> GetBySubLineaIdAsync(int subLineaId)
            => await _context.Producto
                .Where(p => p.SubLineaId == subLineaId && !p.Cancelado)
                .Include(p => p.Translations)
                .Include(p => p.Establecimiento)
                    .ThenInclude(e => e.Translations)
                .Include(p => p.Proveedor)
                    .ThenInclude(pr => pr.Translations)
                .Include(p => p.SubLinea)
                    .ThenInclude(sl => sl.Translations)
                .Include(p => p.UnidadMedida)
                    .ThenInclude(um => um.Translations)
                .ToListAsync();

        public async Task<Producto?> GetByIdAsync(int id)
            => await _context.Producto
                .Include(p => p.Translations)
                .Include(p => p.Establecimiento)
                    .ThenInclude(e => e.Translations)
                .Include(p => p.Proveedor)
                    .ThenInclude(pr => pr.Translations)
                .Include(p => p.SubLinea)
                    .ThenInclude(sl => sl.Translations)
                .Include(p => p.SubLinea.Linea)
                    .ThenInclude(l => l.Translations)
                .Include(p => p.UnidadMedida)
                    .ThenInclude(um => um.Translations)
                .FirstOrDefaultAsync(p => p.Id == id);

        public async Task<Producto?> GetByCodigoAsync(string codigo, bool includeCanceled = false)
        {
            var query = _context.Producto
                .Include(p => p.Translations)
                .Include(p => p.Establecimiento)
                    .ThenInclude(e => e.Translations)
                .Include(p => p.Proveedor)
                    .ThenInclude(pr => pr.Translations)
                .Include(p => p.SubLinea)
                    .ThenInclude(sl => sl.Translations)
                .Include(p => p.UnidadMedida)
                    .ThenInclude(um => um.Translations)
                .Where(p => p.Codigo == codigo);

            if (!includeCanceled)
                query = query.Where(p => !p.Cancelado);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<bool> ExistsByCodigoAsync(string codigo, int? excludeId = null, bool onlyActive = true)
        {
            var query = _context.Producto.Where(p => p.Codigo == codigo);

            if (onlyActive)
                query = query.Where(p => !p.Cancelado);

            if (excludeId.HasValue)
                query = query.Where(p => p.Id != excludeId.Value);

            return await query.AnyAsync();
        }

        public async Task<Producto> AddAsync(Producto entity)
        {
            await _context.Producto.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task UpdateAsync(Producto entity)
        {
            _context.Producto.Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}
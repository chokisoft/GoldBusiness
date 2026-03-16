using GoldBusiness.Domain.Entities;
using GoldBusiness.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq;

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
                .Include(p => p.Pais)
                .Include(p => p.Provincia)
                .Include(p => p.Municipio)
                .Include(p => p.CodigoPostal)
                .Where(p => !p.Cancelado)
                .OrderBy(p => p.Codigo)
                .ToListAsync();

        public async Task<Proveedor?> GetByIdAsync(int id)
            => await _context.Proveedor
                .Include(p => p.Pais)
                .Include(p => p.Provincia)
                .Include(p => p.Municipio)
                .Include(p => p.CodigoPostal)
                .FirstOrDefaultAsync(p => p.Id == id);

        public async Task<Proveedor?> GetByCodigoAsync(string codigo, bool includeCanceled = false)
            => await _context.Proveedor
                .Include(p => p.Pais)
                .Include(p => p.Provincia)
                .Include(p => p.Municipio)
                .Include(p => p.CodigoPostal)
                .FirstOrDefaultAsync(p => p.Codigo == codigo && (includeCanceled || !p.Cancelado));

        public async Task<bool> ExistsByCodigoAsync(string codigo, int? excludeId = null, bool includeCanceled = false)
        {
            var query = _context.Proveedor
                .Where(p => p.Codigo == codigo && (includeCanceled || !p.Cancelado));

            if (excludeId.HasValue)
            {
                query = query.Where(p => p.Id != excludeId.Value);
            }

            return await query.AnyAsync();
        }

        public async Task AddAsync(Proveedor entity)
        {
            await _context.Proveedor.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Proveedor entity)
        {
            _context.Proveedor.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                _context.Proveedor.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }

        // Implementación de GetPagedAsync
        public async Task<(IEnumerable<Proveedor> Items, int Total)> GetPagedAsync(int page, int pageSize, string? search = null)
        {
            var query = _context.Proveedor
                .AsNoTracking()
                .Include(p => p.Pais)
                .Include(p => p.Provincia)
                .Include(p => p.Municipio)
                .Include(p => p.CodigoPostal)
                .Where(p => !p.Cancelado);

            if (!string.IsNullOrWhiteSpace(search))
            {
                var lower = search.ToLower();
                query = query.Where(p =>
                    p.Codigo.ToLower().Contains(lower) ||
                    p.Descripcion.ToLower().Contains(lower) ||
                    (p.Nif != null && p.Nif.ToLower().Contains(lower)) ||
                    (p.Email1 != null && p.Email1.ToLower().Contains(lower)) ||
                    (p.Telefono1 != null && p.Telefono1.ToLower().Contains(lower))
                );
            }

            var total = await query.CountAsync();

            var items = await query
                .OrderBy(p => p.Descripcion)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, total);
        }
    }
}
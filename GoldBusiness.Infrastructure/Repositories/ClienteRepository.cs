using GoldBusiness.Domain.Entities;
using GoldBusiness.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace GoldBusiness.Infrastructure.Repositories
{
    public class ClienteRepository : IClienteRepository
    {
        private readonly ApplicationDbContext _context;

        public ClienteRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Cliente>> GetAllAsync()
            => await _context.Cliente
                .Include(c => c.Pais)
                .Include(c => c.Provincia)
                .Include(c => c.Municipio)
                .Include(c => c.CodigoPostal)
                .Include(c => c.Translations)
                .Where(c => !c.Cancelado)
                .OrderBy(c => c.Codigo)
                .ToListAsync();

        public async Task<(IEnumerable<Cliente> Items, int Total)> GetPagedAsync(int page, int pageSize, string? search = null)
        {
            var query = _context.Cliente
                .AsNoTracking()
                .Include(c => c.Pais)
                .Include(c => c.Provincia)
                .Include(c => c.Municipio)
                .Include(c => c.CodigoPostal)
                .Include(c => c.Translations)
                .Where(c => !c.Cancelado);

            if (!string.IsNullOrWhiteSpace(search))
            {
                var lower = search.ToLower();
                query = query.Where(c =>
                    c.Codigo.ToLower().Contains(lower) ||
                    c.Descripcion.ToLower().Contains(lower) ||
                    (c.Nif != null && c.Nif.ToLower().Contains(lower)) ||
                    (c.Email1 != null && c.Email1.ToLower().Contains(lower)) ||
                    (c.Telefono1 != null && c.Telefono1.ToLower().Contains(lower))
                );
            }

            var total = await query.CountAsync();

            var items = await query
                .OrderBy(c => c.Descripcion)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, total);
        }

        public async Task<Cliente?> GetByIdAsync(int id)
            => await _context.Cliente
                .Include(c => c.Pais)
                .Include(c => c.Provincia)
                .Include(c => c.Municipio)
                .Include(c => c.CodigoPostal)
                .Include(c => c.Translations)
                .FirstOrDefaultAsync(c => c.Id == id);

        public async Task<Cliente?> GetByCodigoAsync(string codigo, bool includeCanceled = false)
            => await _context.Cliente
                .Include(c => c.Pais)
                .Include(c => c.Provincia)
                .Include(c => c.Municipio)
                .Include(c => c.CodigoPostal)
                .Include(c => c.Translations)
                .FirstOrDefaultAsync(c => c.Codigo == codigo && (includeCanceled || !c.Cancelado));

        public async Task<bool> ExistsByCodigoAsync(string codigo, int? excludeId = null, bool includeCanceled = false)
        {
            var query = _context.Cliente
                .Where(c => c.Codigo == codigo && (includeCanceled || !c.Cancelado));

            if (excludeId.HasValue)
            {
                query = query.Where(c => c.Id != excludeId.Value);
            }

            return await query.AnyAsync();
        }

        public async Task AddAsync(Cliente entity)
        {
            await _context.Cliente.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Cliente entity)
        {
            _context.Cliente.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                _context.Cliente.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}
using GoldBusiness.Domain.Entities;
using GoldBusiness.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace GoldBusiness.Infrastructure.Repositories
{
    public class PaisRepository : IPaisRepository
    {
        private readonly ApplicationDbContext _context;

        public PaisRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Pais>> GetAllAsync()
            => await _context.Pais
                .Where(p => !p.Cancelado)
                .Include(p => p.Translations)
                .OrderBy(p => p.Codigo)
                .ToListAsync();

        public async Task<(IEnumerable<Pais> Items, int Total)> GetPagedAsync(int page, int pageSize, string? termino = null)
        {
            var query = _context.Pais
                .AsNoTracking()
                .Where(p => !p.Cancelado);

            if (!string.IsNullOrWhiteSpace(termino))
                query = query.Where(p => p.Codigo.Contains(termino) || p.Descripcion.Contains(termino));

            var total = await query.CountAsync();

            var items = await query
                .Include(p => p.Translations)
                .OrderBy(p => p.Codigo)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, total);
        }

        public async Task<Pais?> GetByIdAsync(int id)
            => await _context.Pais
                .Include(p => p.Translations)
                .FirstOrDefaultAsync(p => p.Id == id && !p.Cancelado);

        public async Task<Pais?> GetByCodigoAsync(string codigo)
            => await _context.Pais
                .Include(p => p.Translations)
                .FirstOrDefaultAsync(p => p.Codigo == codigo && !p.Cancelado);

        public async Task<Pais> AddAsync(Pais entity)
        {
            _context.Pais.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task UpdateAsync(Pais entity)
        {
            _context.Pais.Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}
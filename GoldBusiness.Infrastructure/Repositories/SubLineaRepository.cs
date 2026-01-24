using GoldBusiness.Domain.Entities;
using GoldBusiness.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace GoldBusiness.Infrastructure.Repositories
{
    public class SubLineaRepository : ISubLineaRepository
    {
        private readonly ApplicationDbContext _context;

        public SubLineaRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<SubLinea>> GetAllAsync()
            => await _context.SubLinea
                .Where(s => !s.Cancelado)
                .Include(s => s.Translations)
                .Include(s => s.Linea)
                    .ThenInclude(g => g.Translations)
                .ToListAsync();

        public async Task<SubLinea?> GetByIdAsync(int id)
            => await _context.SubLinea
                .Include(s => s.Translations)
                .Include(s => s.Linea)
                    .ThenInclude(g => g.Translations)
                .FirstOrDefaultAsync(s => s.Id == id);

        public async Task AddAsync(SubLinea entity)
        {
            _context.SubLinea.Add(entity);
            await _context.SaveChangesAsync();

            // Cargar navegación a GrupoCuenta y traducciones
            await _context.Entry(entity)
                .Reference(e => e.Linea)
                .Query()
                .Include(g => g.Translations)
                .LoadAsync();

            await _context.Entry(entity)
                .Collection(e => e.Translations)
                .LoadAsync();
        }

        public async Task UpdateAsync(SubLinea entity)
        {
            _context.SubLinea.Update(entity);
            await _context.SaveChangesAsync();

            await _context.Entry(entity)
                .Reference(e => e.Linea)
                .Query()
                .Include(g => g.Translations)
                .LoadAsync();

            await _context.Entry(entity)
                .Collection(e => e.Translations)
                .LoadAsync();
        }
    }
}
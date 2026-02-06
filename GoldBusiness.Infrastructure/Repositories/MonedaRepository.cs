using GoldBusiness.Domain.Entities;
using GoldBusiness.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace GoldBusiness.Infrastructure.Repositories
{
    public class MonedaRepository : IMonedaRepository
    {
        private readonly ApplicationDbContext _context;

        public MonedaRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Moneda>> GetAllAsync()
            => await _context.Moneda
                .Where(g => !g.Cancelado)
                .Include(g => g.Translations)
                .ToListAsync();

        public async Task<Moneda?> GetByIdAsync(int id)
            => await _context.Moneda
                .Include(g => g.Translations)
                .FirstOrDefaultAsync(g => g.Id == id);

        public async Task AddAsync(Moneda entity)
        {
            _context.Moneda.Add(entity);
            await _context.SaveChangesAsync();

            // Cargar traducciones (y sus traducciones) para la entidad en memoria
            await _context.Entry(entity)
                .Collection(e => e.Translations)
                .LoadAsync();
        }

        public async Task UpdateAsync(Moneda entity)
        {
            _context.Moneda.Update(entity);
            await _context.SaveChangesAsync();

            // Asegurar que las traducciones quedan cargadas
            await _context.Entry(entity)
                .Collection(e => e.Translations)
                .LoadAsync();
        }
    }
}
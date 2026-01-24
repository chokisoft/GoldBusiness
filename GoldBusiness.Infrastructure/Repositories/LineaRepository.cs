using GoldBusiness.Domain.Entities;
using GoldBusiness.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace GoldBusiness.Infrastructure.Repositories
{
    public class LineaRepository : ILineaRepository
    {
        private readonly ApplicationDbContext _context;

        public LineaRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Linea>> GetAllAsync()
            => await _context.Linea
                .Where(g => !g.Cancelado)
                .Include(g => g.Translations)
                .Include(g => g.SubLinea)
                    .ThenInclude(s => s.Translations)
                .ToListAsync();

        public async Task<Linea?> GetByIdAsync(int id)
            => await _context.Linea
                .Include(g => g.Translations)
                .Include(g => g.SubLinea)
                    .ThenInclude(s => s.Translations)
                .FirstOrDefaultAsync(g => g.Id == id);

        public async Task AddAsync(Linea entity)
        {
            _context.Linea.Add(entity);
            await _context.SaveChangesAsync();

            // Cargar traducciones y sublineas (y sus traducciones) para la entidad en memoria
            await _context.Entry(entity)
                .Collection(e => e.Translations)
                .LoadAsync();

            await _context.Entry(entity)
                .Collection(e => e.SubLinea)
                .Query()
                .Include(s => s.Translations)
                .LoadAsync();
        }

        public async Task UpdateAsync(Linea entity)
        {
            _context.Linea.Update(entity);
            await _context.SaveChangesAsync();

            // Asegurar que las traducciones y sublineas quedan cargadas
            await _context.Entry(entity)
                .Collection(e => e.Translations)
                .LoadAsync();

            await _context.Entry(entity)
                .Collection(e => e.SubLinea)
                .Query()
                .Include(s => s.Translations)
                .LoadAsync();
        }
    }
}
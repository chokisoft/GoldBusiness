using GoldBusiness.Domain.Entities;
using GoldBusiness.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace GoldBusiness.Infrastructure.Repositories
{
    public class GrupoCuentaRepository : IGrupoCuentaRepository
    {
        private readonly ApplicationDbContext _context;

        public GrupoCuentaRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<GrupoCuenta>> GetAllAsync()
            => await _context.GrupoCuenta
                .Where(g => !g.Cancelado)
                .Include(g => g.Translations)
                .Include(g => g.SubGrupoCuenta)
                    .ThenInclude(s => s.Translations)
                .ToListAsync();

        public async Task<GrupoCuenta?> GetByIdAsync(int id)
            => await _context.GrupoCuenta
                .Include(g => g.Translations)
                .Include(g => g.SubGrupoCuenta)
                    .ThenInclude(s => s.Translations)
                .FirstOrDefaultAsync(g => g.Id == id);

        public async Task AddAsync(GrupoCuenta entity)
        {
            _context.GrupoCuenta.Add(entity);
            await _context.SaveChangesAsync();

            // Cargar traducciones y subgrupos (y sus traducciones) para la entidad en memoria
            await _context.Entry(entity)
                .Collection(e => e.Translations)
                .LoadAsync();

            await _context.Entry(entity)
                .Collection(e => e.SubGrupoCuenta)
                .Query()
                .Include(s => s.Translations)
                .LoadAsync();
        }

        public async Task UpdateAsync(GrupoCuenta entity)
        {
            _context.GrupoCuenta.Update(entity);
            await _context.SaveChangesAsync();

            // Asegurar que las traducciones y subgrupos quedan cargadas
            await _context.Entry(entity)
                .Collection(e => e.Translations)
                .LoadAsync();

            await _context.Entry(entity)
                .Collection(e => e.SubGrupoCuenta)
                .Query()
                .Include(s => s.Translations)
                .LoadAsync();
        }
    }
}
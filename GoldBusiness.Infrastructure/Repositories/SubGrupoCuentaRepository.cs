using GoldBusiness.Domain.Entities;
using GoldBusiness.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace GoldBusiness.Infrastructure.Repositories
{
    public class SubGrupoCuentaRepository : ISubGrupoCuentaRepository
    {
        private readonly ApplicationDbContext _context;

        public SubGrupoCuentaRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<SubGrupoCuenta>> GetAllAsync()
            => await _context.SubGrupoCuenta
                .Where(s => !s.Cancelado)
                .Include(s => s.Translations)
                .Include(s => s.GrupoCuenta)
                    .ThenInclude(g => g.Translations)
                .ToListAsync();

        public async Task<SubGrupoCuenta?> GetByIdAsync(int id)
            => await _context.SubGrupoCuenta
                .Include(s => s.Translations)
                .Include(s => s.GrupoCuenta)
                    .ThenInclude(g => g.Translations)
                .FirstOrDefaultAsync(s => s.Id == id);

        public async Task AddAsync(SubGrupoCuenta entity)
        {
            _context.SubGrupoCuenta.Add(entity);
            await _context.SaveChangesAsync();

            // Cargar navegación a GrupoCuenta y traducciones
            await _context.Entry(entity)
                .Reference(e => e.GrupoCuenta)
                .Query()
                .Include(g => g.Translations)
                .LoadAsync();

            await _context.Entry(entity)
                .Collection(e => e.Translations)
                .LoadAsync();
        }

        public async Task UpdateAsync(SubGrupoCuenta entity)
        {
            _context.SubGrupoCuenta.Update(entity);
            await _context.SaveChangesAsync();

            await _context.Entry(entity)
                .Reference(e => e.GrupoCuenta)
                .Query()
                .Include(g => g.Translations)
                .LoadAsync();

            await _context.Entry(entity)
                .Collection(e => e.Translations)
                .LoadAsync();
        }
    }
}
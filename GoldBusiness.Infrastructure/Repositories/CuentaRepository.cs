using GoldBusiness.Domain.Entities;
using GoldBusiness.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace GoldBusiness.Infrastructure.Repositories
{
    public class CuentaRepository : ICuentaRepository
    {
        private readonly ApplicationDbContext _context;

        public CuentaRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Cuenta>> GetAllAsync()
            => await _context.Cuenta
                .Where(c => !c.Cancelado)
                .Include(c => c.Translations)
                .Include(c => c.SubGrupoCuenta)
                    .ThenInclude(s => s.Translations)
                .Include(c => c.SubGrupoCuenta)
                    .ThenInclude(s => s.GrupoCuenta)
                        .ThenInclude(g => g.Translations)
                .ToListAsync();

        public async Task<Cuenta?> GetByIdAsync(int id)
            => await _context.Cuenta
                .Include(c => c.Translations)
                .Include(c => c.SubGrupoCuenta)
                    .ThenInclude(s => s.Translations)
                .Include(c => c.SubGrupoCuenta)
                    .ThenInclude(s => s.GrupoCuenta)
                        .ThenInclude(g => g.Translations)
                .FirstOrDefaultAsync(c => c.Id == id);

        public async Task AddAsync(Cuenta entity)
        {
            _context.Cuenta.Add(entity);
            await _context.SaveChangesAsync();

            await _context.Entry(entity)
                .Reference(e => e.SubGrupoCuenta)
                .Query()
                .Include(s => s.Translations)
                .Include(s => s.GrupoCuenta)
                    .ThenInclude(g => g.Translations)
                .LoadAsync();
        }

        public async Task UpdateAsync(Cuenta entity)
        {
            _context.Cuenta.Update(entity);
            await _context.SaveChangesAsync();

            await _context.Entry(entity)
                .Reference(e => e.SubGrupoCuenta)
                .Query()
                .Include(s => s.Translations)
                .Include(s => s.GrupoCuenta)
                    .ThenInclude(g => g.Translations)
                .LoadAsync();
        }
    }
}
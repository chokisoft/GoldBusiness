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
                .OrderBy(p => p.Descripcion)
                .ToListAsync();

        public async Task<Pais?> GetByIdAsync(int id)
            => await _context.Pais
                .Include(p => p.Translations)
                .FirstOrDefaultAsync(p => p.Id == id && !p.Cancelado);

        public async Task<Pais?> GetByCodigoAsync(string codigo)
            => await _context.Pais
                .Include(p => p.Translations)
                .FirstOrDefaultAsync(p => p.Codigo == codigo && !p.Cancelado);
    }
}
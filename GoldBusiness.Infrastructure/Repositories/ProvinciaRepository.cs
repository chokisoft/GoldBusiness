using GoldBusiness.Domain.Entities;
using GoldBusiness.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace GoldBusiness.Infrastructure.Repositories
{
    public class ProvinciaRepository : IProvinciaRepository
    {
        private readonly ApplicationDbContext _context;

        public ProvinciaRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Provincia>> GetAllAsync()
            => await _context.Provincia
                .Where(p => !p.Cancelado)
                .Include(p => p.Pais)
                    .ThenInclude(pais => pais.Translations)
                .Include(p => p.Translations)
                .OrderBy(p => p.Descripcion)
                .ToListAsync();

        public async Task<IEnumerable<Provincia>> GetByPaisIdAsync(int paisId)
            => await _context.Provincia
                .Where(p => p.PaisId == paisId && !p.Cancelado)
                .Include(p => p.Pais)
                    .ThenInclude(pais => pais.Translations)
                .Include(p => p.Translations)
                .OrderBy(p => p.Descripcion)
                .ToListAsync();

        public async Task<Provincia?> GetByIdAsync(int id)
            => await _context.Provincia
                .Include(p => p.Pais)
                    .ThenInclude(pais => pais.Translations)
                .Include(p => p.Translations)
                .FirstOrDefaultAsync(p => p.Id == id && !p.Cancelado);
    }
}
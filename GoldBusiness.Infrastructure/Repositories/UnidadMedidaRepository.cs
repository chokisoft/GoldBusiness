using GoldBusiness.Domain.Entities;
using GoldBusiness.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace GoldBusiness.Infrastructure.Repositories
{
    public class UnidadMedidaRepository : IUnidadMedidaRepository
    {
        private readonly ApplicationDbContext _context;

        public UnidadMedidaRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<UnidadMedida>> GetAllAsync()
            => await _context.UnidadMedida
                .Where(um => !um.Cancelado)
                .Include(um => um.Translations)
                .OrderBy(um => um.Codigo)
                .ToListAsync();

        public async Task<UnidadMedida?> GetByIdAsync(int id)
            => await _context.UnidadMedida
                .Include(um => um.Translations)
                .FirstOrDefaultAsync(um => um.Id == id);

        public async Task<UnidadMedida?> GetByCodigoAsync(string codigo, bool includeCanceled = false)
        {
            var query = _context.UnidadMedida
                .Include(um => um.Translations)
                .Where(um => um.Codigo == codigo);

            if (!includeCanceled)
                query = query.Where(um => !um.Cancelado);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<bool> ExistsByCodigoAsync(string codigo, int? excludeId = null, bool onlyActive = true)
        {
            var query = _context.UnidadMedida.Where(um => um.Codigo == codigo);

            if (onlyActive)
                query = query.Where(um => !um.Cancelado);

            if (excludeId.HasValue)
                query = query.Where(um => um.Id != excludeId.Value);

            return await query.AnyAsync();
        }

        public async Task<UnidadMedida> AddAsync(UnidadMedida entity)
        {
            await _context.UnidadMedida.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task UpdateAsync(UnidadMedida entity)
        {
            _context.UnidadMedida.Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}
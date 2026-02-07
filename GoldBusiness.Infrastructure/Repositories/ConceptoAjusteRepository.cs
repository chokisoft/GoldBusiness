using GoldBusiness.Domain.Entities;
using GoldBusiness.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace GoldBusiness.Infrastructure.Repositories
{
    public class ConceptoAjusteRepository : IConceptoAjusteRepository
    {
        private readonly ApplicationDbContext _context;

        public ConceptoAjusteRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ConceptoAjuste>> GetAllAsync()
            => await _context.ConceptoAjuste
                .Where(g => !g.Cancelado)
                .Include(g => g.Translations)
                .Include(g => g.Cuenta)
                    .ThenInclude(s => s.Translations)
                .ToListAsync();

        public async Task<ConceptoAjuste?> GetByIdAsync(int id)
            => await _context.ConceptoAjuste
                .Include(g => g.Translations)
                .Include(g => g.Cuenta)
                    .ThenInclude(s => s.Translations)
                .FirstOrDefaultAsync(g => g.Id == id);

        public async Task<ConceptoAjuste?> GetByCodigoAsync(string codigo, bool includeCanceled = false)
        {
            var query = _context.ConceptoAjuste
                .Include(g => g.Translations)
                .Include(g => g.Cuenta)
                    .ThenInclude(s => s.Translations)
                .Where(g => g.Codigo == codigo);

            if (!includeCanceled)
                query = query.Where(g => !g.Cancelado);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<bool> ExistsByCodigoAsync(string codigo, int? excludeId = null, bool onlyActive = true)
        {
            var query = _context.ConceptoAjuste.Where(g => g.Codigo == codigo);

            if (onlyActive)
                query = query.Where(g => !g.Cancelado);

            if (excludeId.HasValue)
                query = query.Where(g => g.Id != excludeId.Value);

            return await query.AnyAsync();
        }

        public async Task AddAsync(ConceptoAjuste entity)
        {
            _context.ConceptoAjuste.Add(entity);
            await _context.SaveChangesAsync();

            // Cargar traducciones y sublineas (y sus traducciones) para la entidad en memoria
            await _context.Entry(entity)
                .Collection(e => e.Translations)
                .LoadAsync();

            await _context.Entry(entity)
                .Reference(e => e.Cuenta)
                .Query()
                .Include(c => c.Translations)
                .LoadAsync();
        }

        public async Task UpdateAsync(ConceptoAjuste entity)
        {
            _context.ConceptoAjuste.Update(entity);
            await _context.SaveChangesAsync();

            // Asegurar que las traducciones y sublineas quedan cargadas
            await _context.Entry(entity)
                .Collection(e => e.Translations)
                .LoadAsync();

            await _context.Entry(entity)
                .Reference(e => e.Cuenta)
                .Query()
                .Include(c => c.Translations)
                .LoadAsync();
        }
    }
}
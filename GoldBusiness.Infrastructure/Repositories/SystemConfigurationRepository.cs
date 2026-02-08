using GoldBusiness.Domain.Entities;
using GoldBusiness.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace GoldBusiness.Infrastructure.Repositories
{
    public class SystemConfigurationRepository : ISystemConfigurationRepository
    {
        private readonly ApplicationDbContext _context;

        public SystemConfigurationRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<SystemConfiguration>> GetAllAsync()
            => await _context.SystemConfiguration
                .Include(s => s.CuentaPagarNavigation)
                    .ThenInclude(c => c.Translations)
                .Include(s => s.CuentaCobrarNavigation)
                    .ThenInclude(c => c.Translations)
                .Include(s => s.Translations)
                .ToListAsync();

        public async Task<SystemConfiguration?> GetByIdAsync(int id)
            => await _context.SystemConfiguration
                .Include(s => s.CuentaPagarNavigation)
                    .ThenInclude(c => c.Translations)
                .Include(s => s.CuentaCobrarNavigation)
                    .ThenInclude(c => c.Translations)
                .Include(s => s.Translations)
                .FirstOrDefaultAsync(s => s.Id == id);

        /// <summary>
        /// Obtiene una configuración por su código de sistema.
        /// </summary>
        /// <param name="codigo">Código del sistema</param>
        /// <param name="includeCanceled">No aplica para SystemConfiguration (no tiene propiedad Cancelado)</param>
        public async Task<SystemConfiguration?> GetByCodigoAsync(string codigo, bool includeCanceled = false)
        {
            // includeCanceled se ignora porque SystemConfiguration no tiene propiedad Cancelado
            return await _context.SystemConfiguration
                .Include(s => s.CuentaPagarNavigation)
                    .ThenInclude(c => c.Translations)
                .Include(s => s.CuentaCobrarNavigation)
                    .ThenInclude(c => c.Translations)
                .Include(s => s.Translations)
                .FirstOrDefaultAsync(s => s.CodigoSistema == codigo);
        }

        /// <summary>
        /// Verifica si existe una configuración con el código especificado.
        /// </summary>
        /// <param name="codigo">Código a verificar</param>
        /// <param name="excludeId">ID a excluir de la búsqueda (para actualizaciones)</param>
        /// <param name="onlyActive">No aplica para SystemConfiguration (no tiene propiedad Cancelado)</param>
        public async Task<bool> ExistsByCodigoAsync(string codigo, int? excludeId = null, bool onlyActive = true)
        {
            // onlyActive se ignora porque SystemConfiguration no tiene propiedad Cancelado
            var query = _context.SystemConfiguration
                .Where(s => s.CodigoSistema == codigo);

            if (excludeId.HasValue)
                query = query.Where(s => s.Id != excludeId.Value);

            return await query.AnyAsync();
        }

        public async Task AddAsync(SystemConfiguration entity)
        {
            _context.SystemConfiguration.Add(entity);
            await _context.SaveChangesAsync();

            // Cargar las navegaciones después de guardar
            await _context.Entry(entity)
                .Collection(e => e.Translations)
                .LoadAsync();

            await _context.Entry(entity)
                .Reference(e => e.CuentaPagarNavigation)
                .Query()
                .Include(c => c.Translations)
                .LoadAsync();

            await _context.Entry(entity)
                .Reference(e => e.CuentaCobrarNavigation)
                .Query()
                .Include(c => c.Translations)
                .LoadAsync();
        }

        public async Task UpdateAsync(SystemConfiguration entity)
        {
            _context.SystemConfiguration.Update(entity);
            await _context.SaveChangesAsync();

            // Recargar navegaciones
            await _context.Entry(entity)
                .Collection(e => e.Translations)
                .LoadAsync();

            await _context.Entry(entity)
                .Reference(e => e.CuentaPagarNavigation)
                .Query()
                .Include(c => c.Translations)
                .LoadAsync();

            await _context.Entry(entity)
                .Reference(e => e.CuentaCobrarNavigation)
                .Query()
                .Include(c => c.Translations)
                .LoadAsync();
        }
    }
}
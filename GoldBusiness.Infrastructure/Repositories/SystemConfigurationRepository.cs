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
                .Include(s => s.CuentaPagar)
                    .ThenInclude(c => c.Translations)
                .Include(s => s.CuentaCobrar)
                    .ThenInclude(c => c.Translations)
                .Include(s => s.Translations)
                .OrderBy(c => c.CodigoSistema)
                .ToListAsync();

        public async Task<(IEnumerable<SystemConfiguration> Items, int Total)> GetPagedAsync(int page, int pageSize, string? termino = null)
        {
            var query = _context.SystemConfiguration
                .Include(s => s.CuentaPagar)
                    .ThenInclude(c => c.Translations)
                .Include(s => s.CuentaCobrar)
                    .ThenInclude(c => c.Translations)
                .Include(s => s.Translations)
                .Where(s => !s.Cancelado)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(termino))
            {
                var termLower = termino.ToLower();
                query = query.Where(s =>
                    s.CodigoSistema.ToLower().Contains(termLower) ||
                    s.NombreNegocio.ToLower().Contains(termLower) ||
                    s.Licencia.ToLower().Contains(termLower));
            }

            var total = await query.CountAsync();
            var items = await query
                .OrderBy(s => s.CodigoSistema)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, total);
        }

        public async Task<SystemConfiguration?> GetByIdAsync(int id)
            => await _context.SystemConfiguration
                .Include(s => s.CuentaPagar)
                    .ThenInclude(c => c.Translations)
                .Include(s => s.CuentaCobrar)
                    .ThenInclude(c => c.Translations)
                .Include(s => s.Translations)
                .FirstOrDefaultAsync(s => s.Id == id);

        public async Task<SystemConfiguration?> GetByCodigoAsync(string codigo, bool includeCanceled = false)
        {
            return await _context.SystemConfiguration
                .Include(s => s.CuentaPagar)
                    .ThenInclude(c => c.Translations)
                .Include(s => s.CuentaCobrar)
                    .ThenInclude(c => c.Translations)
                .Include(s => s.Translations)
                .FirstOrDefaultAsync(s => s.CodigoSistema == codigo && (includeCanceled || !s.Cancelado));
        }

        public async Task<bool> ExistsByCodigoAsync(string codigo, int? excludeId = null, bool onlyActive = true)
        {
            if (codigo == null) codigo = string.Empty;
            var codigoNormalized = codigo.Trim();

            var query = _context.SystemConfiguration
                .Where(s => s.CodigoSistema == codigoNormalized && (onlyActive ? !s.Cancelado : true));

            if (excludeId.HasValue)
                query = query.Where(s => s.Id != excludeId.Value);

            return await query.AnyAsync();
        }

        public async Task AddAsync(SystemConfiguration entity)
        {
            await _context.SystemConfiguration.AddAsync(entity);
            await _context.SaveChangesAsync();

            // Cargar las navegaciones después de guardar
            await _context.Entry(entity)
                .Collection(e => e.Translations)
                .LoadAsync();

            await _context.Entry(entity)
                .Reference(e => e.CuentaPagar)
                .Query()
                .Include(c => c.Translations)
                .LoadAsync();

            await _context.Entry(entity)
                .Reference(e => e.CuentaCobrar)
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
                .Reference(e => e.CuentaPagar)
                .Query()
                .Include(c => c.Translations)
                .LoadAsync();

            await _context.Entry(entity)
                .Reference(e => e.CuentaCobrar)
                .Query()
                .Include(c => c.Translations)
                .LoadAsync();
        }
    }
}
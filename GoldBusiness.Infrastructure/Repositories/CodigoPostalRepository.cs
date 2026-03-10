using GoldBusiness.Domain.Entities;
using GoldBusiness.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace GoldBusiness.Infrastructure.Repositories
{
    public class CodigoPostalRepository : ICodigoPostalRepository
    {
        private readonly ApplicationDbContext _context;

        public CodigoPostalRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CodigoPostal>> GetAllAsync()
            => await _context.CodigoPostal
                .Where(cp => !cp.Cancelado)
                .Include(cp => cp.Municipio)
                    .ThenInclude(m => m!.Translations)
                .Include(cp => cp.Municipio!.Provincia)
                    .ThenInclude(p => p!.Translations)
                .OrderBy(cp => cp.Codigo)
                .ToListAsync();

        public async Task<(IEnumerable<CodigoPostal> Items, int Total)> GetPagedAsync(int page, int pageSize, string termino = null, int? municipioId = null)
        {
            var query = _context.CodigoPostal
                .AsNoTracking()
                .Where(cp => !cp.Cancelado);

            if (!string.IsNullOrWhiteSpace(termino))
                query = query.Where(cp => cp.Codigo.Contains(termino));

            if (municipioId.HasValue)
                query = query.Where(cp => cp.MunicipioId == municipioId.Value);

            var total = await query.CountAsync();

            var items = await query
                .Include(cp => cp.Municipio)
                    .ThenInclude(m => m!.Translations)
                .Include(cp => cp.Municipio!.Provincia)
                    .ThenInclude(p => p!.Translations)
                .OrderBy(cp => cp.Codigo)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, total);
        }

        public async Task<IEnumerable<CodigoPostal>> GetByMunicipioIdAsync(int municipioId)
            => await _context.CodigoPostal
                .Where(cp => cp.MunicipioId == municipioId && !cp.Cancelado)
                .Include(cp => cp.Municipio)
                    .ThenInclude(m => m!.Translations)
                .Include(cp => cp.Municipio!.Provincia)
                    .ThenInclude(p => p!.Translations)
                .OrderBy(cp => cp.Codigo)
                .ToListAsync();

        public async Task<CodigoPostal?> GetByIdAsync(int id)
            => await _context.CodigoPostal
                .Include(cp => cp.Municipio)
                    .ThenInclude(m => m!.Translations)
                .Include(cp => cp.Municipio!.Provincia)
                    .ThenInclude(p => p!.Translations)
                .FirstOrDefaultAsync(cp => cp.Id == id);

        public async Task<CodigoPostal?> GetByCodigoAsync(string codigo, bool includeCanceled = false)
        {
            var query = _context.CodigoPostal.Where(cp => cp.Codigo == codigo);
            if (!includeCanceled)
                query = query.Where(cp => !cp.Cancelado);

            return await query
                .Include(cp => cp.Municipio)
                    .ThenInclude(m => m!.Translations)
                .Include(cp => cp.Municipio!.Provincia)
                    .ThenInclude(p => p!.Translations)
                .FirstOrDefaultAsync();
        }

        public async Task<CodigoPostal> AddAsync(CodigoPostal entity)
        {
            _context.CodigoPostal.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<IEnumerable<CodigoPostal>> BuscarAsync(string termino, int? municipioId = null)
        {
            var query = _context.CodigoPostal
                .Where(cp => !cp.Cancelado);

            if (!string.IsNullOrWhiteSpace(termino))
                query = query.Where(cp => cp.Codigo.Contains(termino));

            if (municipioId.HasValue)
                query = query.Where(cp => cp.MunicipioId == municipioId.Value);

            return await query
                .Include(cp => cp.Municipio)
                    .ThenInclude(m => m!.Translations)
                .Include(cp => cp.Municipio!.Provincia)
                    .ThenInclude(p => p!.Translations)
                .OrderBy(cp => cp.Codigo)
                .ToListAsync();
        }

        public async Task UpdateAsync(CodigoPostal entity)
        {
            _context.CodigoPostal.Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}
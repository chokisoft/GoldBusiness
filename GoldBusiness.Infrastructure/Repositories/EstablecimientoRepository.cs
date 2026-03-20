using GoldBusiness.Domain.Entities;
using GoldBusiness.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace GoldBusiness.Infrastructure.Repositories
{
    public class EstablecimientoRepository : IEstablecimientoRepository
    {
        private readonly ApplicationDbContext _context;

        public EstablecimientoRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Establecimiento>> GetAllAsync()
            => await _context.Establecimiento
                .Include(e => e.Negocio)
                .Include(e => e.Pais)
                .Include(e => e.Provincia)
                .Include(e => e.Municipio)
                .Include(e => e.CodigoPostal)
                .Include(e => e.Translations)
                .Where(e => !e.Cancelado)
                .OrderBy(e => e.Codigo)
                .ToListAsync();

        public async Task<(IEnumerable<Establecimiento> Items, int Total)> GetPagedAsync(
            int page, 
            int pageSize, 
            string? termino = null, 
            int? negocioId = null)
        {
            var query = _context.Establecimiento
                .Include(e => e.Negocio)
                .Include(e => e.Pais)
                .Include(e => e.Provincia)
                .Include(e => e.Municipio)
                .Include(e => e.CodigoPostal)
                .Include(e => e.Translations)
                .Where(e => !e.Cancelado)
                .AsQueryable();

            if (negocioId.HasValue)
            {
                query = query.Where(e => e.NegocioId == negocioId.Value);
            }

            if (!string.IsNullOrWhiteSpace(termino))
            {
                var terminoLower = termino.ToLower();
                query = query.Where(e =>
                    e.Codigo.ToLower().Contains(terminoLower) ||
                    e.Descripcion.ToLower().Contains(terminoLower));
            }

            var total = await query.CountAsync();
            var items = await query
                .OrderBy(e => e.Codigo)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, total);
        }

        public async Task<IEnumerable<Establecimiento>> GetByNegocioIdAsync(int negocioId)
            => await _context.Establecimiento
                .Include(e => e.Negocio)
                .Include(e => e.Pais)
                .Include(e => e.Provincia)
                .Include(e => e.Municipio)
                .Include(e => e.CodigoPostal)
                .Include(e => e.Translations)
                .Where(e => e.NegocioId == negocioId && !e.Cancelado)
                .OrderBy(e => e.Codigo)
                .ToListAsync();

        public async Task<Establecimiento?> GetByIdAsync(int id)
            => await _context.Establecimiento
                .Include(e => e.Negocio)
                .Include(e => e.Pais)
                .Include(e => e.Provincia)
                .Include(e => e.Municipio)
                .Include(e => e.CodigoPostal)
                .Include(e => e.Translations)
                .FirstOrDefaultAsync(e => e.Id == id);

        public async Task<Establecimiento?> GetByCodigoAsync(string codigo, bool includeCanceled = false)
        {
            if (codigo == null) codigo = string.Empty;
            var codigoUpper = codigo.ToUpper();

            return await _context.Establecimiento
                .Include(e => e.Negocio)
                .Include(e => e.Pais)
                .Include(e => e.Provincia)
                .Include(e => e.Municipio)
                .Include(e => e.CodigoPostal)
                .Include(e => e.Translations)
                .FirstOrDefaultAsync(e => e.Codigo.ToUpper() == codigoUpper && (includeCanceled || !e.Cancelado));
        }

        public async Task<bool> ExistsByCodigoAsync(string codigo, int? excludeId = null, bool includeCanceled = false)
        {
            if (codigo == null) codigo = string.Empty;
            var codigoUpper = codigo.ToUpper();

            var query = _context.Establecimiento
                .Where(e => e.Codigo.ToUpper() == codigoUpper && (includeCanceled || !e.Cancelado));

            if (excludeId.HasValue)
            {
                query = query.Where(e => e.Id != excludeId.Value);
            }

            return await query.AnyAsync();
        }

        public async Task<Establecimiento> AddAsync(Establecimiento entity)
        {
            await _context.Establecimiento.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task UpdateAsync(Establecimiento entity)
        {
            _context.Establecimiento.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                _context.Establecimiento.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}
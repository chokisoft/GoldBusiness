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
                .OrderBy(cp => cp.Codigo)
                .ToListAsync();

        public async Task<IEnumerable<CodigoPostal>> GetByMunicipioIdAsync(int municipioId)
            => await _context.CodigoPostal
                .Where(cp => cp.MunicipioId == municipioId && !cp.Cancelado)
                .Include(cp => cp.Municipio)
                .OrderBy(cp => cp.Codigo)
                .ToListAsync();

        public async Task<CodigoPostal?> GetByIdAsync(int id)
            => await _context.CodigoPostal
                .Include(cp => cp.Municipio)
                .FirstOrDefaultAsync(cp => cp.Id == id);

        public async Task<CodigoPostal?> GetByCodigoAsync(string codigo, bool includeCanceled = false)
        {
            var query = _context.CodigoPostal.Where(cp => cp.Codigo == codigo);
            if (!includeCanceled)
                query = query.Where(cp => !cp.Cancelado);
            return await query.Include(cp => cp.Municipio).FirstOrDefaultAsync();
        }

        public async Task<CodigoPostal> AddAsync(CodigoPostal entity)
        {
            _context.CodigoPostal.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task UpdateAsync(CodigoPostal entity)
        {
            _context.CodigoPostal.Update(entity);
            await _context.SaveChangesAsync();
        }
    }
}
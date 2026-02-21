using GoldBusiness.Application.Interfaces;
using GoldBusiness.Domain.DTOs;
using GoldBusiness.Domain.Entities;
using GoldBusiness.Infrastructure.Repositories;

namespace GoldBusiness.Application.Services
{
    public class PaisService : IPaisService
    {
        private readonly IPaisRepository _repo;

        public PaisService(IPaisRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<PaisDTO>> GetAllAsync(string lang = "es")
            => (await _repo.GetAllAsync())
                .Select(p => MapToDTO(p, lang))
                .ToList();

        public async Task<PaisDTO?> GetByIdAsync(int id, string lang = "es")
        {
            var entity = await _repo.GetByIdAsync(id);
            return entity != null ? MapToDTO(entity, lang) : null;
        }

        public async Task<PaisDTO?> GetByCodigoAsync(string codigo, string lang = "es")
        {
            var entity = await _repo.GetByCodigoAsync(codigo);
            return entity != null ? MapToDTO(entity, lang) : null;
        }

        private static PaisDTO MapToDTO(Pais entity, string lang)
        {
            var translation = entity.Translations.FirstOrDefault(t => t.Language == lang);
            var descripcion = translation?.Descripcion ?? entity.Descripcion;

            return new PaisDTO
            {
                Id = entity.Id,
                Codigo = entity.Codigo,
                CodigoAlpha2 = entity.CodigoAlpha2,
                CodigoTelefono = entity.CodigoTelefono,
                Descripcion = descripcion,
                RegexTelefono = entity.RegexTelefono,
                FormatoTelefono = entity.FormatoTelefono,
                FormatoEjemplo = entity.FormatoEjemplo
            };
        }
    }
}
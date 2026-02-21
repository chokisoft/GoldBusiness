using GoldBusiness.Application.Interfaces;
using GoldBusiness.Domain.DTOs;
using GoldBusiness.Domain.Entities;
using GoldBusiness.Infrastructure.Repositories;

namespace GoldBusiness.Application.Services
{
    public class MunicipioService : IMunicipioService
    {
        private readonly IMunicipioRepository _repo;

        public MunicipioService(IMunicipioRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<MunicipioDTO>> GetAllAsync(string lang = "es")
            => (await _repo.GetAllAsync())
                .Select(m => MapToDTO(m, lang))
                .ToList();

        public async Task<IEnumerable<MunicipioDTO>> GetByProvinciaIdAsync(int provinciaId, string lang = "es")
            => (await _repo.GetByProvinciaIdAsync(provinciaId))
                .Select(m => MapToDTO(m, lang))
                .ToList();

        public async Task<MunicipioDTO?> GetByIdAsync(int id, string lang = "es")
        {
            var entity = await _repo.GetByIdAsync(id);
            return entity != null ? MapToDTO(entity, lang) : null;
        }

        public async Task<IEnumerable<MunicipioDTO>> BuscarAsync(string termino, int? paisId = null, string lang = "es")
            => (await _repo.BuscarAsync(termino, paisId))
                .Select(m => MapToDTO(m, lang))
                .ToList();

        private static MunicipioDTO MapToDTO(Municipio entity, string lang)
        {
            var translation = entity.Translations.FirstOrDefault(t => t.Language == lang);
            var descripcion = translation?.Descripcion ?? entity.Descripcion;

            var provinciaTranslation = entity.Provincia.Translations.FirstOrDefault(t => t.Language == lang);
            var provinciaDescripcion = provinciaTranslation?.Descripcion ?? entity.Provincia.Descripcion;

            return new MunicipioDTO
            {
                Id = entity.Id,
                Codigo = entity.Codigo,
                Descripcion = descripcion,
                ProvinciaId = entity.ProvinciaId,
                ProvinciaCodigo = entity.Provincia.Codigo,
                ProvinciaDescripcion = provinciaDescripcion
            };
        }
    }
}
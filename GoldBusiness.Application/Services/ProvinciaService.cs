using GoldBusiness.Application.Interfaces;
using GoldBusiness.Domain.DTOs;
using GoldBusiness.Domain.Entities;
using GoldBusiness.Infrastructure.Repositories;
using System.Linq;

namespace GoldBusiness.Application.Services
{
    public class ProvinciaService : IProvinciaService
    {
        private readonly IProvinciaRepository _repo;

        public ProvinciaService(IProvinciaRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<ProvinciaDTO>> GetAllAsync(string lang = "es")
            => (await _repo.GetAllAsync())
                .Select(p => MapToDTO(p, lang))
                .ToList();

        // Implementación adicional para cumplir la firma del interface
        public Task<IEnumerable<ProvinciaDTO>> GetByProvinciaIdAsync(int paisId, string lang = "es")
            => GetByPaisIdAsync(paisId, lang);

        public async Task<IEnumerable<ProvinciaDTO>> GetByPaisIdAsync(int paisId, string lang = "es")
            => (await _repo.GetByPaisIdAsync(paisId))
                .Select(p => MapToDTO(p, lang))
                .ToList();

        public async Task<ProvinciaDTO?> GetByIdAsync(int id, string lang = "es")
        {
            var entity = await _repo.GetByIdAsync(id);
            return entity != null ? MapToDTO(entity, lang) : null;
        }

        public async Task<IEnumerable<ProvinciaDTO>> BuscarAsync(string termino, int? paisId = null, string lang = "es")
            => (await _repo.BuscarAsync(termino, paisId))
                .Select(m => MapToDTO(m, lang))
                .ToList();

        private static ProvinciaDTO MapToDTO(Provincia entity, string lang)
        {
            var translation = entity.Translations.FirstOrDefault(t => t.Language == lang);
            var descripcion = translation?.Descripcion ?? entity.Descripcion;

            var paisTranslation = entity.Pais.Translations.FirstOrDefault(t => t.Language == lang);
            var paisDescripcion = paisTranslation?.Descripcion ?? entity.Pais.Descripcion;

            return new ProvinciaDTO
            {
                Id = entity.Id,
                Codigo = entity.Codigo,
                Descripcion = descripcion,
                PaisId = entity.PaisId,
                PaisCodigo = entity.Pais.Codigo,
                PaisDescripcion = paisDescripcion
            };
        }
    }
}
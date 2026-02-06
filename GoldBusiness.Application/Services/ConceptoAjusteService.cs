using GoldBusiness.Application.Interfaces;
using GoldBusiness.Domain.DTOs;
using GoldBusiness.Domain.Entities;
using GoldBusiness.Infrastructure.Repositories;

namespace GoldBusiness.Application.Services
{
    public class ConceptoAjusteService : IConceptoAjusteService
    {
        private readonly IConceptoAjusteRepository _repo;

        public ConceptoAjusteService(IConceptoAjusteRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<ConceptoAjusteDTO>> GetAllAsync(string lang = "es")
            => (await _repo.GetAllAsync())
                .Select(g => MapToDTO(g, lang))
                .Where(dto => dto is not null)
                .Select(dto => dto!)
                .ToList();

        public async Task<ConceptoAjusteDTO?> GetByIdAsync(int id, string lang = "es")
            => MapToDTO(await _repo.GetByIdAsync(id), lang);

        public async Task<ConceptoAjusteDTO> CreateAsync(ConceptoAjusteDTO dto, string user, string lang = "es")
        {
            var creador = user ?? "system";
            var entity = new ConceptoAjuste(dto.Codigo, dto.Descripcion, dto.CuentaId, creador);

            await _repo.AddAsync(entity);

            entity.AddOrUpdateTranslation(lang, dto.Descripcion, creador);
            await _repo.UpdateAsync(entity);

            return MapToDTO(entity, lang)!;
        }

        public async Task<ConceptoAjusteDTO> UpdateAsync(int id, ConceptoAjusteDTO dto, string user, string lang = "es")
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null) throw new KeyNotFoundException();

            entity.Update(dto.Descripcion, dto.CuentaId, user);

            entity.AddOrUpdateTranslation(lang, dto.Descripcion, user ?? "system");

            await _repo.UpdateAsync(entity);
            return MapToDTO(entity, lang)!;
        }

        public async Task<ConceptoAjusteDTO?> SoftDeleteAsync(int id, string user)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null) return null;

            entity.SoftDelete(user);

            await _repo.UpdateAsync(entity);
            return MapToDTO(entity, "es");
        }

        public async Task AddOrUpdateTranslationAsync(int id, string lang, string descripcion, string user)
        {
            if (string.IsNullOrWhiteSpace(lang)) lang = "es";
            if (string.IsNullOrWhiteSpace(descripcion)) throw new ArgumentException("Descripción requerida.", nameof(descripcion));

            var entity = await _repo.GetByIdAsync(id);
            if (entity == null) throw new KeyNotFoundException();

            entity.AddOrUpdateTranslation(lang, descripcion, user ?? "system");
            await _repo.UpdateAsync(entity);
        }

        private static ConceptoAjusteDTO? MapToDTO(ConceptoAjuste? g, string lang)
        {
            if (g == null) return null;

            return new ConceptoAjusteDTO
            {
                Id = g.Id,
                Codigo = g.Codigo,
                Descripcion = g.GetDescripcion(lang),
                CuentaCodigo = g.Cuenta?.Codigo ?? string.Empty,
                CuentaDescripcion = g.Cuenta?.GetDescripcion(lang) ?? string.Empty,
                Cancelado = g.Cancelado,
                CreadoPor = g.CreadoPor,
                FechaHoraCreado = g.FechaHoraCreado,
                ModificadoPor = g.ModificadoPor,
                FechaHoraModificado = g.FechaHoraModificado
            };
        }
    }
}
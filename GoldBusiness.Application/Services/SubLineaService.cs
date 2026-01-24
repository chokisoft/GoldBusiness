using GoldBusiness.Application.Interfaces;
using GoldBusiness.Domain.DTOs;
using GoldBusiness.Domain.Entities;
using GoldBusiness.Infrastructure.Repositories;

namespace GoldBusiness.Application.Services
{
    public class SubLineaService : ISubLineaService
    {
        private readonly ISubLineaRepository _repo;

        public SubLineaService(ISubLineaRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<SubLineaDTO>> GetAllAsync(string lang = "es")
            => (await _repo.GetAllAsync())
                .Select(s => MapToDTO(s, lang))
                .Where(dto => dto is not null)
                .Select(dto => dto!)
                .ToList();

        public async Task<SubLineaDTO?> GetByIdAsync(int id, string lang = "es")
            => MapToDTO(await _repo.GetByIdAsync(id), lang);

        public async Task<SubLineaDTO> CreateAsync(SubLineaDTO dto, string user, string lang = "es")
        {
            var creador = user ?? "system";
            var entity = new SubLinea(dto.Codigo, dto.Descripcion, dto.LineaId, creador);

            await _repo.AddAsync(entity);

            entity.AddOrUpdateTranslation(lang, dto.Descripcion, creador);
            await _repo.UpdateAsync(entity);

            return MapToDTO(entity, lang)!;
        }

        public async Task<SubLineaDTO> UpdateAsync(int id, SubLineaDTO dto, string user, string lang = "es")
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null) throw new KeyNotFoundException();

            // ✅ Ahora usa dto.Deudora
            entity.Update(dto.Descripcion, dto.LineaId, user);

            entity.AddOrUpdateTranslation(lang, dto.Descripcion, user ?? "system");

            await _repo.UpdateAsync(entity);
            return MapToDTO(entity, lang)!;
        }

        public async Task<SubLineaDTO?> SoftDeleteAsync(int id, string user)
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

        private static SubLineaDTO? MapToDTO(SubLinea? s, string lang)
        {
            if (s == null) return null;

            return new SubLineaDTO
            {
                Id = s.Id,
                Codigo = s.Codigo,
                LineaId = s.LineaId,
                LineaCodigo = s.Linea?.Codigo ?? string.Empty,
                LineaDescripcion = s.Linea?.GetDescripcion(lang) ?? string.Empty,
                Descripcion = s.GetDescripcion(lang),
                Cancelado = s.Cancelado,
                CreadoPor = s.CreadoPor,
                FechaHoraCreado = s.FechaHoraCreado,
                ModificadoPor = s.ModificadoPor,
                FechaHoraModificado = s.FechaHoraModificado
            };
        }
    }
}
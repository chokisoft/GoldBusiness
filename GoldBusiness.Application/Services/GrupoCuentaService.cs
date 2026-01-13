using GoldBusiness.Application.Interfaces;
using GoldBusiness.Domain.DTOs;
using GoldBusiness.Domain.Entities;
using GoldBusiness.Infrastructure.Repositories;

namespace GoldBusiness.Application.Services
{
    public class GrupoCuentaService : IGrupoCuentaService
    {
        private readonly IGrupoCuentaRepository _repo;

        public GrupoCuentaService(IGrupoCuentaRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<GrupoCuentaDTO>> GetAllAsync(string lang = "es")
            => (await _repo.GetAllAsync())
                .Select(g => MapToDTO(g, lang))
                .Where(dto => dto is not null)
                .Select(dto => dto!)
                .ToList();

        public async Task<GrupoCuentaDTO?> GetByIdAsync(int id, string lang = "es")
            => MapToDTO(await _repo.GetByIdAsync(id), lang);

        public async Task<GrupoCuentaDTO> CreateAsync(GrupoCuentaDTO dto, string user, string lang = "es")
        {
            var creador = user ?? "system";
            var entity = new GrupoCuenta(dto.Codigo, dto.Descripcion, creador);

            await _repo.AddAsync(entity);

            entity.AddOrUpdateTranslation(lang, dto.Descripcion, creador);
            await _repo.UpdateAsync(entity);

            return MapToDTO(entity, lang)!;
        }

        public async Task<GrupoCuentaDTO> UpdateAsync(int id, GrupoCuentaDTO dto, string user, string lang = "es")
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null) throw new KeyNotFoundException();

            entity.Update(dto.Descripcion, user);

            entity.AddOrUpdateTranslation(lang, dto.Descripcion, user ?? "system");

            await _repo.UpdateAsync(entity);
            return MapToDTO(entity, lang)!;
        }

        public async Task<GrupoCuentaDTO?> SoftDeleteAsync(int id, string user)
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

        private static GrupoCuentaDTO? MapToDTO(GrupoCuenta? g, string lang)
        {
            if (g == null) return null;

            return new GrupoCuentaDTO
            {
                Id = g.Id,
                Codigo = g.Codigo,
                Descripcion = g.GetDescripcion(lang),
                Cancelado = g.Cancelado,
                CreadoPor = g.CreadoPor,
                FechaHoraCreado = g.FechaHoraCreado,
                ModificadoPor = g.ModificadoPor,
                FechaHoraModificado = g.FechaHoraModificado
            };
        }
    }
}
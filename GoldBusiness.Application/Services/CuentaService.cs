using GoldBusiness.Application.Interfaces;
using GoldBusiness.Domain.DTOs;
using GoldBusiness.Domain.Entities;
using GoldBusiness.Infrastructure.Repositories;

namespace GoldBusiness.Application.Services
{
    public class CuentaService : ICuentaService
    {
        private readonly ICuentaRepository _repo;

        public CuentaService(ICuentaRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<CuentaDTO>> GetAllAsync(string lang = "es")
            => (await _repo.GetAllAsync())
                .Select(c => MapToDTO(c, lang))
                .Where(dto => dto is not null)
                .Select(dto => dto!)
                .ToList();

        public async Task<CuentaDTO?> GetByIdAsync(int id, string lang = "es")
            => MapToDTO(await _repo.GetByIdAsync(id), lang);

        public async Task<CuentaDTO> CreateAsync(CuentaDTO dto, string user, string lang = "es")
        {
            var creador = user ?? "system";
            var entity = new Cuenta(dto.Codigo, dto.Descripcion, dto.SubGrupoCuentaId, creador);

            // Guardar entidad para obtener Id
            await _repo.AddAsync(entity);

            // Añadir traducción en idioma seleccionado
            entity.AddOrUpdateTranslation(lang, dto.Descripcion, creador);
            await _repo.UpdateAsync(entity);

            return MapToDTO(entity, lang)!;
        }

        public async Task<CuentaDTO> UpdateAsync(int id, CuentaDTO dto, string user, string lang = "es")
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null) throw new KeyNotFoundException();

            entity.Update(dto.Descripcion, dto.SubGrupoCuentaId, user);

            // Actualizar/crear traducción
            entity.AddOrUpdateTranslation(lang, dto.Descripcion, user ?? "system");

            await _repo.UpdateAsync(entity);
            return MapToDTO(entity, lang)!;
        }

        public async Task<CuentaDTO?> SoftDeleteAsync(int id, string user)
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

        private static CuentaDTO? MapToDTO(Cuenta? c, string lang)
        {
            if (c == null) return null;

            return new CuentaDTO
            {
                Id = c.Id,
                Codigo = c.Codigo,
                Descripcion = c.GetDescripcion(lang),
                SubGrupoCuentaId = c.SubGrupoCuentaId,
                SubGrupoCuentaCodigo = c.SubGrupoCuenta?.Codigo ?? string.Empty,
                SubGrupoCuentaDescripcion = c.SubGrupoCuenta?.GetDescripcion(lang) ?? string.Empty,
                GrupoCuentaCodigo = c.SubGrupoCuenta?.GrupoCuenta?.Codigo ?? string.Empty,
                GrupoCuentaDescripcion = c.SubGrupoCuenta?.GrupoCuenta?.GetDescripcion(lang) ?? string.Empty,
                Cancelado = c.Cancelado,
                CreadoPor = c.CreadoPor,
                FechaHoraCreado = c.FechaHoraCreado,
                ModificadoPor = c.ModificadoPor,
                FechaHoraModificado = c.FechaHoraModificado
            };
        }
    }
}
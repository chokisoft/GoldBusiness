using GoldBusiness.Application.Helpers;
using GoldBusiness.Application.Interfaces;
using GoldBusiness.Domain.DTOs;
using GoldBusiness.Domain.Entities;
using GoldBusiness.Infrastructure.Repositories;
using Microsoft.Extensions.Localization;

namespace GoldBusiness.Application.Services
{
    public class CuentaService : ICuentaService
    {
        private readonly ICuentaRepository _repo;
        private readonly IStringLocalizer<GoldBusiness.Domain.Resources.ValidationMessages> _localizer;

        public CuentaService(ICuentaRepository repo,
            IStringLocalizer<GoldBusiness.Domain.Resources.ValidationMessages> localizer)
        {
            _repo = repo;
            _localizer = localizer;
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

            // Usar el helper genérico
            var (existe, estaCancelado, existingEntity) = await CodigoValidationHelper
                .ValidateCodigoForCreateAsync(_repo, dto.Codigo);

            if (existe)
            {
                if (estaCancelado && existingEntity != null)
                {
                    // Reactivar el registro existente
                    existingEntity.Reactivar(dto.Descripcion, creador);
                    existingEntity.AddOrUpdateTranslation(lang, dto.Descripcion, creador);
                    await _repo.UpdateAsync(existingEntity);

                    return MapToDTO(existingEntity, lang)!;
                }
                else
                {
                    // Lanzar error con mensaje genérico
                    var errorMessage = CodigoValidationHelper.GetDuplicateCodeErrorMessage(
                        _localizer, dto.Codigo, false);
                    throw new InvalidOperationException(errorMessage);
                }
            }

            // No existe, crear nuevo registro
            var entity = new Cuenta(dto.Codigo, dto.Descripcion, dto.SystemConfigurationId, dto.SubGrupoCuentaId, creador);
            await _repo.AddAsync(entity);

            entity.AddOrUpdateTranslation(lang, dto.Descripcion, creador);
            await _repo.UpdateAsync(entity);

            return MapToDTO(entity, lang)!;
        }

        public async Task<CuentaDTO> UpdateAsync(int id, CuentaDTO dto, string user, string lang = "es")
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null) throw new KeyNotFoundException();

            // Si el código cambió, validar
            if (entity.Codigo != dto.Codigo)
            {
                // Verificar si existe otro registro con el nuevo código (incluyendo cancelados)
                var existingWithNewCode = await _repo.GetByCodigoAsync(dto.Codigo, includeCanceled: true);

                if (existingWithNewCode != null && existingWithNewCode.Id != id)
                {
                    if (existingWithNewCode.Cancelado)
                    {
                        // Existe pero está cancelado - no permitir el cambio
                        var errorMessage = $"Ya existe un registro cancelado con el código '{dto.Codigo}'. " +
                                         $"Considere reactivar el registro existente (ID: {existingWithNewCode.Id}).";
                        throw new InvalidOperationException(errorMessage);
                    }
                    else
                    {
                        // Existe y está activo
                        var errorMessage = string.Format(_localizer["CodigoDuplicado"].Value, dto.Codigo);
                        throw new InvalidOperationException(errorMessage);
                    }
                }

                entity.SetCodigo(dto.Codigo);
            }

            entity.Update(dto.Descripcion, dto.SystemConfigurationId, dto.SubGrupoCuentaId, user);

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
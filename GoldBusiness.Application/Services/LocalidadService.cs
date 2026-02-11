using GoldBusiness.Application.Helpers;
using GoldBusiness.Application.Interfaces;
using GoldBusiness.Domain.DTOs;
using GoldBusiness.Domain.Entities;
using GoldBusiness.Infrastructure.Repositories;
using Microsoft.Extensions.Localization;

namespace GoldBusiness.Application.Services
{
    public class LocalidadService : ILocalidadService
    {
        private readonly ILocalidadRepository _repo;
        private readonly IStringLocalizer<GoldBusiness.Domain.Resources.ValidationMessages> _localizer;

        public LocalidadService(
            ILocalidadRepository repo,
            IStringLocalizer<GoldBusiness.Domain.Resources.ValidationMessages> localizer)
        {
            _repo = repo;
            _localizer = localizer;
        }

        public async Task<IEnumerable<LocalidadDTO>> GetAllAsync(string lang = "es")
            => (await _repo.GetAllAsync())
                .Select(l => MapToDTO(l, lang))
                .Where(dto => dto is not null)
                .Select(dto => dto!)
                .ToList();

        public async Task<IEnumerable<LocalidadDTO>> GetByEstablecimientoIdAsync(int establecimientoId, string lang = "es")
            => (await _repo.GetByEstablecimientoIdAsync(establecimientoId))
                .Select(l => MapToDTO(l, lang))
                .Where(dto => dto is not null)
                .Select(dto => dto!)
                .ToList();

        public async Task<LocalidadDTO?> GetByIdAsync(int id, string lang = "es")
            => MapToDTO(await _repo.GetByIdAsync(id), lang);

        public async Task<LocalidadDTO> CreateAsync(LocalidadDTO dto, string user, string lang = "es")
        {
            var creador = user ?? "system";

            var (existe, estaCancelado, existingEntity) = await CodigoValidationHelper
                .ValidateCodigoForCreateAsync(_repo, dto.Codigo);

            if (existe)
            {
                if (estaCancelado && existingEntity != null)
                {
                    existingEntity.Reactivar(dto.Descripcion, creador);
                    existingEntity.AddOrUpdateTranslation(lang, dto.Descripcion, creador);
                    await _repo.UpdateAsync(existingEntity);

                    return MapToDTO(existingEntity, lang)!;
                }
                else
                {
                    var errorMessage = CodigoValidationHelper.GetDuplicateCodeErrorMessage(
                        _localizer, dto.Codigo, false);
                    throw new InvalidOperationException(errorMessage);
                }
            }

            var entity = new Localidad(
                dto.EstablecimientoId,
                dto.Codigo,
                dto.Descripcion,
                dto.CuentaInventarioId,
                dto.CuentaCostoId,
                dto.CuentaVentaId,
                dto.CuentaDevolucionId,
                dto.Almacen,
                creador);

            await _repo.AddAsync(entity);

            entity.AddOrUpdateTranslation(lang, dto.Descripcion, creador);
            await _repo.UpdateAsync(entity);

            return MapToDTO(entity, lang)!;
        }

        public async Task<LocalidadDTO> UpdateAsync(int id, LocalidadDTO dto, string user, string lang = "es")
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null) throw new KeyNotFoundException();

            if (entity.Codigo != dto.Codigo)
            {
                var existingWithNewCode = await _repo.GetByCodigoAsync(dto.Codigo, includeCanceled: true);

                if (existingWithNewCode != null && existingWithNewCode.Id != id)
                {
                    if (existingWithNewCode.Cancelado)
                    {
                        var errorMessage = $"Ya existe un registro cancelado con el código '{dto.Codigo}'. " +
                                         $"Considere reactivar el registro existente (ID: {existingWithNewCode.Id}).";
                        throw new InvalidOperationException(errorMessage);
                    }
                    else
                    {
                        var errorMessage = string.Format(_localizer["CodigoDuplicado"].Value, dto.Codigo);
                        throw new InvalidOperationException(errorMessage);
                    }
                }

                entity.SetCodigo(dto.Codigo);
            }

            entity.Update(
                dto.Descripcion,
                dto.CuentaInventarioId,
                dto.CuentaCostoId,
                dto.CuentaVentaId,
                dto.CuentaDevolucionId,
                dto.Almacen,
                user);

            entity.AddOrUpdateTranslation(lang, dto.Descripcion, user ?? "system");

            await _repo.UpdateAsync(entity);
            return MapToDTO(entity, lang)!;
        }

        public async Task<LocalidadDTO?> SoftDeleteAsync(int id, string user)
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
            if (string.IsNullOrWhiteSpace(descripcion))
                throw new ArgumentException("Descripción requerida.", nameof(descripcion));

            var entity = await _repo.GetByIdAsync(id);
            if (entity == null) throw new KeyNotFoundException();

            entity.AddOrUpdateTranslation(lang, descripcion, user ?? "system");
            await _repo.UpdateAsync(entity);
        }

        private static LocalidadDTO? MapToDTO(Localidad? l, string lang)
        {
            if (l == null) return null;

            return new LocalidadDTO
            {
                Id = l.Id,
                EstablecimientoId = l.EstablecimientoId,
                EstablecimientoCodigo = l.Establecimiento?.Codigo ?? string.Empty,
                EstablecimientoDescripcion = l.Establecimiento?.GetDescripcion(lang) ?? string.Empty,
                Codigo = l.Codigo,
                Descripcion = l.GetDescripcion(lang),
                Almacen = l.Almacen,
                CuentaInventarioId = l.CuentaInventarioId,
                CuentaInventarioCodigo = l.CuentaInventario?.Codigo ?? string.Empty,
                CuentaInventarioDescripcion = l.CuentaInventario?.GetDescripcion(lang) ?? string.Empty,
                CuentaCostoId = l.CuentaCostoId,
                CuentaCostoCodigo = l.CuentaCosto?.Codigo ?? string.Empty,
                CuentaCostoDescripcion = l.CuentaCosto?.GetDescripcion(lang) ?? string.Empty,
                CuentaVentaId = l.CuentaVentaId,
                CuentaVentaCodigo = l.CuentaVenta?.Codigo ?? string.Empty,
                CuentaVentaDescripcion = l.CuentaVenta?.GetDescripcion(lang) ?? string.Empty,
                CuentaDevolucionId = l.CuentaDevolucionId,
                CuentaDevolucionCodigo = l.CuentaDevolucion?.Codigo ?? string.Empty,
                CuentaDevolucionDescripcion = l.CuentaDevolucion?.GetDescripcion(lang) ?? string.Empty,
                Activo = l.Activo,
                Cancelado = l.Cancelado,
                CreadoPor = l.CreadoPor,
                FechaHoraCreado = l.FechaHoraCreado,
                ModificadoPor = l.ModificadoPor,
                FechaHoraModificado = l.FechaHoraModificado
            };
        }
    }
}
using GoldBusiness.Application.Helpers;
using GoldBusiness.Application.Interfaces;
using GoldBusiness.Domain.DTOs;
using GoldBusiness.Domain.Entities;
using GoldBusiness.Infrastructure.Repositories;
using Microsoft.Extensions.Localization;

namespace GoldBusiness.Application.Services
{
    public class ProductoService : IProductoService
    {
        private readonly IProductoRepository _repo;
        private readonly IStringLocalizer<GoldBusiness.Domain.Resources.ValidationMessages> _localizer;

        public ProductoService(
            IProductoRepository repo,
            IStringLocalizer<GoldBusiness.Domain.Resources.ValidationMessages> localizer)
        {
            _repo = repo;
            _localizer = localizer;
        }

        public async Task<IEnumerable<ProductoDTO>> GetAllAsync(string lang = "es")
            => (await _repo.GetAllAsync())
                .Select(p => MapToDTO(p, lang))
                .Where(dto => dto is not null)
                .Select(dto => dto!)
                .ToList();

        public async Task<IEnumerable<ProductoDTO>> GetByEstablecimientoIdAsync(int establecimientoId, string lang = "es")
            => (await _repo.GetByEstablecimientoIdAsync(establecimientoId))
                .Select(p => MapToDTO(p, lang))
                .Where(dto => dto is not null)
                .Select(dto => dto!)
                .ToList();

        public async Task<IEnumerable<ProductoDTO>> GetByProveedorIdAsync(int proveedorId, string lang = "es")
            => (await _repo.GetByProveedorIdAsync(proveedorId))
                .Select(p => MapToDTO(p, lang))
                .Where(dto => dto is not null)
                .Select(dto => dto!)
                .ToList();

        public async Task<IEnumerable<ProductoDTO>> GetBySubLineaIdAsync(int subLineaId, string lang = "es")
            => (await _repo.GetBySubLineaIdAsync(subLineaId))
                .Select(p => MapToDTO(p, lang))
                .Where(dto => dto is not null)
                .Select(dto => dto!)
                .ToList();

        public async Task<ProductoDTO?> GetByIdAsync(int id, string lang = "es")
            => MapToDTO(await _repo.GetByIdAsync(id), lang);

        public async Task<ProductoDTO> CreateAsync(ProductoDTO dto, string user, string lang = "es")
        {
            var creador = user ?? "system";

            var (existe, estaCancelado, existingEntity) = await CodigoValidationHelper
                .ValidateCodigoForCreateAsync(_repo, dto.Codigo);

            if (existe)
            {
                if (estaCancelado && existingEntity != null)
                {
                    existingEntity.Reactivar(dto.Descripcion, creador);

                    // ✅ CORREGIDO: Agregar todos los parámetros
                    existingEntity.AddOrUpdateTranslation(
                        lang,
                        dto.Descripcion,
                        dto.Caracteristicas ?? string.Empty,
                        creador);

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

            var entity = new Producto(
                dto.EstablecimientoId,
                dto.Codigo,
                dto.Descripcion,
                dto.UnidadMedidaId,
                dto.ProveedorId,
                dto.SubLineaId,
                dto.PrecioVenta,
                dto.PrecioCosto,
                dto.Iva,
                creador,
                dto.Servicio);

            if (!string.IsNullOrWhiteSpace(dto.CodigoReferencia))
                entity.SetCodigoReferencia(dto.CodigoReferencia);

            entity.SetStockMinimo(dto.StockMinimo);

            if (!string.IsNullOrWhiteSpace(dto.Caracteristicas))
                entity.SetCaracteristicas(dto.Caracteristicas);

            if (dto.Imagen != null && dto.Imagen.Length > 0)
                entity.SetImagen(dto.Imagen);

            await _repo.AddAsync(entity);

            // ✅ CORREGIDO: Agregar todos los parámetros
            entity.AddOrUpdateTranslation(
                lang,
                dto.Descripcion,
                dto.Caracteristicas ?? string.Empty,
                creador);

            await _repo.UpdateAsync(entity);

            return MapToDTO(entity, lang)!;
        }

        public async Task<ProductoDTO> UpdateAsync(int id, ProductoDTO dto, string user, string lang = "es")
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

            // ✅ CORREGIDO: Agregar TODOS los parámetros en el orden correcto
            entity.Update(
                dto.Descripcion,
                dto.UnidadMedidaId,
                dto.ProveedorId,
                dto.SubLineaId,
                dto.PrecioVenta,
                dto.PrecioCosto,
                dto.Iva,
                dto.CodigoReferencia ?? string.Empty,
                dto.StockMinimo,
                dto.Servicio,
                dto.Caracteristicas ?? string.Empty,
                user);

            if (dto.Imagen != null && dto.Imagen.Length > 0)
                entity.SetImagen(dto.Imagen);

            // ✅ CORREGIDO: Agregar todos los parámetros
            entity.AddOrUpdateTranslation(
                lang,
                dto.Descripcion,
                dto.Caracteristicas ?? string.Empty,
                user ?? "system");

            await _repo.UpdateAsync(entity);
            return MapToDTO(entity, lang)!;
        }

        public async Task<ProductoDTO?> SoftDeleteAsync(int id, string user)
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

            // ✅ CORREGIDO: Agregar todos los parámetros
            entity.AddOrUpdateTranslation(
                lang,
                descripcion,
                string.Empty,
                user ?? "system");

            await _repo.UpdateAsync(entity);
        }

        private static ProductoDTO? MapToDTO(Producto? p, string lang)
        {
            if (p == null) return null;

            return new ProductoDTO
            {
                Id = p.Id,
                EstablecimientoId = p.EstablecimientoId,
                EstablecimientoCodigo = p.Establecimiento?.Codigo ?? string.Empty,
                EstablecimientoDescripcion = p.Establecimiento?.GetDescripcion(lang) ?? string.Empty,
                Codigo = p.Codigo,
                Descripcion = p.GetDescripcion(lang),
                UnidadMedidaId = p.UnidadMedidaId,
                UnidadMedidaCodigo = p.UnidadMedida?.Codigo ?? string.Empty,
                UnidadMedidaDescripcion = p.UnidadMedida?.GetDescripcion(lang) ?? string.Empty,
                ProveedorId = p.ProveedorId,
                ProveedorCodigo = p.Proveedor?.Codigo ?? string.Empty,
                ProveedorDescripcion = p.Proveedor?.GetDescripcion(lang) ?? string.Empty,
                PrecioVenta = p.PrecioVenta,
                PrecioCosto = p.PrecioCosto,
                Iva = p.Iva,
                CodigoReferencia = p.CodigoReferencia,
                StockMinimo = p.StockMinimo,
                Servicio = p.Servicio,
                SubLineaId = p.SubLineaId,
                SubLineaCodigo = p.SubLinea?.Codigo ?? string.Empty,
                SubLineaDescripcion = p.SubLinea?.GetDescripcion(lang) ?? string.Empty,
                LineaCodigo = p.SubLinea?.Linea?.Codigo ?? string.Empty,
                LineaDescripcion = p.SubLinea?.Linea?.GetDescripcion(lang) ?? string.Empty,
                Imagen = p.Imagen,
                Caracteristicas = p.GetCaracteristicas(lang),
                Cancelado = p.Cancelado,
                CreadoPor = p.CreadoPor,
                FechaHoraCreado = p.FechaHoraCreado,
                ModificadoPor = p.ModificadoPor,
                FechaHoraModificado = p.FechaHoraModificado
            };
        }
    }
}
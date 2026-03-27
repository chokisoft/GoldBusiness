using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GoldBusiness.Application.Helpers;
using GoldBusiness.Application.Interfaces;
using GoldBusiness.Domain.DTOs;
using GoldBusiness.Domain.Entities;
using GoldBusiness.Infrastructure.Repositories;
using Microsoft.Extensions.Localization;

namespace GoldBusiness.Application.Services
{
    public class SystemConfigurationService : ISystemConfigurationService
    {
        private readonly ISystemConfigurationRepository _repo;
        private readonly IPaisRepository _paisRepo;
        private readonly IStringLocalizer<GoldBusiness.Domain.Resources.ValidationMessages> _localizer;

        public SystemConfigurationService(
            ISystemConfigurationRepository repo,
            IPaisRepository paisRepo,
            IStringLocalizer<GoldBusiness.Domain.Resources.ValidationMessages> localizer)
        {
            _repo = repo;
            _paisRepo = paisRepo;
            _localizer = localizer;
        }

        public async Task<IEnumerable<SystemConfigurationDTO>> GetAllAsync(string lang = "es")
            => (await _repo.GetAllAsync())
                .Select(s => MapToDTO(s, lang))
                .Where(dto => dto is not null)
                .Select(dto => dto!)
                .ToList();

        public async Task<(IEnumerable<SystemConfigurationDTO> Items, int Total)> GetPagedAsync(int page, int pageSize, string? termino = null, string lang = "es")
        {
            var (items, total) = await _repo.GetPagedAsync(page, pageSize, termino);
            var dtos = items.Select(s => MapToDTO(s, lang))
                            .Where(dto => dto is not null)
                            .Select(dto => dto!)
                            .ToList();
            return (dtos, total);
        }

        public async Task<SystemConfigurationDTO?> GetByIdAsync(int id, string lang = "es")
            => MapToDTO(await _repo.GetByIdAsync(id), lang);

        public async Task<SystemConfigurationDTO> CreateAsync(SystemConfigurationDTO dto, string user, string lang = "es")
        {
            var creador = user ?? "system";

            // Usar el helper genérico
            var (existe, estaCancelado, existingEntity) = await CodigoValidationHelper
                .ValidateCodigoForCreateAsync(_repo, dto.CodigoSistema);

            if (existe)
            {
                if (estaCancelado && existingEntity != null)
                {
                    existingEntity.Reactivar(creador);
                    existingEntity.AddOrUpdateTranslation(lang, dto.NombreNegocio, dto.Direccion, dto.Municipio, dto.Provincia, creador);
                    await _repo.UpdateAsync(existingEntity);

                    return MapToDTO(existingEntity, lang);
                }
                else
                {
                    var errorMessage = CodigoValidationHelper.GetDuplicateCodeErrorMessage(
                        _localizer, dto.CodigoSistema, false);
                    throw new InvalidOperationException(errorMessage);
                }
            }

            // Obtener país si existe para validar teléfono
            Pais? pais = null;
            pais = await _paisRepo.GetByIdAsync(dto.PaisId);

            var entity = new SystemConfiguration(
                dto.CodigoSistema,
                dto.Licencia,
                dto.NombreNegocio,
                dto.Direccion,
                dto.PaisId,
                dto.ProvinciaId,
                dto.MunicipioId,
                dto.CodigoPostalId,
                dto.Imagen,
                dto.Web,
                dto.Email,
                dto.Telefono,
                dto.Caducidad,
                creador);

                await _repo.AddAsync(entity);

                entity.AddOrUpdateTranslation(
                lang,
                dto.NombreNegocio,
                dto.Direccion ?? string.Empty,
                string.Empty,
                string.Empty,
                creador);

            await _repo.AddAsync(entity);
            await _repo.UpdateAsync(entity);

            return MapToDTO(entity, lang)!;
        }

        public async Task<SystemConfigurationDTO> UpdateAsync(int id, SystemConfigurationDTO dto, string user, string lang = "es")
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null) throw new KeyNotFoundException($"SystemConfiguration con ID {id} no encontrada");

            // Normalizar y validar cambio de código para evitar duplicados
            var incomingCodigo = dto.CodigoSistema?.Trim() ?? string.Empty;
            var codigoUpper = incomingCodigo.ToUpperInvariant();

            if (!string.Equals(entity.CodigoSistema, incomingCodigo, StringComparison.OrdinalIgnoreCase))
            {
                var (existe, estaCancelado, existingId) = await CodigoValidationHelper
                    .ValidateCodigoForUpdateAsync(_repo, codigoUpper, id);

                if (existe)
                {
                    if (estaCancelado && existingId.HasValue)
                    {
                        var errorMessage = CodigoValidationHelper.GetDuplicateCodeErrorMessage(_localizer, dto.CodigoSistema, true, existingId);
                        throw new InvalidOperationException(errorMessage);
                    }
                    else
                    {
                        var errorMessage = string.Format(_localizer["CodigoDuplicado"].Value, dto.CodigoSistema);
                        throw new InvalidOperationException(errorMessage);
                    }
                }

                entity.SetCodigoSistema(codigoUpper);
            }

            // Obtener país si existe para validar teléfono
            Pais? pais = null;
            pais = await _paisRepo.GetByIdAsync(dto.PaisId);

            entity.SetLicencia(dto.Licencia);
            entity.SetNombreNegocio(dto.NombreNegocio);
            entity.SetDireccion(dto.Direccion ?? string.Empty);
            entity.SetProvincia(dto.ProvinciaId);
            entity.SetMunicipio(dto.MunicipioId);
            entity.SetCodigoPostal(dto.CodigoPostalId);
            entity.SetImagen(dto.Imagen ?? string.Empty);
            entity.SetWeb(dto.Web ?? string.Empty);
            entity.SetEmail(dto.Email ?? string.Empty);
            entity.SetTelefono(dto.Telefono ?? string.Empty);
            entity.SetCaducidad(dto.Caducidad);
            entity.SetCuentas(dto.CuentaPagarId, dto.CuentaCobrarId);

            entity.AddOrUpdateTranslation(
                lang,
                dto.NombreNegocio,
                dto.Direccion ?? string.Empty,
                string.Empty,
                string.Empty,
                user);

            // Manejar cambio de estado Activo desde el formulario (Cancelado no se envía desde form)
            if (dto.Activo == false && entity.Activo)
            {
                entity.Desactivar(user);
            }
            else if (dto.Activo == true && !entity.Activo)
            {
                entity.Activar(user);
            }
            else
            {
                // Si no hay cambio de estado, actualizar auditoría general
                entity.ActualizarAuditoria(user);
            }

            await _repo.UpdateAsync(entity);
            return MapToDTO(entity, lang)!;
        }

        public async Task<SystemConfigurationDTO?> SoftDeleteAsync(int id, string user, string lang = "es")
        {
            var usuario = user ?? "system";
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null) return null;

            entity.SoftDelete(usuario);
            await _repo.UpdateAsync(entity);
            return MapToDTO(entity, lang);
        }

        public async Task AddOrUpdateTranslationAsync(
            int id,
            string lang,
            string nombreNegocio,
            string? direccion,
            string? municipio,
            string? provincia,
            string user)
        {
            var modificador = user ?? "system";

            var entity = await _repo.GetByIdAsync(id);
            if (entity == null)
                throw new KeyNotFoundException($"SystemConfiguration con ID {id} no encontrada");

            entity.AddOrUpdateTranslation(
                lang,
                nombreNegocio,
                direccion ?? string.Empty,
                municipio ?? string.Empty,
                provincia ?? string.Empty,
                modificador);

            // CORRECCIÓN: llamar por posición (nombre del parámetro en BaseEntity es 'usuario')
            entity.ActualizarAuditoria(modificador);

            await _repo.UpdateAsync(entity);
        }

        /// <summary>
        /// Mapea la entidad SystemConfiguration a DTO
        /// </summary>
        private static SystemConfigurationDTO? MapToDTO(SystemConfiguration? s, string lang)
        {
            if (s == null) return null;

            return new SystemConfigurationDTO
            {
                Id = s.Id,
                CodigoSistema = s.CodigoSistema,
                Licencia = s.Licencia,
                NombreNegocio = s.GetNombreNegocio(lang),
                Direccion = s.GetDireccion(lang),

                // Mapear IDs
                PaisId = s.PaisId,
                ProvinciaId = s.ProvinciaId,
                MunicipioId = s.MunicipioId,
                CodigoPostalId = s.CodigoPostalId,

                // Propiedades de presentación (texto)
                Municipio = s.GetMunicipio(lang),
                Provincia = s.GetProvincia(lang),
                CodPostal = s.CodigoPostal?.Codigo ?? string.Empty,

                Imagen = s.Imagen,
                Web = s.Web,
                Email = s.Email,
                Telefono = s.Telefono,
                CuentaPagarId = s.CuentaPagarId,
                CuentaCobrarId = s.CuentaCobrarId,
                Caducidad = s.Caducidad,

                // Propiedades de navegación (nullable)
                CuentaPagarCodigo = s.CuentaPagar?.Codigo,
                CuentaPagarDescripcion = s.CuentaPagar?.GetDescripcion(lang),
                CuentaCobrarCodigo = s.CuentaCobrar?.Codigo,
                CuentaCobrarDescripcion = s.CuentaCobrar?.GetDescripcion(lang),

                // Estado
                Activo = s.Activo,
                Cancelado = s.Cancelado,

                // Auditoría
                CreadoPor = s.CreadoPor,
                FechaHoraCreado = s.FechaHoraCreado,
                ModificadoPor = s.ModificadoPor,
                FechaHoraModificado = s.FechaHoraModificado
            };
        }
    }
}
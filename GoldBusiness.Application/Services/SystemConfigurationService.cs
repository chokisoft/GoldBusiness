using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GoldBusiness.Application.Helpers;
using GoldBusiness.Application.Interfaces;
using GoldBusiness.Domain.DTOs;
using GoldBusiness.Domain.Entities;
using GoldBusiness.Domain.Enums;
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

            var (existe, estaCancelado, existingEntity) = await CodigoValidationHelper
                .ValidateCodigoForCreateAsync(_repo, dto.CodigoSistema);

            if (existe)
            {
                if (estaCancelado && existingEntity != null)
                {
                    existingEntity.Reactivar(creador);
                    existingEntity.AddOrUpdateTranslation(lang, dto.NombreNegocio, dto.Direccion ?? string.Empty, dto.Municipio ?? string.Empty, dto.Provincia ?? string.Empty, creador);
                    await _repo.UpdateAsync(existingEntity);
                    return MapToDTO(existingEntity, lang)!;
                }
                else
                {
                    var errorMessage = CodigoValidationHelper.GetDuplicateCodeErrorMessage(
                        _localizer, dto.CodigoSistema, false);
                    throw new InvalidOperationException(errorMessage);
                }
            }

            var entity = new SystemConfiguration(
                dto.CodigoSistema,
                dto.Licencia,
                dto.NombreNegocio,
                dto.PersonaContacto ?? string.Empty,
                dto.FormaJuridicaId ?? 0,
                dto.Direccion ?? string.Empty,
                dto.PaisId,
                dto.ProvinciaId,
                dto.MunicipioId,
                dto.CodigoPostalId,
                dto.Imagen,
                dto.Web,
                dto.Email,
                dto.Telefono ?? string.Empty,
                dto.Caducidad == default ? DateTime.UtcNow.AddYears(1) : dto.Caducidad, // ⭐ CORREGIDO
                dto.IdentificadorFiscal,
                dto.TipoIdentificadorFiscal ?? TipoIdentificacionFiscal.NIF,
                dto.RegimenFiscal ?? RegimenFiscal.General,
                dto.TasaIvaDefecto ?? 21m,
                dto.RegistradaIva ?? true,
                dto.IvaInternacional ?? false,
                creador
            );

            await _repo.AddAsync(entity);

            entity.AddOrUpdateTranslation(
                lang,
                dto.NombreNegocio,
                dto.Direccion ?? string.Empty,
                string.Empty,
                string.Empty,
                creador);

            await _repo.UpdateAsync(entity);
            return MapToDTO(entity, lang)!;
        }

        public async Task<SystemConfigurationDTO> UpdateAsync(int id, SystemConfigurationDTO dto, string user, string lang = "es")
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null) throw new KeyNotFoundException($"SystemConfiguration con ID {id} no encontrada");

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

            entity.SetLicencia(dto.Licencia);
            entity.SetNombreNegocio(dto.NombreNegocio);
            entity.SetPersonaContacto(dto.PersonaContacto ?? string.Empty);
            entity.SetFormaJuridica(dto.FormaJuridicaId ?? 0);
            entity.SetDireccion(dto.Direccion ?? string.Empty);
            entity.SetPais(dto.PaisId);
            entity.SetProvincia(dto.ProvinciaId);
            entity.SetMunicipio(dto.MunicipioId);
            entity.SetCodigoPostal(dto.CodigoPostalId);
            entity.SetImagen(dto.Imagen ?? string.Empty);
            entity.SetWeb(dto.Web ?? string.Empty);
            entity.SetEmail(dto.Email ?? string.Empty);
            entity.SetTelefono(dto.Telefono ?? string.Empty);
            entity.SetCaducidad(dto.Caducidad);
            
            // Campos fiscales
            entity.SetIdentificadorFiscal(dto.IdentificadorFiscal ?? string.Empty);
            entity.SetTipoIdentificadorFiscal(dto.TipoIdentificadorFiscal ?? TipoIdentificacionFiscal.NIF);
            entity.SetRegimenFiscal(dto.RegimenFiscal ?? RegimenFiscal.General);
            entity.SetTasaIvaDefecto(dto.TasaIvaDefecto ?? 21m);
            entity.SetRegistradaIva(dto.RegistradaIva ?? true);
            entity.SetIvaInternacional(dto.IvaInternacional ?? false);

            if (dto.CuentaPagarId.HasValue) entity.AsignarCuentaPagar(dto.CuentaPagarId.Value);
            if (dto.CuentaCobrarId.HasValue) entity.AsignarCuentaCobrar(dto.CuentaCobrarId.Value);

            entity.ActualizarAuditoria(user ?? "system");

            entity.AddOrUpdateTranslation(
                lang,
                dto.NombreNegocio,
                dto.Direccion ?? string.Empty,
                string.Empty,
                string.Empty,
                user ?? "system");

            await _repo.UpdateAsync(entity);
            return MapToDTO(entity, lang)!;
        }

        public async Task<SystemConfigurationDTO?> SoftDeleteAsync(int id, string user, string lang = "es")
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null) return null;

            entity.SoftDelete(user);
            await _repo.UpdateAsync(entity);
            return MapToDTO(entity, lang);
        }

        public async Task AddOrUpdateTranslationAsync(int id, string lang, string nombreNegocio, string direccion, string municipio, string provincia, string user)
        {
            if (string.IsNullOrWhiteSpace(lang)) lang = "es";
            if (string.IsNullOrWhiteSpace(nombreNegocio)) throw new ArgumentException("Nombre de negocio requerido.", nameof(nombreNegocio));

            var entity = await _repo.GetByIdAsync(id);
            if (entity == null) throw new KeyNotFoundException();

            entity.AddOrUpdateTranslation(lang, nombreNegocio, direccion ?? string.Empty, municipio ?? string.Empty, provincia ?? string.Empty, user ?? "system");
            await _repo.UpdateAsync(entity);
        }

        private static SystemConfigurationDTO? MapToDTO(SystemConfiguration? s, string lang)
        {
            if (s == null) return null;

            return new SystemConfigurationDTO
            {
                Id = s.Id,
                CodigoSistema = s.CodigoSistema,
                Licencia = s.Licencia,
                NombreNegocio = s.GetNombreNegocio(lang),
                PersonaContacto = s.PersonaContacto,
                FormaJuridicaId = s.FormaJuridicaId,
                Direccion = s.GetDireccion(lang),
                PaisId = s.PaisId,
                ProvinciaId = s.ProvinciaId,
                MunicipioId = s.MunicipioId,
                CodigoPostalId = s.CodigoPostalId,
                Municipio = s.GetMunicipio(lang),
                Provincia = s.GetProvincia(lang),
                CodPostal = s.CodigoPostal?.Codigo,
                Imagen = s.Imagen,
                Web = s.Web,
                Email = s.Email,
                Telefono = s.Telefono,
                CuentaPagarId = s.CuentaPagarId,
                CuentaCobrarId = s.CuentaCobrarId,
                
                // Campos fiscales
                IdentificadorFiscal = s.IdentificadorFiscal,
                TipoIdentificadorFiscal = s.TipoIdentificadorFiscal,
                RegimenFiscal = s.RegimenFiscal,
                TasaIvaDefecto = s.TasaIvaDefecto,
                RegistradaIva = s.RegistradaIva,
                IvaInternacional = s.IvaInternacional,
                
                Caducidad = s.Caducidad,
                CreadoPor = s.CreadoPor,
                FechaHoraCreado = s.FechaHoraCreado,
                ModificadoPor = s.ModificadoPor,
                FechaHoraModificado = s.FechaHoraModificado,
                CuentaPagarCodigo = s.CuentaPagar?.Codigo,
                CuentaPagarDescripcion = s.CuentaPagar?.GetDescripcion(lang),
                CuentaCobrarCodigo = s.CuentaCobrar?.Codigo,
                CuentaCobrarDescripcion = s.CuentaCobrar?.GetDescripcion(lang),
                Activo = s.Activo,
                Cancelado = s.Cancelado
            };
        }
    }
}
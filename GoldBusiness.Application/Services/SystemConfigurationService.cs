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
        private readonly IStringLocalizer<GoldBusiness.Domain.Resources.ValidationMessages> _localizer;

        public SystemConfigurationService(
            ISystemConfigurationRepository repo,
            IStringLocalizer<GoldBusiness.Domain.Resources.ValidationMessages> localizer)
        {
            _repo = repo;
            _localizer = localizer;
        }

        public async Task<IEnumerable<SystemConfigurationDTO>> GetAllAsync(string lang = "es")
            => (await _repo.GetAllAsync())
                .Select(s => MapToDTO(s, lang))
                .Where(dto => dto is not null)
                .Select(dto => dto!)
                .ToList();

        public async Task<SystemConfigurationDTO?> GetByIdAsync(int id, string lang = "es")
            => MapToDTO(await _repo.GetByIdAsync(id), lang);

        // ✅ CAMBIADO: Constructor sin cuentas, las establece después
        public async Task<SystemConfigurationDTO> CreateAsync(
            SystemConfigurationDTO dto,
            string user,
            string lang = "es")
        {
            var entity = new SystemConfiguration(

            dto.CodigoSistema,
            dto.Licencia,
            dto.NombreNegocio,
            dto.Direccion,
            dto.Municipio,
            dto.Provincia,
            dto.CodPostal,
            dto.Imagen,
            dto.Web,
            dto.Email,
            dto.Telefono,
            dto.Caducidad,
            user);

            // ✅ Establecer cuentas (pueden ser null)
            if (dto.CuentaPagarId.HasValue || dto.CuentaCobrarId.HasValue)
            {
                entity.SetCuentas(dto.CuentaPagarId, dto.CuentaCobrarId);
            }

            await _repo.AddAsync(entity);

            entity.AddOrUpdateTranslation(lang, dto.NombreNegocio, dto.Direccion ?? string.Empty, user);
            await _repo.UpdateAsync(entity);

            return MapToDTO(entity, lang)!;
        }

        // ✅ CAMBIADO: Manejo de nullable
        public async Task<SystemConfigurationDTO> UpdateAsync(
            int id,
            SystemConfigurationDTO dto,
            string user,
            string lang = "es")
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null) throw new KeyNotFoundException();

            entity.SetCodigoSistema(dto.CodigoSistema);
            entity.SetLicencia(dto.Licencia);
            entity.SetNombreNegocio(dto.NombreNegocio);
            entity.SetDireccion(dto.Direccion ?? string.Empty);
            entity.SetMunicipio(dto.Municipio ?? string.Empty);
            entity.SetProvincia(dto.Provincia ?? string.Empty);
            entity.SetCodPostal(dto.CodPostal ?? string.Empty);
            entity.SetImagen(dto.Imagen ?? string.Empty);
            entity.SetWeb(dto.Web ?? string.Empty);
            entity.SetEmail(dto.Email ?? string.Empty);
            entity.SetTelefono(dto.Telefono ?? string.Empty);
            entity.SetCaducidad(dto.Caducidad);

            // ✅ Manejo de nullable
            entity.SetCuentas(dto.CuentaPagarId, dto.CuentaCobrarId);

            entity.AddOrUpdateTranslation(lang, dto.NombreNegocio, dto.Direccion ?? string.Empty, user);

            entity.ActualizarAuditoria(user);

            await _repo.UpdateAsync(entity);
            return MapToDTO(entity, lang)!;
        }

        public async Task AddOrUpdateTranslationAsync(
            int id,
            string lang,
            string nombreNegocio,
            string? direccion,
            string user)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null) throw new KeyNotFoundException();

            entity.AddOrUpdateTranslation(lang, nombreNegocio, direccion ?? string.Empty, user);
            await _repo.UpdateAsync(entity);
        }

        // ✅ CAMBIADO: Manejo correcto de propiedades nullable
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
                Municipio = s.Municipio,
                Provincia = s.Provincia,
                CodPostal = s.CodPostal,
                Imagen = s.Imagen,
                Web = s.Web,
                Email = s.Email,
                Telefono = s.Telefono,
                CuentaPagarId = s.CuentaPagarId,
                CuentaCobrarId = s.CuentaCobrarId,
                Caducidad = s.Caducidad,
                CuentaPagarCodigo = s.CuentaPagarNavigation?.Codigo,
                CuentaPagarDescripcion = s.CuentaPagarNavigation?.GetDescripcion(lang),
                CuentaCobrarCodigo = s.CuentaCobrarNavigation?.Codigo,
                CuentaCobrarDescripcion = s.CuentaCobrarNavigation?.GetDescripcion(lang),
                CreadoPor = s.CreadoPor,
                FechaHoraCreado = s.FechaHoraCreado,
                ModificadoPor = s.ModificadoPor,
                FechaHoraModificado = s.FechaHoraModificado
            };
        }
    }
}
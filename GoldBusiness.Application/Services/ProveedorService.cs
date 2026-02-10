using GoldBusiness.Application.Helpers;
using GoldBusiness.Application.Interfaces;
using GoldBusiness.Domain.DTOs;
using GoldBusiness.Domain.Entities;
using GoldBusiness.Infrastructure.Repositories;
using Microsoft.Extensions.Localization;

namespace GoldBusiness.Application.Services
{
    public class ProveedorService : IProveedorService
    {
        private readonly IProveedorRepository _repo;
        private readonly IStringLocalizer<GoldBusiness.Domain.Resources.ValidationMessages> _localizer;

        public ProveedorService(IProveedorRepository repo,
            IStringLocalizer<GoldBusiness.Domain.Resources.ValidationMessages> localizer)
        {
            _repo = repo;
            _localizer = localizer;
        }

        public async Task<IEnumerable<ProveedorDTO>> GetAllAsync(string lang = "es")
            => (await _repo.GetAllAsync())
                .Select(g => MapToDTO(g, lang))
                .Where(dto => dto is not null)
                .Select(dto => dto!)
                .ToList();

        public async Task<ProveedorDTO?> GetByIdAsync(int id, string lang = "es")
            => MapToDTO(await _repo.GetByIdAsync(id), lang);

        public async Task<ProveedorDTO> CreateAsync(ProveedorDTO dto, string user, string lang = "es")
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
            var entity = new Proveedor(
                dto.Codigo,
                dto.Descripcion,
                dto.Nif,
                dto.Iban,
                dto.BicoSwift,
                dto.Iva,
                dto.Direccion,
                dto.Municipio,
                dto.Provincia,
                dto.CodPostal,
                dto.Web,
                dto.Email1,
                dto.Email2,
                dto.Telefono1,
                dto.Telefono2,
                dto.Fax1,
                dto.Fax2,
                creador);

            await _repo.AddAsync(entity);

            entity.AddOrUpdateTranslation(lang, dto.Descripcion, creador);
            await _repo.UpdateAsync(entity);

            return MapToDTO(entity, lang)!;
        }

        public async Task<ProveedorDTO> UpdateAsync(int id, ProveedorDTO dto, string user, string lang = "es")
        {
            var modificador = user ?? "system";

            var entity = await _repo.GetByIdAsync(id);
            if (entity == null)
                throw new KeyNotFoundException($"SystemConfiguration con ID {id} no encontrada");

            // Actualizar todas las propiedades
            entity.SetDescripcion(dto.Descripcion);
            entity.SetNif(dto.Nif ?? string.Empty);
            entity.SetIban(dto.Iban ?? string.Empty);
            entity.SetBicoSwift(dto.BicoSwift ?? string.Empty);
            entity.SetIva(dto.Iva);
            entity.SetDireccion(dto.Direccion ?? string.Empty);
            entity.SetMunicipio(dto.Municipio ?? string.Empty);
            entity.SetProvincia(dto.Provincia ?? string.Empty);
            entity.SetCodPostal(dto.CodPostal ?? string.Empty);
            entity.SetWeb(dto.Web ?? string.Empty);
            entity.SetEmails(dto.Email1 ?? string.Empty, dto.Email2 ?? string.Empty);
            entity.SetTelefonos(dto.Telefono1 ?? string.Empty, dto.Telefono2 ?? string.Empty);
            entity.SetFaxes(dto.Fax1 ?? string.Empty, dto.Fax2 ?? string.Empty);

            // Actualizar traducción
            entity.AddOrUpdateTranslation(lang, dto.Descripcion, modificador);

            // Actualizar auditoría
            entity.ActualizarAuditoria(modificador);

            await _repo.UpdateAsync(entity);
            return MapToDTO(entity, lang)!;
        }

        public async Task<ProveedorDTO?> SoftDeleteAsync(int id, string user)
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

        private static ProveedorDTO? MapToDTO(Proveedor? g, string lang)
        {
            if (g == null) return null;

            return new ProveedorDTO
            {
                Id = g.Id,
                Codigo = g.Codigo,
                Descripcion = g.GetDescripcion(lang),
                Nif = g.Nif,
                Iban = g.Iban,
                BicoSwift = g.BicoSwift,
                Iva = g.Iva,
                Direccion = g.Direccion,
                Municipio = g.Municipio,
                Provincia = g.Provincia,
                CodPostal = g.CodPostal,
                Web = g.Web,
                Email1 = g.Email1,
                Email2 = g.Email2,
                Telefono1 = g.Telefono1,
                Telefono2 = g.Telefono2,
                Fax1 = g.Fax1,
                Fax2 = g.Fax2,
                Cancelado = g.Cancelado,
                CreadoPor = g.CreadoPor,
                FechaHoraCreado = g.FechaHoraCreado,
                ModificadoPor = g.ModificadoPor,
                FechaHoraModificado = g.FechaHoraModificado
            };
        }
    }
}
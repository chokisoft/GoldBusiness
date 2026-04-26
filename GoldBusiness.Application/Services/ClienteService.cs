using GoldBusiness.Application.Helpers;
using GoldBusiness.Application.Interfaces;
using GoldBusiness.Domain.DTOs;
using GoldBusiness.Domain.Entities;
using GoldBusiness.Infrastructure.Repositories;
using Microsoft.Extensions.Localization;

namespace GoldBusiness.Application.Services
{
    public class ClienteService : IClienteService
    {
        private readonly IClienteRepository _repo;
        private readonly IPaisRepository _paisRepo;
        private readonly ITranslatorService _translatorService;
        private readonly IStringLocalizer<GoldBusiness.Domain.Resources.ValidationMessages> _localizer;

        public ClienteService(
            IClienteRepository repo,
            IPaisRepository paisRepo,
            ITranslatorService translatorService,
            IStringLocalizer<GoldBusiness.Domain.Resources.ValidationMessages> localizer)
        {
            _repo = repo;
            _paisRepo = paisRepo;
            _translatorService = translatorService;
            _localizer = localizer;
        }

        public async Task<IEnumerable<ClienteDTO>> GetAllAsync(string lang = "es")
            => (await _repo.GetAllAsync())
                .Select(g => MapToDTO(g, lang))
                .Where(dto => dto is not null)
                .Select(dto => dto!)
                .ToList();

        public async Task<(IEnumerable<ClienteDTO> Items, int Total)> GetPagedAsync(int page, int pageSize, string? termino = null, string lang = "es")
        {
            var (items, total) = await _repo.GetPagedAsync(page, pageSize, termino);
            var dtos = items.Select(l => MapToDTO(l, lang))
                            .Where(dto => dto is not null)
                            .Select(dto => dto!)
                            .ToList();
            return (dtos, total);
        }

        public async Task<ClienteDTO?> GetByIdAsync(int id, string lang = "es")
            => MapToDTO(await _repo.GetByIdAsync(id), lang);

        public async Task<ClienteDTO> CreateAsync(ClienteDTO dto, string user, string lang = "es")
        {
            var creador = user ?? "system";

            var (existe, estaCancelado, existingEntity) = await CodigoValidationHelper
                .ValidateCodigoForCreateAsync(_repo, dto.Codigo);

            if (existe)
            {
                if (estaCancelado && existingEntity != null)
                {
                    existingEntity.Reactivar(dto.Descripcion, creador);
                    existingEntity.Actualizar(
                        dto.Descripcion,
                        dto.IdentificadorFiscal,
                        dto.Iban,
                        dto.BicoSwift,
                        dto.TasaIva,
                        dto.Direccion,
                        dto.PaisId,
                        dto.ProvinciaId,
                        dto.MunicipioId,
                        dto.CodigoPostalId,
                        dto.Web,
                        dto.Email,
                        dto.Telefono,
                        dto.TipoIdentificadorFiscal,
                        dto.RegimenFiscal,
                        dto.ExentoIva,
                        dto.Extranjero,
                        dto.CodigoPaisIso,
                        dto.ValidarIdentificadorFiscal,
                        dto.InversionSujetoPasivo,
                        null,
                        creador
                    );

                    await GenerateAndSaveTranslationsAsync(existingEntity, dto.Descripcion, creador);
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

            var entity = new Cliente(
                dto.Codigo,
                dto.Descripcion,
                dto.IdentificadorFiscal,
                dto.Iban,
                dto.BicoSwift,
                dto.TasaIva,
                dto.Direccion,
                dto.PaisId,
                dto.ProvinciaId,
                dto.MunicipioId,
                dto.CodigoPostalId,
                dto.Web,
                dto.Email,
                dto.Telefono,
                dto.TipoIdentificadorFiscal,
                dto.RegimenFiscal,
                dto.ExentoIva,
                dto.Extranjero,
                dto.CodigoPaisIso,
                dto.ValidarIdentificadorFiscal,
                dto.InversionSujetoPasivo,
                creador
            );

            await _repo.AddAsync(entity);
            await GenerateAndSaveTranslationsAsync(entity, dto.Descripcion, creador);
            await _repo.UpdateAsync(entity);
            return MapToDTO(entity, lang)!;
        }

        public async Task<ClienteDTO> UpdateAsync(int id, ClienteDTO dto, string user, string lang = "es")
        {
            var modificador = user ?? "system";

            var entity = await _repo.GetByIdAsync(id);
            if (entity == null)
                throw new KeyNotFoundException($"Cliente con ID {id} no encontrado");

            entity.Actualizar(
                dto.Descripcion,
                dto.IdentificadorFiscal,
                dto.Iban,
                dto.BicoSwift,
                dto.TasaIva,
                dto.Direccion,
                dto.PaisId,
                dto.ProvinciaId,
                dto.MunicipioId,
                dto.CodigoPostalId,
                dto.Web,
                dto.Email,
                dto.Telefono,
                dto.TipoIdentificadorFiscal,
                dto.RegimenFiscal,
                dto.ExentoIva,
                dto.Extranjero,
                dto.CodigoPaisIso,
                dto.ValidarIdentificadorFiscal,
                dto.InversionSujetoPasivo,
                null,
                modificador
            );

            await GenerateAndSaveTranslationsAsync(entity, dto.Descripcion, modificador);
            await _repo.UpdateAsync(entity);
            return MapToDTO(entity, lang)!;
        }

        public async Task<ClienteDTO?> SoftDeleteAsync(int id, string user)
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

        private async Task GenerateAndSaveTranslationsAsync(Cliente entity, string descripcionBase, string user)
        {
            var supportedLanguages = new[] { "es", "en", "fr", "de", "pt" };
            entity.AddOrUpdateTranslation("es", descripcionBase, user);

            foreach (var targetLang in supportedLanguages)
            {
                if (targetLang == "es") continue;

                try
                {
                    var translation = await _translatorService.TranslateAsync(descripcionBase, "es", targetLang);
                    entity.AddOrUpdateTranslation(targetLang, string.IsNullOrWhiteSpace(translation) ? descripcionBase : translation, user);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"⚠️ Error generando traducción {targetLang} para Cliente {entity.Id}: {ex.Message}");
                    entity.AddOrUpdateTranslation(targetLang, descripcionBase, user);
                }
            }
        }

        private static ClienteDTO? MapToDTO(Cliente? g, string lang)
        {
            if (g == null) return null;

            return new ClienteDTO
            {
                Id = g.Id,
                Codigo = g.Codigo,
                Descripcion = g.GetDescripcion(lang),
                IdentificadorFiscal = g.IdentificadorFiscal,
                Iban = g.Iban,
                BicoSwift = g.BicoSwift,
                TasaIva = g.TasaIva,
                Direccion = g.Direccion,
                PaisId = g.PaisId,
                PaisDescripcion = g.Pais?.GetDescripcion(lang),
                ProvinciaId = g.ProvinciaId,
                ProvinciaDescripcion = g.Provincia?.GetDescripcion(lang),
                MunicipioId = g.MunicipioId,
                MunicipioDescripcion = g.Municipio?.GetDescripcion(lang),
                CodigoPostalId = g.CodigoPostalId,
                CodigoPostalCodigo = g.CodigoPostal?.Codigo,
                Web = g.Web,
                Email = g.Email,
                Telefono = g.Telefono,
                TipoIdentificadorFiscal = g.TipoIdentificadorFiscal,
                RegimenFiscal = g.RegimenFiscal,
                ExentoIva = g.ExentoIva,
                Extranjero = g.Extranjero,
                CodigoPaisIso = g.CodigoPaisIso,
                ValidarIdentificadorFiscal = g.ValidarIdentificadorFiscal,
                InversionSujetoPasivo = g.InversionSujetoPasivo,
                Cancelado = g.Cancelado,
                CreadoPor = g.CreadoPor,
                FechaHoraCreado = g.FechaHoraCreado,
                ModificadoPor = g.ModificadoPor,
                FechaHoraModificado = g.FechaHoraModificado
            };
        }
    }
}
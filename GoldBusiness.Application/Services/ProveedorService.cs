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
        private readonly IPaisRepository _paisRepo;
        private readonly ITranslatorService _translatorService;
        private readonly IStringLocalizer<GoldBusiness.Domain.Resources.ValidationMessages> _localizer;

        public ProveedorService(
            IProveedorRepository repo,
            IPaisRepository paisRepo,
            ITranslatorService translatorService,
            IStringLocalizer<GoldBusiness.Domain.Resources.ValidationMessages> localizer)
        {
            _repo = repo;
            _paisRepo = paisRepo;
            _translatorService = translatorService;
            _localizer = localizer;
        }

        public async Task<IEnumerable<ProveedorDTO>> GetAllAsync(string lang = "es")
            => (await _repo.GetAllAsync())
                .Select(g => MapToDTO(g, lang))
                .Where(dto => dto is not null)
                .Select(dto => dto!)
                .ToList();

        public async Task<(IEnumerable<ProveedorDTO> Items, int Total)> GetPagedAsync(int page, int pageSize, string? termino = null, string lang = "es")
        {
            var (items, total) = await _repo.GetPagedAsync(page, pageSize, termino);
            var dtos = items.Select(l => MapToDTO(l, lang))
                            .Where(dto => dto is not null)
                            .Select(dto => dto!)
                            .ToList();
            return (dtos, total);
        }

        public async Task<ProveedorDTO?> GetByIdAsync(int id, string lang = "es")
            => MapToDTO(await _repo.GetByIdAsync(id), lang);

        public async Task<ProveedorDTO> CreateAsync(ProveedorDTO dto, string user, string lang = "es")
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
                        dto.Descripcion,                        // 1
                        dto.Nif,                                // 2
                        dto.Iban,                               // 3
                        dto.BicoSwift,                          // 4
                        dto.Iva,                                // 5
                        dto.Direccion,                          // 6
                        dto.PaisId,                             // 7
                        dto.ProvinciaId,                        // 8
                        dto.MunicipioId,                        // 9
                        dto.CodigoPostalId,                     // 10
                        dto.Web,                                // 11
                        dto.Email1,                             // 12
                        dto.Email2,                             // 13
                        dto.Telefono1,                          // 14
                        dto.Telefono2,                          // 15
                        dto.Fax1,                               // 16
                        dto.Fax2,                               // 17
                        null,                                   // 18 - Pais
                        dto.TipoIdentificadorFiscal,            // 19
                        dto.RegimenFiscal,                      // 20
                        dto.ExentoIva,                          // 21
                        dto.Extranjero,                         // 22
                        dto.CodigoPaisIso,                      // 23
                        dto.ValidarIdentificadorFiscal,         // 24
                        dto.InversionSujetoPasivo,              // 25
                        creador                                 // 26
                    );

                    // ✅ Generar traducciones automáticas
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

            // ✅ Constructor correcto con 19 argumentos
            var entity = new Proveedor(
                dto.Codigo,                             // 1
                dto.Descripcion,                        // 2
                dto.Nif,                                // 3
                dto.Iban,                               // 4
                dto.BicoSwift,                          // 5
                dto.Iva,                                // 6
                dto.Direccion,                          // 7
                dto.PaisId,                             // 8
                dto.ProvinciaId,                        // 9
                dto.MunicipioId,                        // 10
                dto.CodigoPostalId,                     // 11
                dto.Web,                                // 12
                dto.Email1,                             // 13
                dto.Email2,                             // 14
                dto.Telefono1,                          // 15
                dto.Telefono2,                          // 16
                dto.Fax1,                               // 17
                dto.Fax2,                               // 18
                dto.TipoIdentificadorFiscal,            // 19
                dto.RegimenFiscal,                      // 20
                dto.ExentoIva,                          // 21
                dto.Extranjero,                         // 22
                dto.CodigoPaisIso,                      // 23
                dto.ValidarIdentificadorFiscal,         // 24
                dto.InversionSujetoPasivo,              // 25
                creador                                 // 26
            );

            await _repo.AddAsync(entity);

            // ✅ Generar traducciones automáticas a es/en/fr
            await GenerateAndSaveTranslationsAsync(entity, dto.Descripcion, creador);

            await _repo.UpdateAsync(entity);
            return MapToDTO(entity, lang)!;
        }

        public async Task<ProveedorDTO> UpdateAsync(int id, ProveedorDTO dto, string user, string lang = "es")
        {
            var modificador = user ?? "system";

            var entity = await _repo.GetByIdAsync(id);
            if (entity == null)
                throw new KeyNotFoundException($"Proveedor con ID {id} no encontrado");

            entity.Actualizar(
                dto.Descripcion,                        // 1
                dto.Nif,                                // 2
                dto.Iban,                               // 3
                dto.BicoSwift,                          // 4
                dto.Iva,                                // 5
                dto.Direccion,                          // 6
                dto.PaisId,                             // 7
                dto.ProvinciaId,                        // 8
                dto.MunicipioId,                        // 9
                dto.CodigoPostalId,                     // 10
                dto.Web,                                // 11
                dto.Email1,                             // 12
                dto.Email2,                             // 13
                dto.Telefono1,                          // 14
                dto.Telefono2,                          // 15
                dto.Fax1,                               // 16
                dto.Fax2,                               // 17
                null,                                   // 18 - Pais
                dto.TipoIdentificadorFiscal,            // 19
                dto.RegimenFiscal,                      // 20
                dto.ExentoIva,                          // 21
                dto.Extranjero,                         // 22
                dto.CodigoPaisIso,                      // 23
                dto.ValidarIdentificadorFiscal,         // 24
                dto.InversionSujetoPasivo,              // 25
                modificador                             // 26
            );

            // ✅ Generar traducciones automáticas a es/en/fr
            await GenerateAndSaveTranslationsAsync(entity, dto.Descripcion, modificador);

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

        // ✅ CORREGIDO: Usar TranslateAsync en lugar de TranslateToAllAsync
        private async Task GenerateAndSaveTranslationsAsync(Proveedor entity, string descripcionBase, string user)
        {
            try
            {
                // Traducir a inglés
                var enTranslation = await _translatorService.TranslateAsync(descripcionBase, "es", "en");
                
                // Traducir a francés
                var frTranslation = await _translatorService.TranslateAsync(descripcionBase, "es", "fr");

                entity.AddOrUpdateTranslation("es", descripcionBase, user);
                entity.AddOrUpdateTranslation("en", enTranslation, user);
                entity.AddOrUpdateTranslation("fr", frTranslation, user);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"⚠️ Error generando traducciones para Proveedor {entity.Id}: {ex.Message}");
                // Fallback: usar la descripción base para todos los idiomas
                entity.AddOrUpdateTranslation("es", descripcionBase, user);
                entity.AddOrUpdateTranslation("en", descripcionBase, user);
                entity.AddOrUpdateTranslation("fr", descripcionBase, user);
            }
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
                PaisId = g.PaisId,
                PaisDescripcion = g.Pais?.GetDescripcion(lang),
                ProvinciaId = g.ProvinciaId,
                ProvinciaDescripcion = g.Provincia?.GetDescripcion(lang),
                MunicipioId = g.MunicipioId,
                MunicipioDescripcion = g.Municipio?.GetDescripcion(lang),
                CodigoPostalId = g.CodigoPostalId,
                CodigoPostalCodigo = g.CodigoPostal?.Codigo,
                Web = g.Web,
                Email1 = g.Email1,
                Email2 = g.Email2,
                Telefono1 = g.Telefono1,
                Telefono2 = g.Telefono2,
                Fax1 = g.Fax1,
                Fax2 = g.Fax2,
                
                // Campos fiscales
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
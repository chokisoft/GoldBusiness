using GoldBusiness.Application.Helpers;
using GoldBusiness.Application.Interfaces;
using GoldBusiness.Domain.DTOs;
using GoldBusiness.Domain.Entities;
using GoldBusiness.Infrastructure.Repositories;
using Microsoft.Extensions.Localization;
using GoldBusiness.Domain.Helpers;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace GoldBusiness.Application.Services
{
    public class GrupoCuentaService : IGrupoCuentaService
    {
        private readonly IGrupoCuentaRepository _repo;
        private readonly IStringLocalizer<GoldBusiness.Domain.Resources.ValidationMessages> _localizer;
        private readonly ITranslatorService _translatorService;
        private readonly ILogger<GrupoCuentaService> _logger;

        public GrupoCuentaService(
            IGrupoCuentaRepository repo,
            IStringLocalizer<GoldBusiness.Domain.Resources.ValidationMessages> localizer,
            ITranslatorService translatorService,
            ILogger<GrupoCuentaService> logger)
        {
            _repo = repo;
            _localizer = localizer;
            _translatorService = translatorService;
            _logger = logger;
        }

        public async Task<IEnumerable<GrupoCuentaDTO>> GetAllAsync(string lang = "es")
            => (await _repo.GetAllAsync())
                .Select(g => MapToDTO(g, lang))
                .Where(dto => dto is not null)
                .Select(dto => dto!)
                .ToList();

        public async Task<(IEnumerable<GrupoCuentaDTO> Items, int Total)> GetPagedAsync(int page, int pageSize, string? termino = null, string lang = "es")
        {
            var (items, total) = await _repo.GetPagedAsync(page, pageSize, termino);
            var dtos = items.Select(cp => MapToDTO(cp, lang)).ToList();
            return (dtos, total);
        }

        public async Task<GrupoCuentaDTO?> GetByIdAsync(int id, string lang = "es")
            => MapToDTO(await _repo.GetByIdAsync(id), lang);

        public async Task<GrupoCuentaDTO> CreateAsync(GrupoCuentaDTO dto, string user, string lang = "es")
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

            // Preparar traducciones (es, en, fr, de, pt)
            var supportedLanguages = new[] { "es", "en", "fr", "de", "pt" };
            var provided = dto.Translations ?? new List<TranslationInputDTO>();
            if (!provided.Any())
            {
                provided.Add(new TranslationInputDTO { Language = lang, TranslatedText = dto.Descripcion });
            }

            var map = provided
                .Where(t => !string.IsNullOrWhiteSpace(t.Language) && !string.IsNullOrWhiteSpace(t.TranslatedText))
                .ToDictionary(
                    t => t.Language.Split('-', StringSplitOptions.RemoveEmptyEntries)[0].ToLowerInvariant(),
                    t => t.TranslatedText,
                    StringComparer.OrdinalIgnoreCase);

            if (!map.ContainsKey(lang))
            {
                map[lang] = dto.Descripcion;
            }

            var normalizedLang = LanguageHelper.NormalizeLang(lang);
            var sourceLang = map.ContainsKey(normalizedLang) ? normalizedLang : map.Keys.First();
            var sourceText = map[sourceLang];

            foreach (var target in supportedLanguages)
            {
                if (!map.ContainsKey(target))
                {
                    try
                    {
                        var translated = await _translatorService.TranslateAsync(sourceText, sourceLang, target);
                        map[target] = string.IsNullOrWhiteSpace(translated) ? sourceText : translated;
                    }
                    catch
                    {
                        map[target] = sourceText;
                    }
                }
            }

            // LOG: contenido del map antes de persistir
            _logger.LogInformation("GrupoCuenta Create: codigo={Codigo}, source={SourceLang}, sourceText={SourceText}", dto.Codigo, sourceLang, sourceText);
            _logger.LogInformation("Translations map: {Map}", string.Join("; ", map.Select(kv => $"{kv.Key}={kv.Value}")));

            var entity = new GrupoCuenta(dto.Codigo, map[lang], creador);
            await _repo.AddAsync(entity);

            foreach (var kv in map)
            {
                // LOG antes de persistir cada traducción
                _logger.LogInformation("Persisting translation (Create) -> GrupoCuentaId={GrupoId}, Lang={Lang}, Text={Text}", entity.Id, kv.Key, kv.Value);
                entity.AddOrUpdateTranslation(kv.Key, kv.Value, creador);
            }

            await _repo.UpdateAsync(entity);
            return MapToDTO(entity, lang)!;
        }

        public async Task<GrupoCuentaDTO> UpdateAsync(int id, GrupoCuentaDTO dto, string user, string lang = "es")
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

            entity.Update(dto.Descripcion, user);

            // ? Preparar traducciones automáticas (es, en, fr, de, pt)
            var supportedLanguages = new[] { "es", "en", "fr", "de", "pt" };
            var provided = dto.Translations ?? new List<TranslationInputDTO>();
            if (!provided.Any())
            {
                provided.Add(new TranslationInputDTO { Language = lang, TranslatedText = dto.Descripcion });
            }

            var map = provided
                .Where(t => !string.IsNullOrWhiteSpace(t.Language) && !string.IsNullOrWhiteSpace(t.TranslatedText))
                .ToDictionary(
                    t => t.Language.Split('-', StringSplitOptions.RemoveEmptyEntries)[0].ToLowerInvariant(),
                    t => t.TranslatedText,
                    StringComparer.OrdinalIgnoreCase);

            if (!map.ContainsKey(lang))
            {
                map[lang] = dto.Descripcion;
            }

            var normalizedLang2 = LanguageHelper.NormalizeLang(lang);
            var sourceLang2 = map.ContainsKey(normalizedLang2) ? normalizedLang2 : map.Keys.First();
            var sourceText2 = map[sourceLang2];

            foreach (var target in supportedLanguages)
            {
                if (!map.ContainsKey(target))
                {
                    try
                    {
                        var translated = await _translatorService.TranslateAsync(sourceText2, sourceLang2, target);
                        map[target] = string.IsNullOrWhiteSpace(translated) ? sourceText2 : translated;
                    }
                    catch
                    {
                        map[target] = sourceText2;
                    }
                }
            }

            // LOG: contenido del map antes de persistir (update)
            _logger.LogInformation("GrupoCuenta Update: id={Id}, codigo={Codigo}, source={SourceLang}", id, dto.Codigo, sourceLang2);
            _logger.LogInformation("Translations map: {Map}", string.Join("; ", map.Select(kv => $"{kv.Key}={kv.Value}")));

            foreach (var kv in map)
            {
                // LOG antes de persistir cada traducción
                _logger.LogInformation("Persisting translation (Update) -> GrupoCuentaId={GrupoId}, Lang={Lang}, Text={Text}", entity.Id, kv.Key, kv.Value);
                entity.AddOrUpdateTranslation(kv.Key, kv.Value, user ?? "system");
            }

            await _repo.UpdateAsync(entity);
            return MapToDTO(entity, lang)!;
        }

        public async Task<GrupoCuentaDTO?> SoftDeleteAsync(int id, string user)
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

        private static GrupoCuentaDTO? MapToDTO(GrupoCuenta? g, string lang)
        {
            if (g == null) return null;

            return new GrupoCuentaDTO
            {
                Id = g.Id,
                Codigo = g.Codigo,
                Descripcion = g.GetDescripcion(lang),
                Cancelado = g.Cancelado,
                CreadoPor = g.CreadoPor,
                FechaHoraCreado = g.FechaHoraCreado,
                ModificadoPor = g.ModificadoPor,
                FechaHoraModificado = g.FechaHoraModificado,
                CantidadSubGrupos = g.SubGrupoCuenta?.Count ?? 0
            };
        }
    }
}
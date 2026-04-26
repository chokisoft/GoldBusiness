using GoldBusiness.Application.Helpers;
using GoldBusiness.Application.Interfaces;
using GoldBusiness.Domain.DTOs;
using GoldBusiness.Domain.Entities;
using GoldBusiness.Domain.Helpers;
using GoldBusiness.Infrastructure.Repositories;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

namespace GoldBusiness.Application.Services
{
    public class SubGrupoCuentaService : ISubGrupoCuentaService
    {
        private readonly ISubGrupoCuentaRepository _repo;
        private readonly IStringLocalizer<GoldBusiness.Domain.Resources.ValidationMessages> _localizer;
        private readonly ITranslatorService _translatorService;
        private readonly ILogger<SubGrupoCuentaService> _logger;

        public SubGrupoCuentaService(
            ISubGrupoCuentaRepository repo,
            IStringLocalizer<GoldBusiness.Domain.Resources.ValidationMessages> localizer,
            ITranslatorService translatorService,
            ILogger<SubGrupoCuentaService> logger)
        {
            _repo = repo;
            _localizer = localizer;
            _translatorService = translatorService;
            _logger = logger;
        }

        public async Task<IEnumerable<SubGrupoCuentaDTO>> GetAllAsync(string lang = "es")
            => (await _repo.GetAllAsync())
                .Select(s => MapToDTO(s, lang))
                .Where(dto => dto is not null)
                .Select(dto => dto!)
                .ToList();

        public async Task<(IEnumerable<SubGrupoCuentaDTO> Items, int Total)> GetPagedAsync(int page, int pageSize, string? termino = null, int? grupoCuentaId = null, string lang = "es")
        {
            var (items, total) = await _repo.GetPagedAsync(page, pageSize, termino, grupoCuentaId);
            var dtos = items.Select(s => MapToDTO(s, lang)).ToList();
            return (dtos, total);
        }

        public async Task<SubGrupoCuentaDTO?> GetByIdAsync(int id, string lang = "es")
            => MapToDTO(await _repo.GetByIdAsync(id), lang);

        public async Task<SubGrupoCuentaDTO> CreateAsync(SubGrupoCuentaDTO dto, string user, string lang = "es")
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

            // ✅ Preparar traducciones automáticas (es, en, fr, de, pt)
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

            _logger.LogInformation("SubGrupoCuenta Create: codigo={Codigo}, source={SourceLang}, sourceText={SourceText}", dto.Codigo, sourceLang, sourceText);
            _logger.LogInformation("Translations map: {Map}", string.Join("; ", map.Select(kv => $"{kv.Key}={kv.Value}")));

            var entity = new SubGrupoCuenta(dto.Codigo, map[lang], dto.GrupoCuentaId, dto.Deudora, creador);
            await _repo.AddAsync(entity);

            foreach (var kv in map)
            {
                _logger.LogInformation("Persisting translation (Create) -> SubGrupoCuentaId={SubId}, Lang={Lang}, Text={Text}", entity.Id, kv.Key, kv.Value);
                entity.AddOrUpdateTranslation(kv.Key, kv.Value, creador);
            }

            await _repo.UpdateAsync(entity);
            return MapToDTO(entity, lang)!;
        }

        public async Task<SubGrupoCuentaDTO> UpdateAsync(int id, SubGrupoCuentaDTO dto, string user, string lang = "es")
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

            entity.Update(dto.Descripcion, dto.GrupoCuentaId, dto.Deudora, user);

            // ✅ Preparar traducciones automáticas (es, en, fr, de, pt)
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

            _logger.LogInformation("SubGrupoCuenta Update: id={Id}, codigo={Codigo}, source={SourceLang}", id, dto.Codigo, sourceLang);
            _logger.LogInformation("Translations map: {Map}", string.Join("; ", map.Select(kv => $"{kv.Key}={kv.Value}")));

            foreach (var kv in map)
            {
                _logger.LogInformation("Persisting translation (Update) -> SubGrupoCuentaId={SubId}, Lang={Lang}, Text={Text}", id, kv.Key, kv.Value);
                entity.AddOrUpdateTranslation(kv.Key, kv.Value, user ?? "system");
            }

            await _repo.UpdateAsync(entity);
            return MapToDTO(entity, lang)!;
        }

        public async Task<SubGrupoCuentaDTO?> SoftDeleteAsync(int id, string user)
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

        private static SubGrupoCuentaDTO? MapToDTO(SubGrupoCuenta? s, string lang)
        {
            if (s == null) return null;

            return new SubGrupoCuentaDTO
            {
                Id = s.Id,
                Codigo = s.Codigo,
                GrupoCuentaId = s.GrupoCuentaId,
                GrupoCuentaCodigo = s.GrupoCuenta?.Codigo ?? string.Empty,
                GrupoCuentaDescripcion = s.GrupoCuenta?.GetDescripcion(lang) ?? string.Empty,
                Descripcion = s.GetDescripcion(lang),
                Deudora = s.Deudora,
                Cancelado = s.Cancelado,
                CreadoPor = s.CreadoPor,
                FechaHoraCreado = s.FechaHoraCreado,
                ModificadoPor = s.ModificadoPor,
                FechaHoraModificado = s.FechaHoraModificado
            };
        }
    }
}
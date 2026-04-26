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
    public class CuentaService : ICuentaService
    {
        private readonly ICuentaRepository _repo;
        private readonly IStringLocalizer<GoldBusiness.Domain.Resources.ValidationMessages> _localizer;
        private readonly ITranslatorService _translatorService;
        private readonly ILogger<CuentaService> _logger;

        public CuentaService(
            ICuentaRepository repo,
            IStringLocalizer<GoldBusiness.Domain.Resources.ValidationMessages> localizer,
            ITranslatorService translatorService,
            ILogger<CuentaService> logger)
        {
            _repo = repo;
            _localizer = localizer;
            _translatorService = translatorService;
            _logger = logger;
        }

        public async Task<IEnumerable<CuentaDTO>> GetAllAsync(string lang = "es", IReadOnlyCollection<string>? accessLevels = null)
            => (await _repo.GetAllAsync(accessLevels))
                .Select(c => MapToDTO(c, lang))
                .Where(dto => dto is not null)
                .Select(dto => dto!)
                .ToList();

        public async Task<(IEnumerable<CuentaDTO> Items, int Total)> GetPagedAsync(int page, int pageSize, string? termino = null, int? subGrupoCuentaId = null, string lang = "es", IReadOnlyCollection<string>? accessLevels = null)
        {
            var (items, total) = await _repo.GetPagedAsync(page, pageSize, termino, subGrupoCuentaId, accessLevels);
            var dtos = items.Select(c => MapToDTO(c, lang))
                            .Where(dto => dto is not null)
                            .Select(dto => dto!)
                            .ToList();
            return (dtos, total);
        }

        public async Task<CuentaDTO?> GetByIdAsync(int id, string lang = "es", IReadOnlyCollection<string>? accessLevels = null)
            => MapToDTO(await _repo.GetByIdAsync(id, accessLevels), lang);

        public async Task<CuentaDTO> CreateAsync(CuentaDTO dto, string user, string lang = "es", IReadOnlyCollection<string>? accessLevels = null)
        {
            var creador = user ?? "system";

            if (!HasAccessToCodigo(dto.Codigo, accessLevels))
            {
                throw new UnauthorizedAccessException("No tiene permisos para crear registros con este código.");
            }

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

            _logger.LogInformation("Cuenta Create: codigo={Codigo}, source={SourceLang}, sourceText={SourceText}", dto.Codigo, sourceLang, sourceText);
            _logger.LogInformation("Translations map: {Map}", string.Join("; ", map.Select(kv => $"{kv.Key}={kv.Value}")));

            var entity = new Cuenta(dto.Codigo, map[lang], dto.SystemConfigurationId, dto.SubGrupoCuentaId, creador);
            await _repo.AddAsync(entity);

            foreach (var kv in map)
            {
                _logger.LogInformation("Persisting translation (Create) -> CuentaId={CuentaId}, Lang={Lang}, Text={Text}", entity.Id, kv.Key, kv.Value);
                entity.AddOrUpdateTranslation(kv.Key, kv.Value, creador);
            }

            await _repo.UpdateAsync(entity);
            return MapToDTO(entity, lang)!;
        }

        public async Task<CuentaDTO> UpdateAsync(int id, CuentaDTO dto, string user, string lang = "es", IReadOnlyCollection<string>? accessLevels = null)
        {
            var entity = await _repo.GetByIdAsync(id, accessLevels);
            if (entity == null) throw new KeyNotFoundException();

            if (!HasAccessToCodigo(dto.Codigo, accessLevels))
            {
                throw new UnauthorizedAccessException("No tiene permisos para actualizar registros con este código.");
            }

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

            entity.Update(dto.Descripcion, dto.SystemConfigurationId, dto.SubGrupoCuentaId, user);

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

            _logger.LogInformation("Cuenta Update: id={Id}, codigo={Codigo}, source={SourceLang}", id, dto.Codigo, sourceLang);
            _logger.LogInformation("Translations map: {Map}", string.Join("; ", map.Select(kv => $"{kv.Key}={kv.Value}")));

            foreach (var kv in map)
            {
                _logger.LogInformation("Persisting translation (Update) -> CuentaId={CuentaId}, Lang={Lang}, Text={Text}", id, kv.Key, kv.Value);
                entity.AddOrUpdateTranslation(kv.Key, kv.Value, user ?? "system");
            }

            await _repo.UpdateAsync(entity);
            return MapToDTO(entity, lang)!;
        }

        public async Task<CuentaDTO?> SoftDeleteAsync(int id, string user, IReadOnlyCollection<string>? accessLevels = null)
        {
            var entity = await _repo.GetByIdAsync(id, accessLevels);
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

        private static CuentaDTO? MapToDTO(Cuenta? c, string lang)
        {
            if (c == null) return null;

            return new CuentaDTO
            {
                Id = c.Id,
                Codigo = c.Codigo,
                Descripcion = c.GetDescripcion(lang),
                SystemConfigurationId = c.SystemConfigurationId,
                SubGrupoCuentaId = c.SubGrupoCuentaId,
                SubGrupoCuentaCodigo = c.SubGrupoCuenta?.Codigo ?? string.Empty,
                SubGrupoCuentaDescripcion = c.SubGrupoCuenta?.GetDescripcion(lang) ?? string.Empty,
                GrupoCuentaCodigo = c.SubGrupoCuenta?.GrupoCuenta?.Codigo ?? string.Empty,
                GrupoCuentaDescripcion = c.SubGrupoCuenta?.GrupoCuenta?.GetDescripcion(lang) ?? string.Empty,
                Cancelado = c.Cancelado,
                CreadoPor = c.CreadoPor,
                FechaHoraCreado = c.FechaHoraCreado,
                ModificadoPor = c.ModificadoPor,
                FechaHoraModificado = c.FechaHoraModificado
            };
        }

        private static bool HasAccessToCodigo(string codigo, IReadOnlyCollection<string>? accessLevels)
        {
            if (accessLevels == null || accessLevels.Count == 0)
            {
                return false;
            }

            var normalizedCode = (codigo ?? string.Empty).Trim().ToUpperInvariant();
            var normalizedAccess = accessLevels
                .Select(x => x?.Trim().ToUpperInvariant())
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Distinct()
                .ToList();

            if (normalizedAccess.Contains("*"))
            {
                return true;
            }

            return normalizedAccess.Any(prefix => normalizedCode.StartsWith(prefix!));
        }
    }
}
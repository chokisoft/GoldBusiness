using GoldBusiness.Application.Interfaces;
using GoldBusiness.Domain.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.Globalization;

namespace GoldBusiness.WebApi.Controllers
{
    /// <summary>
    /// Controlador para gestión de Concepto Ajuste.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Policy = "ERPFullAccess")]
    public class ConceptoAjusteController : ControllerBase
    {
        private readonly IConceptoAjusteService _service;
        private readonly IStringLocalizer<GoldBusiness.Domain.Resources.ValidationMessages> _localizer;

        public ConceptoAjusteController(
            IConceptoAjusteService service,
            IStringLocalizer<GoldBusiness.Domain.Resources.ValidationMessages> localizer)
        {
            _service = service;
            _localizer = localizer;
        }

        /// <summary>
        /// Obtiene todos los concepto de ajuste.
        /// El idioma se detecta automáticamente del header Accept-Language.
        /// </summary>
        /// <returns>Lista de los concepto de ajuste localizados</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ConceptoAjusteDTO>>> Get()
        {
            var lang = GetCurrentLanguage();
            return Ok(await _service.GetAllAsync(lang));
        }

        /// <summary>
        /// Obtiene un concepto de ajuste por ID.
        /// El idioma se detecta automáticamente del header Accept-Language.
        /// </summary>
        /// <param name="id">ID del concepto de ajuste</param>
        /// <returns>Moneda localizado</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<ConceptoAjusteDTO>> Get(int id)
        {
            var lang = GetCurrentLanguage();
            var dto = await _service.GetByIdAsync(id, lang);
            return dto == null ? NotFound() : Ok(dto);
        }

        /// <summary>
        /// Crea una nuevo concepto de ajuste.
        /// El idioma se detecta automáticamente del header Accept-Language.
        /// </summary>
        /// <param name="dto">Datos de la nuevo concepto de ajuste</param>
        /// <returns>Concepto Ajuste creado</returns>
        [HttpPost]
        public async Task<ActionResult<ConceptoAjusteDTO>> Post([FromBody] ConceptoAjusteDTO dto)
        {
            var lang = GetCurrentLanguage();
            var usuario = User?.Identity?.Name ?? "system";
            var result = await _service.CreateAsync(dto, usuario, lang);
            return CreatedAtAction(nameof(Get), new { id = result.Id }, result);
        }

        /// <summary>
        /// Actualiza un concepto ajuste existente.
        /// El idioma se detecta automáticamente del header Accept-Language.
        /// </summary>
        /// <param name="id">ID del concepto ajuste a actualizar</param>
        /// <param name="dto">Datos actualizados del concepto ajuste</param>
        /// <returns>Concepto ajuste actualizado</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] ConceptoAjusteDTO dto)
        {
            var lang = GetCurrentLanguage();
            var usuario = User?.Identity?.Name ?? "system";
            var result = await _service.UpdateAsync(id, dto, usuario, lang);
            return Ok(result);
        }

        /// <summary>
        /// Elimina (soft delete) un concepto ajuste.
        /// </summary>
        /// <param name="id">ID de la moneda a eliminar</param>
        /// <returns>Moneda eliminada</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var usuario = User?.Identity?.Name ?? "system";
            var dto = await _service.SoftDeleteAsync(id, usuario);
            return dto == null ? NotFound() : Ok(dto);
        }

        /// <summary>
        /// Agrega o actualiza una traducción para un concepto ajuste.
        /// </summary>
        /// <param name="id">ID del concepto ajuste</param>
        /// <param name="dto">Datos de la traducción (idioma y texto)</param>
        /// <returns>Resultado de la operación</returns>
        [HttpPost("{id}/translations")]
        public async Task<IActionResult> AddOrUpdateTranslation(int id, [FromBody] TranslationInputDTO dto)
        {
            var supportedLanguages = new[] { "es", "en", "fr" };
            
            var lang = string.IsNullOrWhiteSpace(dto.Language) 
                ? "es" 
                : dto.Language.Split('-', StringSplitOptions.RemoveEmptyEntries)[0].ToLowerInvariant();
            
            if (!supportedLanguages.Contains(lang))
            {
                return BadRequest(new 
                { 
                    Message = _localizer["UnsupportedLanguage"].Value,
                    ProvidedLanguage = dto.Language,
                    SupportedLanguages = supportedLanguages
                });
            }

            var usuario = User?.Identity?.Name ?? "system";
            await _service.AddOrUpdateTranslationAsync(id, lang, dto.TranslatedText, usuario);
            
            return Ok(new 
            { 
                Message = _localizer["TranslationUpdated"].Value,
                GroupId = id,
                Language = lang
            });
        }

        /// <summary>
        /// Obtiene el idioma actual de la request basado en Accept-Language.
        /// </summary>
        /// <returns>Código de idioma (es, en, fr)</returns>
        private string GetCurrentLanguage()
        {
            var currentCulture = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName.ToLowerInvariant();
            var supportedLanguages = new[] { "es", "en", "fr" };
            return supportedLanguages.Contains(currentCulture) ? currentCulture : "es";
        }
    }
}
using GoldBusiness.Application.Interfaces;
using GoldBusiness.Domain.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace GoldBusiness.WebApi.Controllers
{
    /// <summary>
    /// Controlador para gestión de Sub Grupos de Cuenta.
    /// Segundo nivel del plan de cuentas, pertenece a un GrupoCuenta.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Policy = "ERPFullAccess")]
    public class SubGrupoCuentaController : ControllerBase
    {
        private readonly ISubGrupoCuentaService _service;

        public SubGrupoCuentaController(ISubGrupoCuentaService service)
        {
            _service = service;
        }

        /// <summary>
        /// Obtiene todos los sub grupos de cuenta.
        /// El idioma se detecta automáticamente del header Accept-Language.
        /// </summary>
        /// <returns>Lista de sub grupos de cuenta localizados</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SubGrupoCuentaDTO>>> Get()
        {
            var lang = GetCurrentLanguage();
            return Ok(await _service.GetAllAsync(lang));
        }

        /// <summary>
        /// Obtiene un sub grupo de cuenta por ID.
        /// El idioma se detecta automáticamente del header Accept-Language.
        /// </summary>
        /// <param name="id">ID del sub grupo de cuenta</param>
        /// <returns>Sub grupo de cuenta localizado</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<SubGrupoCuentaDTO>> Get(int id)
        {
            var lang = GetCurrentLanguage();
            var dto = await _service.GetByIdAsync(id, lang);
            return dto == null ? NotFound() : Ok(dto);
        }

        /// <summary>
        /// Crea un nuevo sub grupo de cuenta.
        /// El idioma se detecta automáticamente del header Accept-Language.
        /// </summary>
        /// <param name="dto">Datos del nuevo sub grupo de cuenta</param>
        /// <returns>Sub grupo de cuenta creado</returns>
        [HttpPost]
        public async Task<ActionResult<SubGrupoCuentaDTO>> Post([FromBody] SubGrupoCuentaDTO dto)
        {
            var lang = GetCurrentLanguage();
            var usuario = User?.Identity?.Name ?? "system";
            var result = await _service.CreateAsync(dto, usuario, lang);
            return CreatedAtAction(nameof(Get), new { id = result.Id }, result);
        }

        /// <summary>
        /// Actualiza un sub grupo de cuenta existente.
        /// El idioma se detecta automáticamente del header Accept-Language.
        /// </summary>
        /// <param name="id">ID del sub grupo de cuenta a actualizar</param>
        /// <param name="dto">Datos actualizados del sub grupo</param>
        /// <returns>Sub grupo de cuenta actualizado</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] SubGrupoCuentaDTO dto)
        {
            var lang = GetCurrentLanguage();
            var usuario = User?.Identity?.Name ?? "system";
            var result = await _service.UpdateAsync(id, dto, usuario, lang);
            return Ok(result);
        }

        /// <summary>
        /// Elimina (soft delete) un sub grupo de cuenta.
        /// </summary>
        /// <param name="id">ID del sub grupo de cuenta a eliminar</param>
        /// <returns>Sub grupo de cuenta eliminado</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var usuario = User?.Identity?.Name ?? "system";
            var dto = await _service.SoftDeleteAsync(id, usuario);
            return dto == null ? NotFound() : Ok(dto);
        }

        /// <summary>
        /// Agrega o actualiza una traducción para un sub grupo de cuenta.
        /// </summary>
        /// <param name="id">ID del sub grupo de cuenta</param>
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
                    Message = "Idioma no soportado. Idiomas válidos: es, en, fr",
                    ProvidedLanguage = dto.Language,
                    SupportedLanguages = supportedLanguages
                });
            }

            var usuario = User?.Identity?.Name ?? "system";
            await _service.AddOrUpdateTranslationAsync(id, lang, dto.TranslatedText, usuario);
            
            return Ok(new 
            { 
                Message = "Traducción actualizada correctamente",
                SubGroupId = id,
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
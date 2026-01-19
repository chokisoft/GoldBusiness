using GoldBusiness.Application.Interfaces;
using GoldBusiness.Domain.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace GoldBusiness.WebApi.Controllers
{
    /// <summary>
    /// Controlador para gestión de Cuentas Contables.
    /// Nivel más detallado del plan de cuentas, pertenece a un SubGrupoCuenta.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Policy = "ERPFullAccess")]
    public class CuentaController : ControllerBase
    {
        private readonly ICuentaService _cuentaService;

        public CuentaController(ICuentaService cuentaService)
        {
            _cuentaService = cuentaService;
        }

        /// <summary>
        /// Obtiene todas las cuentas contables.
        /// El idioma se detecta automáticamente del header Accept-Language.
        /// </summary>
        /// <returns>Lista de cuentas localizadas</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CuentaDTO>>> GetCuenta()
        {
            var lang = GetCurrentLanguage();
            return Ok(await _cuentaService.GetAllAsync(lang));
        }

        /// <summary>
        /// Obtiene una cuenta contable por ID.
        /// El idioma se detecta automáticamente del header Accept-Language.
        /// </summary>
        /// <param name="id">ID de la cuenta</param>
        /// <returns>Cuenta localizada</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<CuentaDTO>> GetCuenta(int id)
        {
            var lang = GetCurrentLanguage();
            var cuenta = await _cuentaService.GetByIdAsync(id, lang);
            return cuenta == null ? NotFound() : Ok(cuenta);
        }

        /// <summary>
        /// Crea una nueva cuenta contable.
        /// El idioma se detecta automáticamente del header Accept-Language.
        /// </summary>
        /// <param name="dto">Datos de la nueva cuenta</param>
        /// <returns>Cuenta creada</returns>
        [HttpPost]
        public async Task<ActionResult<CuentaDTO>> PostCuenta([FromBody] CuentaDTO dto)
        {
            var lang = GetCurrentLanguage();
            var usuario = User?.Identity?.Name ?? "system";
            var result = await _cuentaService.CreateAsync(dto, usuario, lang);
            return CreatedAtAction(nameof(GetCuenta), new { id = result.Id }, result);
        }

        /// <summary>
        /// Actualiza una cuenta contable existente.
        /// El idioma se detecta automáticamente del header Accept-Language.
        /// </summary>
        /// <param name="id">ID de la cuenta a actualizar</param>
        /// <param name="dto">Datos actualizados de la cuenta</param>
        /// <returns>Cuenta actualizada</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCuenta(int id, [FromBody] CuentaDTO dto)
        {
            var lang = GetCurrentLanguage();
            var usuario = User?.Identity?.Name ?? "system";
            var result = await _cuentaService.UpdateAsync(id, dto, usuario, lang);
            return Ok(result);
        }

        /// <summary>
        /// Elimina (soft delete) una cuenta contable.
        /// </summary>
        /// <param name="id">ID de la cuenta a eliminar</param>
        /// <returns>Cuenta eliminada</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCuenta(int id)
        {
            var usuario = User?.Identity?.Name ?? "system";
            var cuenta = await _cuentaService.SoftDeleteAsync(id, usuario);
            return cuenta == null ? NotFound() : Ok(cuenta);
        }

        /// <summary>
        /// Agrega o actualiza una traducción para una cuenta.
        /// </summary>
        /// <param name="id">ID de la cuenta</param>
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
            await _cuentaService.AddOrUpdateTranslationAsync(id, lang, dto.TranslatedText, usuario);
            
            return Ok(new 
            { 
                Message = "Traducción actualizada correctamente",
                AccountId = id,
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
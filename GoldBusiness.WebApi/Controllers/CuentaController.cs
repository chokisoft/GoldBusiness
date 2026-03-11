using GoldBusiness.Application.Interfaces;
using GoldBusiness.Domain.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.Globalization;

namespace GoldBusiness.WebApi.Controllers
{
    /// <summary>
    /// Controlador para gestión de Cuentas Contables.
    /// Nivel más detallado del plan de cuentas, pertenece a un SubGrupoCuenta.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Policy = "ERPAdminOrFullAccess")]
    public class CuentaController : BaseEntityController
    {
        private readonly ICuentaService _service;

        public CuentaController(
            ICuentaService service,
            IStringLocalizer<GoldBusiness.Domain.Resources.ValidationMessages> localizer) : base(localizer)
        {
            _service = service;
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
            return Ok(await _service.GetAllAsync(lang));
        }

        [HttpGet("paged")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> GetPaged(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 50,
            [FromQuery] string? term = null,
            [FromQuery] int? subGrupoCuentaId = null)
        {
            var lang = GetCurrentLanguage();
            var (items, total) = await _service.GetPagedAsync(page, pageSize, term, subGrupoCuentaId, lang);
            return Ok(new { items, total });
        }

        /// <summary>
        /// Obtiene una cuenta contable por ID.
        /// El idioma se detecta automáticamente del header Accept-Language.
        /// </summary>
        /// <param name="id">ID de la cuenta</param>
        /// <returns>Cuenta localizada</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<CuentaDTO>> GetById(int id)
        {
            var lang = GetCurrentLanguage();
            var cuenta = await _service.GetByIdAsync(id, lang);

            if (cuenta == null)
                return NotFound();

            return Ok(cuenta); // ✅ Ya es un DTO
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
            var result = await _service.CreateAsync(dto, usuario, lang);
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
            var result = await _service.UpdateAsync(id, dto, usuario, lang);
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
            var cuenta = await _service.SoftDeleteAsync(id, usuario);
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
                AccountId = id,
                Language = lang
            });
        }
    }
}
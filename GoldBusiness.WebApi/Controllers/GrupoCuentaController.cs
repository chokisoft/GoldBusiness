using GoldBusiness.Application.Interfaces;
using GoldBusiness.Domain.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.Globalization;

namespace GoldBusiness.WebApi.Controllers
{
    /// <summary>
    /// Controlador para gestión de Grupos de Cuenta.
    /// Nivel superior del plan de cuentas (Activo, Pasivo, Patrimonio, Ingresos, Gastos).
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Policy = "ERPFullAccess")]
    public class GrupoCuentaController : BaseEntityController
    {
        private readonly IGrupoCuentaService _service;

        public GrupoCuentaController(
            IGrupoCuentaService service,
            IStringLocalizer<GoldBusiness.Domain.Resources.ValidationMessages> localizer) : base(localizer)
        {
            _service = service;
        }

        /// <summary>
        /// Obtiene todos los grupos de cuenta.
        /// El idioma se detecta automáticamente del header Accept-Language.
        /// </summary>
        /// <returns>Lista de grupos de cuenta localizados</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GrupoCuentaDTO>>> Get()
        {
            var lang = GetCurrentLanguage();
            return Ok(await _service.GetAllAsync(lang));
        }

        /// <summary>
        /// Obtiene un grupo de cuenta por ID.
        /// El idioma se detecta automáticamente del header Accept-Language.
        /// </summary>
        /// <param name="id">ID del grupo de cuenta</param>
        /// <returns>Grupo de cuenta localizado</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<GrupoCuentaDTO>> Get(int id)
        {
            var lang = GetCurrentLanguage();
            var dto = await _service.GetByIdAsync(id, lang);
            return dto == null ? NotFound() : Ok(dto);
        }

        /// <summary>
        /// Crea un nuevo grupo de cuenta.
        /// Si existe un código cancelado, lo reactiva automáticamente.
        /// El idioma se detecta automáticamente del header Accept-Language.
        /// </summary>
        /// <param name="dto">Datos del nuevo grupo de cuenta</param>
        /// <returns>Grupo de cuenta creado o reactivado</returns>
        [HttpPost]
        public async Task<ActionResult<GrupoCuentaDTO>> Post([FromBody] GrupoCuentaDTO dto)
        {
            try
            {
                var lang = GetCurrentLanguage();
                var usuario = GetCurrentUser();
                var result = await _service.CreateAsync(dto, usuario, lang);

                // Detectar si fue reactivado
                if (WasReactivated(result.FechaHoraCreado, result.FechaHoraModificado))
                {
                    return CreateReactivatedResponse(result, result.Codigo);
                }

                return CreatedAtAction(nameof(Get), new { id = result.Id }, result);
            }
            catch (InvalidOperationException ex)
            {
                return HandleDuplicateCodeError(nameof(dto.Codigo), ex.Message);
            }
        }

        /// <summary>
        /// Actualiza un grupo de cuenta existente.
        /// El idioma se detecta automáticamente del header Accept-Language.
        /// </summary>
        /// <param name="id">ID del grupo de cuenta a actualizar</param>
        /// <param name="dto">Datos actualizados del grupo</param>
        /// <returns>Grupo de cuenta actualizado</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] GrupoCuentaDTO dto)
        {
            try
            {
                var lang = GetCurrentLanguage();
                var usuario = GetCurrentUser();
                var result = await _service.UpdateAsync(id, dto, usuario, lang);
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return HandleDuplicateCodeError(nameof(dto.Codigo), ex.Message);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        /// <summary>
        /// Elimina (soft delete) un grupo de cuenta.
        /// </summary>
        /// <param name="id">ID del grupo de cuenta a eliminar</param>
        /// <returns>Grupo de cuenta eliminado</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var usuario = User?.Identity?.Name ?? "system";
            var dto = await _service.SoftDeleteAsync(id, usuario);
            return dto == null ? NotFound() : Ok(dto);
        }

        /// <summary>
        /// Agrega o actualiza una traducción para un grupo de cuenta.
        /// </summary>
        /// <param name="id">ID del grupo de cuenta</param>
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
    }
}
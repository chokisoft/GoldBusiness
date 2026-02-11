using GoldBusiness.Application.Interfaces;
using GoldBusiness.Domain.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace GoldBusiness.WebApi.Controllers
{
    /// <summary>
    /// Controlador para gestión de Localidades.
    /// Una localidad representa una ubicación física dentro de un establecimiento.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Policy = "ERPFullAccess")]
    public class LocalidadController : BaseEntityController
    {
        private readonly ILocalidadService _service;

        public LocalidadController(
            ILocalidadService service,
            IStringLocalizer<GoldBusiness.Domain.Resources.ValidationMessages> localizer) : base(localizer)
        {
            _service = service;
        }

        /// <summary>
        /// Obtiene todas las localidades.
        /// El idioma se detecta automáticamente del header Accept-Language.
        /// </summary>
        /// <returns>Lista de localidades localizadas</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LocalidadDTO>>> Get()
        {
            var lang = GetCurrentLanguage();
            return Ok(await _service.GetAllAsync(lang));
        }

        /// <summary>
        /// Obtiene todas las localidades de un establecimiento específico.
        /// </summary>
        /// <param name="establecimientoId">ID del establecimiento</param>
        /// <returns>Lista de localidades del establecimiento</returns>
        [HttpGet("establecimiento/{establecimientoId}")]
        public async Task<ActionResult<IEnumerable<LocalidadDTO>>> GetByEstablecimiento(int establecimientoId)
        {
            var lang = GetCurrentLanguage();
            return Ok(await _service.GetByEstablecimientoIdAsync(establecimientoId, lang));
        }

        /// <summary>
        /// Obtiene una localidad por ID.
        /// El idioma se detecta automáticamente del header Accept-Language.
        /// </summary>
        /// <param name="id">ID de la localidad</param>
        /// <returns>Localidad localizada</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<LocalidadDTO>> Get(int id)
        {
            var lang = GetCurrentLanguage();
            var dto = await _service.GetByIdAsync(id, lang);
            return dto == null ? NotFound() : Ok(dto);
        }

        /// <summary>
        /// Crea una nueva localidad.
        /// El idioma se detecta automáticamente del header Accept-Language.
        /// </summary>
        /// <param name="dto">Datos de la nueva localidad</param>
        /// <returns>Localidad creada</returns>
        [HttpPost]
        public async Task<ActionResult<LocalidadDTO>> Post([FromBody] LocalidadDTO dto)
        {
            try
            {
                var lang = GetCurrentLanguage();
                var usuario = GetCurrentUser();
                var result = await _service.CreateAsync(dto, usuario, lang);

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
        /// Actualiza una localidad existente.
        /// El idioma se detecta automáticamente del header Accept-Language.
        /// </summary>
        /// <param name="id">ID de la localidad a actualizar</param>
        /// <param name="dto">Datos actualizados de la localidad</param>
        /// <returns>Localidad actualizada</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] LocalidadDTO dto)
        {
            var lang = GetCurrentLanguage();
            var usuario = GetCurrentUser();
            var result = await _service.UpdateAsync(id, dto, usuario, lang);
            return Ok(result);
        }

        /// <summary>
        /// Elimina (soft delete) una localidad.
        /// </summary>
        /// <param name="id">ID de la localidad a eliminar</param>
        /// <returns>Localidad eliminada</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var usuario = GetCurrentUser();
            var dto = await _service.SoftDeleteAsync(id, usuario);
            return dto == null ? NotFound() : Ok(dto);
        }

        /// <summary>
        /// Agrega o actualiza una traducción para una localidad.
        /// </summary>
        /// <param name="id">ID de la localidad</param>
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

            var usuario = GetCurrentUser();
            await _service.AddOrUpdateTranslationAsync(id, lang, dto.TranslatedText, usuario);

            return Ok(new
            {
                Message = _localizer["TranslationUpdated"].Value,
                LocalidadId = id,
                Language = lang
            });
        }
    }
}
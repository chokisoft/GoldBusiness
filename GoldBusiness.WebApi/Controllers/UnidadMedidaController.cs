using GoldBusiness.Application.Interfaces;
using GoldBusiness.Application.Services;
using GoldBusiness.Domain.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace GoldBusiness.WebApi.Controllers
{
    /// <summary>
    /// Controlador para gestión de Unidades de Medida.
    /// Las unidades de medida definen cómo se cuantifican los productos (kg, litros, metros, etc.).
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Policy = "ERPAdminOrFullAccess")]
    public class UnidadMedidaController : BaseEntityController
    {
        private readonly IUnidadMedidaService _service;

        public UnidadMedidaController(
            IUnidadMedidaService service,
            IStringLocalizer<GoldBusiness.Domain.Resources.ValidationMessages> localizer) : base(localizer)
        {
            _service = service;
        }

        /// <summary>
        /// Obtiene todas las unidades de medida activas.
        /// </summary>
        /// <returns>Lista de unidades de medida</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<UnidadMedidaDTO>), 200)]
        public async Task<ActionResult<IEnumerable<UnidadMedidaDTO>>> Get()
        {
            var lang = GetCurrentLanguage();
            return Ok(await _service.GetAllAsync(lang));
        }

        [HttpGet("paged")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> GetPaged(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 50,
            [FromQuery] string? term = null)
        {
            var lang = GetCurrentLanguage();
            var (items, total) = await _service.GetPagedAsync(page, pageSize, term, lang);
            return Ok(new { items, total });
        }

        /// <summary>
        /// Obtiene una unidad de medida por su ID.
        /// </summary>
        /// <param name="id">ID de la unidad de medida</param>
        /// <returns>Unidad de medida encontrada</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(UnidadMedidaDTO), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<UnidadMedidaDTO>> Get(int id)
        {
            var lang = GetCurrentLanguage();
            var dto = await _service.GetByIdAsync(id, lang);
            return dto == null ? NotFound() : Ok(dto);
        }

        /// <summary>
        /// Crea una nueva unidad de medida.
        /// </summary>
        /// <param name="dto">Datos de la unidad de medida a crear</param>
        /// <returns>Unidad de medida creada</returns>
        [HttpPost]
        [ProducesResponseType(typeof(UnidadMedidaDTO), 201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<UnidadMedidaDTO>> Post([FromBody] UnidadMedidaDTO dto)
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
        /// Actualiza una unidad de medida existente.
        /// </summary>
        /// <param name="id">ID de la unidad de medida</param>
        /// <param name="dto">Datos actualizados</param>
        /// <returns>Unidad de medida actualizada</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(UnidadMedidaDTO), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Put(int id, [FromBody] UnidadMedidaDTO dto)
        {
            var lang = GetCurrentLanguage();
            var usuario = GetCurrentUser();
            var result = await _service.UpdateAsync(id, dto, usuario, lang);
            return Ok(result);
        }

        /// <summary>
        /// Elimina (soft delete) una unidad de medida.
        /// </summary>
        /// <param name="id">ID de la unidad de medida</param>
        /// <returns>Unidad de medida eliminada</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(UnidadMedidaDTO), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Delete(int id)
        {
            var usuario = GetCurrentUser();
            var dto = await _service.SoftDeleteAsync(id, usuario);
            return dto == null ? NotFound() : Ok(dto);
        }

        /// <summary>
        /// Agrega o actualiza una traducción para una unidad de medida.
        /// </summary>
        /// <param name="id">ID de la unidad de medida</param>
        /// <param name="dto">Datos de la traducción</param>
        /// <returns>Resultado de la operación</returns>
        [HttpPost("{id}/translations")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
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
                UnidadMedidaId = id,
                Language = lang
            });
        }
    }
}
using GoldBusiness.Application.Interfaces;
using GoldBusiness.Domain.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.Globalization;

namespace GoldBusiness.WebApi.Controllers
{
    /// <summary>
    /// Controlador para gesti�n de Proveedores.
    /// Gestiona informaci�n de clientes, proveedores y terceros del negocio.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Policy = "ERPAdminOrFullAccess")]
    public class ProveedorController(IProveedorService service, IStringLocalizer<GoldBusiness.Domain.Resources.ValidationMessages> localizer) : BaseEntityController(localizer)
    {
        private readonly IProveedorService _service = service;

        /// <summary>
        /// Obtiene todos los Proveedores.
        /// El idioma se detecta autom�ticamente del header Accept-Language.
        /// </summary>
        /// <returns>Lista de Proveedores localizados</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProveedorDTO>>> Get()
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
        /// Obtiene un Proveedor por ID.
        /// El idioma se detecta autom�ticamente del header Accept-Language.
        /// </summary>
        /// <param name="id">ID del Proveedor</param>
        /// <returns>Proveedor localizado</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<ProveedorDTO>> Get(int id)
        {
            var lang = GetCurrentLanguage();
            var dto = await _service.GetByIdAsync(id, lang);
            return dto == null ? NotFound() : Ok(dto);
        }

        /// <summary>
        /// Crea un nuevo Proveedor.
        /// El idioma se detecta autom�ticamente del header Accept-Language.
        /// </summary>
        /// <param name="dto">Datos del nuevo Proveedor</param>
        /// <returns>Proveedor creado</returns>
        [HttpPost]
        public async Task<ActionResult<ProveedorDTO>> Post([FromBody] ProveedorDTO dto)
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
        /// Actualiza un Proveedor existente.
        /// El idioma se detecta autom�ticamente del header Accept-Language.
        /// </summary>
        /// <param name="id">ID del Proveedor a actualizar</param>
        /// <param name="dto">Datos actualizados del Proveedor</param>
        /// <returns>Proveedor actualizado</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] ProveedorDTO dto)
        {
            var lang = GetCurrentLanguage();
            var usuario = User?.Identity?.Name ?? "system";
            var result = await _service.UpdateAsync(id, dto, usuario, lang);
            return Ok(result);
        }

        /// <summary>
        /// Elimina (soft delete) un Proveedor.
        /// </summary>
        /// <param name="id">ID del Proveedor a eliminar</param>
        /// <returns>Proveedor eliminado</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var usuario = User?.Identity?.Name ?? "system";
            var dto = await _service.SoftDeleteAsync(id, usuario);
            return dto == null ? NotFound() : Ok(dto);
        }

        /// <summary>
        /// Agrega o actualiza una traducci�n para un Proveedor.
        /// </summary>
        /// <param name="id">ID del Proveedor</param>
        /// <param name="dto">Datos de la traducci�n (idioma y texto)</param>
        /// <returns>Resultado de la operaci�n</returns>
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
                ProveedorId = id,
                Language = lang
            });
        }
    }
}
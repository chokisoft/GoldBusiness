using GoldBusiness.Application.Interfaces;
using GoldBusiness.Domain.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace GoldBusiness.WebApi.Controllers
{
    /// <summary>
    /// Controlador para gestión de Productos.
    /// Un producto representa un artículo o servicio del inventario.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Policy = "ERPFullAccess")]
    public class ProductoController : BaseEntityController
    {
        private readonly IProductoService _service;

        public ProductoController(
            IProductoService service,
            IStringLocalizer<GoldBusiness.Domain.Resources.ValidationMessages> localizer) : base(localizer)
        {
            _service = service;
        }

        /// <summary>
        /// Obtiene todos los productos.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductoDTO>>> Get()
        {
            var lang = GetCurrentLanguage();
            return Ok(await _service.GetAllAsync(lang));
        }

        /// <summary>
        /// Obtiene todos los productos de un establecimiento específico.
        /// </summary>
        [HttpGet("establecimiento/{establecimientoId}")]
        public async Task<ActionResult<IEnumerable<ProductoDTO>>> GetByEstablecimiento(int establecimientoId)
        {
            var lang = GetCurrentLanguage();
            return Ok(await _service.GetByEstablecimientoIdAsync(establecimientoId, lang));
        }

        /// <summary>
        /// Obtiene todos los productos de un proveedor específico.
        /// </summary>
        [HttpGet("proveedor/{proveedorId}")]
        public async Task<ActionResult<IEnumerable<ProductoDTO>>> GetByProveedor(int proveedorId)
        {
            var lang = GetCurrentLanguage();
            return Ok(await _service.GetByProveedorIdAsync(proveedorId, lang));
        }

        /// <summary>
        /// Obtiene todos los productos de una sublínea específica.
        /// </summary>
        [HttpGet("sublinea/{subLineaId}")]
        public async Task<ActionResult<IEnumerable<ProductoDTO>>> GetBySubLinea(int subLineaId)
        {
            var lang = GetCurrentLanguage();
            return Ok(await _service.GetBySubLineaIdAsync(subLineaId, lang));
        }

        /// <summary>
        /// Obtiene un producto por ID.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductoDTO>> Get(int id)
        {
            var lang = GetCurrentLanguage();
            var dto = await _service.GetByIdAsync(id, lang);
            return dto == null ? NotFound() : Ok(dto);
        }

        /// <summary>
        /// Crea un nuevo producto.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<ProductoDTO>> Post([FromBody] ProductoDTO dto)
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
        /// Actualiza un producto existente.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] ProductoDTO dto)
        {
            var lang = GetCurrentLanguage();
            var usuario = GetCurrentUser();
            var result = await _service.UpdateAsync(id, dto, usuario, lang);
            return Ok(result);
        }

        /// <summary>
        /// Elimina (soft delete) un producto.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var usuario = GetCurrentUser();
            var dto = await _service.SoftDeleteAsync(id, usuario);
            return dto == null ? NotFound() : Ok(dto);
        }

        /// <summary>
        /// Agrega o actualiza una traducción para un producto.
        /// </summary>
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
                ProductoId = id,
                Language = lang
            });
        }
    }
}
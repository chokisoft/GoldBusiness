using GoldBusiness.Application.Interfaces;
using GoldBusiness.Domain.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace GoldBusiness.WebApi.Controllers
{
    /// <summary>
    /// Controlador para gestión de Fichas de Producto (BOM).
    /// Una ficha de producto representa la composición o estructura de un producto.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Policy = "ERPFullAccess")]
    public class FichaProductoController : BaseEntityController
    {
        private readonly IFichaProductoService _service;

        public FichaProductoController(
            IFichaProductoService service,
            IStringLocalizer<GoldBusiness.Domain.Resources.ValidationMessages> localizer) : base(localizer)
        {
            _service = service;
        }

        /// <summary>
        /// Obtiene todas las fichas de producto.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FichaProductoDTO>>> Get()
        {
            var lang = GetCurrentLanguage();
            return Ok(await _service.GetAllAsync(lang));
        }

        /// <summary>
        /// Obtiene todas las fichas de un producto específico.
        /// </summary>
        [HttpGet("producto/{productoId}")]
        public async Task<ActionResult<IEnumerable<FichaProductoDTO>>> GetByProducto(int productoId)
        {
            var lang = GetCurrentLanguage();
            return Ok(await _service.GetByProductoIdAsync(productoId, lang));
        }

        /// <summary>
        /// Obtiene todas las fichas de una localidad específica.
        /// </summary>
        [HttpGet("localidad/{localidadId}")]
        public async Task<ActionResult<IEnumerable<FichaProductoDTO>>> GetByLocalidad(int localidadId)
        {
            var lang = GetCurrentLanguage();
            return Ok(await _service.GetByLocalidadIdAsync(localidadId, lang));
        }

        /// <summary>
        /// Obtiene una ficha de producto por ID.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<FichaProductoDTO>> Get(int id)
        {
            var lang = GetCurrentLanguage();
            var dto = await _service.GetByIdAsync(id, lang);
            return dto == null ? NotFound() : Ok(dto);
        }

        /// <summary>
        /// Crea una nueva ficha de producto.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<FichaProductoDTO>> Post([FromBody] FichaProductoDTO dto)
        {
            try
            {
                var lang = GetCurrentLanguage();
                var usuario = GetCurrentUser();
                var result = await _service.CreateAsync(dto, usuario, lang);

                return CreatedAtAction(nameof(Get), new { id = result.Id }, result);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        /// <summary>
        /// Actualiza una ficha de producto existente.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] FichaProductoDTO dto)
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
                return BadRequest(new { error = ex.Message });
            }
        }

        /// <summary>
        /// Elimina (soft delete) una ficha de producto.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var usuario = GetCurrentUser();
            var dto = await _service.SoftDeleteAsync(id, usuario);
            return dto == null ? NotFound() : Ok(dto);
        }
    }
}
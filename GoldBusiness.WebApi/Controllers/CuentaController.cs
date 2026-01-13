using GoldBusiness.Application.Interfaces;
using GoldBusiness.Domain.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GoldBusiness.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "DESARROLLADOR")]
    public class CuentaController : ControllerBase
    {
        private readonly ICuentaService _cuentaService;

        public CuentaController(ICuentaService cuentaService)
        {
            _cuentaService = cuentaService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CuentaDTO>>> GetCuenta([FromQuery] string? lang = "es")
            => Ok(await _cuentaService.GetAllAsync(lang ?? "es"));

        [HttpGet("{id}")]
        public async Task<ActionResult<CuentaDTO>> GetCuenta(int id, [FromQuery] string? lang = "es")
        {
            var cuenta = await _cuentaService.GetByIdAsync(id, lang ?? "es");
            return cuenta == null ? NotFound() : Ok(cuenta);
        }

        [HttpPost]
        public async Task<ActionResult<CuentaDTO>> PostCuenta(CuentaDTO dto, [FromQuery] string? lang = "es")
            => Ok(await _cuentaService.CreateAsync(dto, User?.Identity?.Name ?? "system", lang ?? "es"));

        [HttpPut("{id}")]
        public async Task<IActionResult> PutCuenta(int id, CuentaDTO dto, [FromQuery] string? lang = "es")
            => Ok(await _cuentaService.UpdateAsync(id, dto, User?.Identity?.Name ?? "system", lang ?? "es"));

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCuenta(int id)
        {
            var cuenta = await _cuentaService.SoftDeleteAsync(id, User?.Identity?.Name ?? "system");
            return cuenta == null ? NotFound() : Ok(cuenta);
        }

        // Endpoint para añadir/actualizar traducción
        [HttpPost("{id}/translations")]
        public async Task<IActionResult> AddOrUpdateTranslation(int id, TranslationInputDTO dto)
        {
            var supported = new[] { "es", "en", "fr" };
            var lang = string.IsNullOrWhiteSpace(dto.Language) ? "es" : dto.Language.Split('-', StringSplitOptions.RemoveEmptyEntries)[0].ToLowerInvariant();
            if (!supported.Contains(lang)) return BadRequest("Idioma no soportado.");

            await _cuentaService.AddOrUpdateTranslationAsync(id, lang, dto.Descripcion, User?.Identity?.Name ?? "system");
            return Ok();
        }
    }
}
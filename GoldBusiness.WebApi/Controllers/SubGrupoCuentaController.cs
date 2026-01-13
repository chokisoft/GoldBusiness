using GoldBusiness.Application.Interfaces;
using GoldBusiness.Domain.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GoldBusiness.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "DESARROLLADOR")]
    public class SubGrupoCuentaController : ControllerBase
    {
        private readonly ISubGrupoCuentaService _service;

        public SubGrupoCuentaController(ISubGrupoCuentaService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SubGrupoCuentaDTO>>> Get([FromQuery] string? lang = "es")
            => Ok(await _service.GetAllAsync(lang ?? "es"));

        [HttpGet("{id}")]
        public async Task<ActionResult<SubGrupoCuentaDTO>> Get(int id, [FromQuery] string? lang = "es")
        {
            var dto = await _service.GetByIdAsync(id, lang ?? "es");
            return dto == null ? NotFound() : Ok(dto);
        }

        [HttpPost]
        public async Task<ActionResult<SubGrupoCuentaDTO>> Post(SubGrupoCuentaDTO dto, [FromQuery] string? lang = "es")
            => Ok(await _service.CreateAsync(dto, User?.Identity?.Name ?? "system", lang ?? "es"));

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, SubGrupoCuentaDTO dto, [FromQuery] string? lang = "es")
            => Ok(await _service.UpdateAsync(id, dto, User?.Identity?.Name ?? "system", lang ?? "es"));

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var dto = await _service.SoftDeleteAsync(id, User?.Identity?.Name ?? "system");
            return dto == null ? NotFound() : Ok(dto);
        }

        [HttpPost("{id}/translations")]
        public async Task<IActionResult> AddOrUpdateTranslation(int id, TranslationInputDTO dto)
        {
            var supported = new[] { "es", "en", "fr" };
            var lang = string.IsNullOrWhiteSpace(dto.Language) ? "es" : dto.Language.Split('-', StringSplitOptions.RemoveEmptyEntries)[0].ToLowerInvariant();
            if (!supported.Contains(lang)) return BadRequest("Idioma no soportado.");

            await _service.AddOrUpdateTranslationAsync(id, lang, dto.Descripcion, User?.Identity?.Name ?? "system");
            return Ok();
        }
    }
}
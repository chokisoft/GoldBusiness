using GoldBusiness.Application.Interfaces;
using GoldBusiness.Domain.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.Globalization;

namespace GoldBusiness.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Policy = "ERPFullAccess")]
    public class SystemConfigurationController : BaseEntityController
    {
        private readonly ISystemConfigurationService _service;

        public SystemConfigurationController(ISystemConfigurationService service,
            IStringLocalizer<GoldBusiness.Domain.Resources.ValidationMessages> localizer) : base(localizer)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SystemConfigurationDTO>>> GetAll()
        {
            var lang = GetCurrentLanguage();
            return Ok(await _service.GetAllAsync(lang));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SystemConfigurationDTO>> GetById(int id)
        {
            var lang = GetCurrentLanguage();
            var result = await _service.GetByIdAsync(id, lang);
            return result == null ? NotFound() : Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<SystemConfigurationDTO>> Create([FromBody] SystemConfigurationDTO dto)
        {
            var lang = GetCurrentLanguage();
            var usuario = User?.Identity?.Name ?? "system";
            var result = await _service.CreateAsync(dto, usuario, lang);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] SystemConfigurationDTO dto)
        {
            var lang = GetCurrentLanguage();
            var usuario = User?.Identity?.Name ?? "system";
            var result = await _service.UpdateAsync(id, dto, usuario, lang);
            return Ok(result);
        }

        [HttpPost("{id}/translations")]
        public async Task<IActionResult> AddOrUpdateTranslation(
            int id,
            [FromBody] TranslationRequest request)
        {
            var usuario = User?.Identity?.Name ?? "system";
            await _service.AddOrUpdateTranslationAsync(
                id, request.Language, request.NombreNegocio, request.Direccion, request.Municipio, request.Provincia, usuario);
            return NoContent();
        }

        public class TranslationRequest
        {
            public string Language { get; set; } = string.Empty;
            public string NombreNegocio { get; set; } = string.Empty;
            public string? Direccion { get; set; }
            public string? Municipio { get; set; }
            public string? Provincia { get; set; }
        }
    }
}
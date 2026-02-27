using GoldBusiness.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace GoldBusiness.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Policy = "ERPFullAccess")]
    public class ProvinciaController : BaseEntityController
    {
        private readonly IProvinciaService _service;

        public ProvinciaController(
            IProvinciaService service,
            IStringLocalizer<GoldBusiness.Domain.Resources.ValidationMessages> localizer) : base(localizer)
        {
            _service = service;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<GoldBusiness.Domain.DTOs.ProvinciaDTO>>> Get()
        {
            var lang = GetCurrentLanguage();
            var result = await _service.GetAllAsync(lang);
            return Ok(result);
        }

        [HttpGet("pais/{paisId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<GoldBusiness.Domain.DTOs.ProvinciaDTO>>> GetByPais(int paisId)
        {
            var lang = GetCurrentLanguage();
            var result = await _service.GetByPaisIdAsync(paisId, lang);
            return Ok(result);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<GoldBusiness.Domain.DTOs.ProvinciaDTO>> Get(int id)
        {
            var lang = GetCurrentLanguage();
            var dto = await _service.GetByIdAsync(id, lang);
            return dto == null ? NotFound() : Ok(dto);
        }
    }
}
using GoldBusiness.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace GoldBusiness.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Policy = "ERPFullAccess")]
    public class CodigoPostalController : BaseEntityController
    {
        private readonly ICodigoPostalService _service;

        public CodigoPostalController(
            ICodigoPostalService service,
            IStringLocalizer<GoldBusiness.Domain.Resources.ValidationMessages> localizer) : base(localizer)
        {
            _service = service;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<GoldBusiness.Domain.DTOs.CodigoPostalDTO>>> Get()
        {
            var lang = GetCurrentLanguage();
            var result = await _service.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("paged")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> GetPaged([FromQuery] int page = 1, [FromQuery] int pageSize = 50, [FromQuery] string? term = null, [FromQuery] int? municipioId = null)
        {
            var lang = GetCurrentLanguage();
            var (items, total) = await _service.GetPagedAsync(page, pageSize, term, municipioId, lang);
            return Ok(new { items, total });
        }

        [HttpGet("municipio/{municipioId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<GoldBusiness.Domain.DTOs.CodigoPostalDTO>>> GetByMunicipio(int municipioId)
        {
            var lang = GetCurrentLanguage();
            var list = await _service.GetByMunicipioIdAsync(municipioId, lang);
            return Ok(list);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<GoldBusiness.Domain.DTOs.CodigoPostalDTO>> Get(int id)
        {
            var lang = GetCurrentLanguage();
            var dto = await _service.GetByIdAsync(id, lang);
            return dto == null ? NotFound() : Ok(dto);
        }

    }
}
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

        [HttpGet("by-municipio/{municipioId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<GoldBusiness.Domain.DTOs.CodigoPostalDTO>>> GetByMunicipio(int municipioId)
        {
            var lang = GetCurrentLanguage();
            var list = await _service.GetByMunicipioIdAsync(municipioId, lang);
            return Ok(list);
        }
    }
}
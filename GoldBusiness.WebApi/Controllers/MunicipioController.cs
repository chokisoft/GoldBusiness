using GoldBusiness.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace GoldBusiness.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Policy = "ERPFullAccess")]
    public class MunicipioController : BaseEntityController
    {
        private readonly IMunicipioService _service;

        public MunicipioController(
            IMunicipioService service,
            IStringLocalizer<GoldBusiness.Domain.Resources.ValidationMessages> localizer) : base(localizer)
        {
            _service = service;
        }

        [HttpGet("by-provincia/{provinciaId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<GoldBusiness.Domain.DTOs.MunicipioDTO>>> GetByProvincia(int provinciaId)
        {
            var lang = GetCurrentLanguage();
            var list = await _service.GetByProvinciaIdAsync(provinciaId, lang);
            return Ok(list);
        }
    }
}
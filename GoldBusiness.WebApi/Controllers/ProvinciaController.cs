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

        [HttpGet("by-pais/{paisId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<GoldBusiness.Domain.DTOs.ProvinciaDTO>>> GetByPais(int paisId)
        {
            var lang = GetCurrentLanguage();
            var list = await _service.GetByPaisIdAsync(paisId, lang);
            return Ok(list);
        }
    }
}
using GoldBusiness.Application.Interfaces;
using GoldBusiness.Domain.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace GoldBusiness.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Policy = "ERPFullAccess")]
    [ApiExplorerSettings(GroupName = "v2")]
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
        public async Task<ActionResult<IEnumerable<CodigoPostalDTO>>> Get()
            => Ok(await _service.GetAllAsync());

        [HttpGet("municipio/{municipioId}")]
        public async Task<ActionResult<IEnumerable<CodigoPostalDTO>>> GetByMunicipio(int municipioId)
            => Ok(await _service.GetByMunicipioIdAsync(municipioId, "es"));

        [HttpGet("{id}")]
        public async Task<ActionResult<CodigoPostalDTO>> Get(int id)
        {
            var dto = await _service.GetByIdAsync(id, "es");
            return dto == null ? NotFound() : Ok(dto);
        }

        [HttpPost]
        public async Task<ActionResult<CodigoPostalDTO>> Post([FromBody] CodigoPostalDTO dto)
        {
            try
            {
                var usuario = GetCurrentUser();
                var result = await _service.CreateAsync(dto, "es", usuario);

                if (WasReactivated(result.FechaHoraCreado, result.FechaHoraModificado))
                    return CreateReactivatedResponse(result, result.Codigo);

                return CreatedAtAction(nameof(Get), new { id = result.Id }, result);
            }
            catch (InvalidOperationException ex)
            {
                return HandleDuplicateCodeError(nameof(dto.Codigo), ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] CodigoPostalDTO dto)
        {
            var usuario = GetCurrentUser();
            var result = await _service.UpdateAsync(id, dto, "es", usuario);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var usuario = GetCurrentUser();
            var dto = await _service.SoftDeleteAsync(id, usuario);
            return dto == null ? NotFound() : Ok(dto);
        }
    }
}
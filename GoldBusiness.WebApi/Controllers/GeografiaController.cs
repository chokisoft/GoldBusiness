using GoldBusiness.Application.Interfaces;
using GoldBusiness.Domain.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace GoldBusiness.WebApi.Controllers
{
    /// <summary>
    /// Controlador para consultas geográficas (País/Provincia/Municipio).
    /// Usado principalmente para selectores en formularios.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Policy = "ERPFullAccess")]
    public class GeografiaController : BaseEntityController
    {
        private readonly IPaisService _paisService;
        private readonly IProvinciaService _provinciaService;
        private readonly IMunicipioService _municipioService;
        private readonly ICodigoPostalService _codigoPostalService;

        public GeografiaController(
            IPaisService paisService,
            IProvinciaService provinciaService,
            IMunicipioService municipioService,
            ICodigoPostalService codigoPostalService,
            IStringLocalizer<GoldBusiness.Domain.Resources.ValidationMessages> localizer) 
            : base(localizer)
        {
            _paisService = paisService;
            _provinciaService = provinciaService;
            _municipioService = municipioService;
            _codigoPostalService = codigoPostalService;
        }

        /// <summary>
        /// Obtiene todos los países activos.
        /// El idioma se detecta automáticamente del header Accept-Language.
        /// </summary>
        [HttpGet("paises")]
        public async Task<ActionResult<IEnumerable<PaisDTO>>> GetPaises()
        {
            var lang = GetCurrentLanguage();
            return Ok(await _paisService.GetAllAsync(lang));
        }

        /// <summary>
        /// Obtiene un país por ID.
        /// </summary>
        [HttpGet("paises/{id}")]
        public async Task<ActionResult<PaisDTO>> GetPaisById(int id)
        {
            var lang = GetCurrentLanguage();
            var pais = await _paisService.GetByIdAsync(id, lang);
            
            if (pais == null)
                return NotFound();
            
            return Ok(pais);
        }

        /// <summary>
        /// Obtiene todas las provincias de un país.
        /// </summary>
        [HttpGet("paises/{paisId}/provincias")]
        public async Task<ActionResult<IEnumerable<ProvinciaDTO>>> GetProvinciasByPais(int paisId)
        {
            var lang = GetCurrentLanguage();
            return Ok(await _provinciaService.GetByPaisIdAsync(paisId, lang));
        }

        /// <summary>
        /// Obtiene todos los municipios de una provincia.
        /// </summary>
        [HttpGet("provincias/{provinciaId}/municipios")]
        public async Task<ActionResult<IEnumerable<MunicipioDTO>>> GetMunicipiosByProvincia(int provinciaId)
        {
            var lang = GetCurrentLanguage();
            return Ok(await _municipioService.GetByProvinciaIdAsync(provinciaId, lang));
        }

        /// <summary>
        /// Búsqueda de municipios por término (útil para autocomplete).
        /// </summary>
        [HttpGet("municipios/buscar")]
        public async Task<ActionResult<IEnumerable<MunicipioDTO>>> BuscarMunicipios(
            [FromQuery] string termino,
            [FromQuery] int? paisId = null)
        {
            var lang = GetCurrentLanguage();
            return Ok(await _municipioService.BuscarAsync(termino, paisId, lang));
        }

        /// <summary>
        /// Obtiene todos los códigos postales de un municipio.
        /// </summary>
        [HttpGet("municipios/{municipioId}/codigospostales")]
        public async Task<ActionResult<IEnumerable<CodigoPostalDTO>>> GetCodigosPostalesByMunicipio(int municipioId)
        {
            var lang = GetCurrentLanguage();
            return Ok(await _codigoPostalService.GetByMunicipioIdAsync(municipioId, lang));
        }

        /// <summary>
        /// Búsqueda de códigos postales por término (útil para autocomplete).
        /// </summary>
        [HttpGet("codigospostales/buscar")]
        public async Task<ActionResult<IEnumerable<CodigoPostalDTO>>> BuscarCodigosPostales(
            [FromQuery] string termino,
            [FromQuery] int? municipioId = null)
        {
            var lang = GetCurrentLanguage();
            return Ok(await _codigoPostalService.BuscarAsync(termino, municipioId, lang));
        }
    }


}
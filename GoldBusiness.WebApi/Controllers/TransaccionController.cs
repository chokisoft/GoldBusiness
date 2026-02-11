using GoldBusiness.Application.Interfaces;
using GoldBusiness.Domain.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace GoldBusiness.WebApi.Controllers
{
    /// <summary>
    /// Controlador para gestión de Transacciones.
    /// Una transacción representa un tipo de operación o movimiento en el sistema.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Policy = "ERPFullAccess")]
    public class TransaccionController : BaseEntityController
    {
        private readonly ITransaccionService _service;

        public TransaccionController(
            ITransaccionService service,
            IStringLocalizer<GoldBusiness.Domain.Resources.ValidationMessages> localizer) : base(localizer)
        {
            _service = service;
        }

        /// <summary>
        /// Obtiene todas las transacciones.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TransaccionDTO>>> Get()
        {
            var lang = GetCurrentLanguage();
            return Ok(await _service.GetAllAsync(lang));
        }

        /// <summary>
        /// Obtiene una transacción por ID.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<TransaccionDTO>> Get(int id)
        {
            var lang = GetCurrentLanguage();
            var dto = await _service.GetByIdAsync(id, lang);
            return dto == null ? NotFound() : Ok(dto);
        }

        /// <summary>
        /// Crea una nueva transacción.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<TransaccionDTO>> Post([FromBody] TransaccionDTO dto)
        {
            try
            {
                var lang = GetCurrentLanguage();
                var usuario = GetCurrentUser();
                var result = await _service.CreateAsync(dto, usuario, lang);

                return CreatedAtAction(nameof(Get), new { id = result.Id }, result);
            }
            catch (InvalidOperationException ex)
            {
                return HandleDuplicateCodeError(nameof(dto.Codigo), ex.Message);
            }
        }

        /// <summary>
        /// Actualiza una transacción existente.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] TransaccionDTO dto)
        {
            var lang = GetCurrentLanguage();
            var usuario = GetCurrentUser();
            var result = await _service.UpdateAsync(id, dto, usuario, lang);
            return Ok(result);
        }

        /// <summary>
        /// Agrega o actualiza una traducción para una transacción.
        /// </summary>
        [HttpPost("{id}/translations")]
        public async Task<IActionResult> AddOrUpdateTranslation(int id, [FromBody] TranslationInputDTO dto)
        {
            var supportedLanguages = new[] { "es", "en", "fr" };

            var lang = string.IsNullOrWhiteSpace(dto.Language)
                ? "es"
                : dto.Language.Split('-', StringSplitOptions.RemoveEmptyEntries)[0].ToLowerInvariant();

            if (!supportedLanguages.Contains(lang))
            {
                return BadRequest(new
                {
                    Message = _localizer["UnsupportedLanguage"].Value,
                    ProvidedLanguage = dto.Language,
                    SupportedLanguages = supportedLanguages
                });
            }

            var usuario = GetCurrentUser();
            await _service.AddOrUpdateTranslationAsync(id, lang, dto.TranslatedText, usuario);

            return Ok(new
            {
                Message = _localizer["TranslationUpdated"].Value,
                TransaccionId = id,
                Language = lang
            });
        }
    }
}
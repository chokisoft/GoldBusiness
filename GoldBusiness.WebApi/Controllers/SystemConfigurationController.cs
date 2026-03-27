using GoldBusiness.Application.Interfaces;
using GoldBusiness.Domain.DTOs;
using GoldBusiness.Domain.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

namespace GoldBusiness.WebApi.Controllers
{
    /// <summary>
    /// Controlador para gestión de la Configuración del Sistema.
    /// Gestiona los parámetros generales del negocio, licencias y logotipo de la empresa.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Policy = "ERPAdminOrFullAccess")]
    public class SystemConfigurationController : BaseEntityController
    {
        private readonly ISystemConfigurationService _service;
        private readonly IStringLocalizer<ValidationMessages> _localizer;

        private static readonly string LogoBasePath =
            RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
                ? @"F:\Images\CompanyLogo"
                : "/Images/CompanyLogo";

        private static readonly string[] AllowedExtensions =
            [".png", ".jpg", ".jpeg", ".gif", ".webp"];

        public SystemConfigurationController(
            ISystemConfigurationService service,
            IStringLocalizer<ValidationMessages> localizer) : base(localizer)
        {
            _service = service;
            _localizer = localizer;
        }

        /// <summary>Obtiene todas las configuraciones del sistema activas.</summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<SystemConfigurationDTO>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<SystemConfigurationDTO>>> Get()
        {
            var lang = GetCurrentLanguage();
            var list = await _service.GetAllAsync(lang);
            return Ok(list);
        }

        /// <summary>Lista paginada (server-side).</summary>
        [HttpGet("paged")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> GetPaged(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 50,
            [FromQuery] string? term = null)
        {
            var lang = GetCurrentLanguage();
            var (items, total) = await _service.GetPagedAsync(page, pageSize, term, lang);
            return Ok(new { items, total, page, pageSize });
        }

        /// <summary>Obtiene una configuración por ID.</summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(SystemConfigurationDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<SystemConfigurationDTO>> Get(int id)
        {
            var lang = GetCurrentLanguage();
            var dto = await _service.GetByIdAsync(id, lang);
            return dto == null ? NotFound() : Ok(dto);
        }

        /// <summary>Crear nueva configuración. Reactiva si existe cancelado.</summary>
        [HttpPost]
        [ProducesResponseType(typeof(SystemConfigurationDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<SystemConfigurationDTO>> Post([FromBody] SystemConfigurationDTO dto)
        {
            try
            {
                var lang = GetCurrentLanguage();
                var usuario = GetCurrentUser();
                var result = await _service.CreateAsync(dto, usuario, lang);

                if (WasReactivated(result.FechaHoraCreado, result.FechaHoraModificado))
                    return CreateReactivatedResponse(result, result.CodigoSistema);

                return CreatedAtAction(nameof(Get), new { id = result.Id }, result);
            }
            catch (InvalidOperationException ex)
            {
                return HandleDuplicateCodeError(nameof(dto.CodigoSistema), ex.Message);
            }
        }

        /// <summary>Actualiza configuración existente.</summary>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(SystemConfigurationDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Put(int id, [FromBody] SystemConfigurationDTO dto)
        {
            try
            {
                var lang = GetCurrentLanguage();
                var usuario = GetCurrentUser();
                var result = await _service.UpdateAsync(id, dto, usuario, lang);
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return HandleDuplicateCodeError(nameof(dto.CodigoSistema), ex.Message);
            }
        }

        /// <summary>Soft-delete de una configuración (marca Cancelado).</summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var usuario = GetCurrentUser();
            var dto = await _service.SoftDeleteAsync(id, usuario);
            return dto == null ? NotFound() : Ok(dto);
        }

        /// <summary>Agrega o actualiza una traducción para la configuración del sistema.</summary>
        [HttpPost("{id}/translations")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AddOrUpdateTranslation(int id, [FromBody] TranslationRequest request)
        {
            var supportedLanguages = new[] { "es", "en", "fr" };

            var lang = string.IsNullOrWhiteSpace(request.Language)
                ? "es"
                : request.Language.Split('-', StringSplitOptions.RemoveEmptyEntries)[0].ToLowerInvariant();

            if (!supportedLanguages.Contains(lang))
            {
                return BadRequest(new
                {
                    Message = _localizer["UnsupportedLanguage"].Value,
                    ProvidedLanguage = request.Language,
                    SupportedLanguages = supportedLanguages
                });
            }

            var usuario = GetCurrentUser();
            await _service.AddOrUpdateTranslationAsync(
                id, lang, request.NombreNegocio,
                request.Direccion, request.Municipio, request.Provincia, usuario);

            return Ok(new
            {
                Message = _localizer["TranslationUpdated"].Value,
                SystemConfigurationId = id,
                Language = lang
            });
        }

        // ═══════════════════════════════════════════════════════════
        // 🖼️ LOGO
        // ═══════════════════════════════════════════════════════════

        /// <summary>Datos de la solicitud de subida de logo (multipart/form-data).</summary>
        public sealed class UploadLogoRequest
        {
            [Required(
                ErrorMessageResourceType = typeof(ValidationMessages),
                ErrorMessageResourceName = nameof(ValidationMessages.Required)
            )]
            [Display(
                Name = nameof(ValidationMessages.Field_Imagen),
                ResourceType = typeof(ValidationMessages)
            )]
            public IFormFile File { get; set; } = null!;

            [Required(
                ErrorMessageResourceType = typeof(ValidationMessages),
                ErrorMessageResourceName = nameof(ValidationMessages.Required)
            )]
            [Display(
                Name = nameof(ValidationMessages.Field_Codigo),
                ResourceType = typeof(ValidationMessages)
            )]
            [StringLength(50,
                ErrorMessageResourceType = typeof(ValidationMessages),
                ErrorMessageResourceName = nameof(ValidationMessages.StringLengthMax)
            )]
            public string CodigoSistema { get; set; } = string.Empty;
        }

        /// <summary>
        /// Sube el logo del negocio.
        /// Se guarda en F:\Images\CompanyLogo (Windows) o /Images/CompanyLogo (Linux).
        /// </summary>
        [HttpPost("upload-logo")]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(typeof(LogoUploadResult), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<LogoUploadResult>> UploadLogo([FromForm] UploadLogoRequest request)
        {
            if (request.File == null || request.File.Length == 0)
                return BadRequest(new { Message = _localizer["Required"].Value.Replace("{0}", _localizer["Field_Logo"].Value) });

            var extension = Path.GetExtension(request.File.FileName).ToLowerInvariant();
            if (!AllowedExtensions.Contains(extension))
                return BadRequest(new { Message = _localizer["LogoFormatoInvalido"].Value });

            if (request.File.Length > 2 * 1024 * 1024)
                return BadRequest(new { Message = _localizer["LogoTamañoExcedido"].Value });

            var safeCode = new string(request.CodigoSistema
                .Where(c => char.IsLetterOrDigit(c) || c == '-' || c == '_')
                .ToArray());

            if (string.IsNullOrEmpty(safeCode))
                return BadRequest(new { Message = _localizer["CodigoObligatorio"].Value });

            Directory.CreateDirectory(LogoBasePath);

            // Un logo por negocio: usar el código del sistema como nombre base
            var fileName = $"{safeCode}{extension}";
            var filePath = Path.Combine(LogoBasePath, fileName);

            // Eliminar logo anterior si existe
            foreach (var ext in AllowedExtensions)
            {
                var oldFile = Path.Combine(LogoBasePath, $"{safeCode}{ext}");
                if (System.IO.File.Exists(oldFile))
                {
                    System.IO.File.Delete(oldFile);
                }
            }

            await using var stream = new FileStream(filePath, FileMode.Create);
            await request.File.CopyToAsync(stream);

            return Ok(new LogoUploadResult(fileName));
        }

        /// <summary>Sirve el archivo de logo del negocio. No requiere autenticación.</summary>
        [HttpGet("logo/{fileName}")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetLogo(string fileName)
        {
            var safeFileName = Path.GetFileName(fileName);
            if (string.IsNullOrWhiteSpace(safeFileName))
                return BadRequest(new { Message = "Nombre de archivo inválido." });

            var filePath = Path.Combine(LogoBasePath, safeFileName);

            if (!System.IO.File.Exists(filePath))
                return NotFound();

            var extension = Path.GetExtension(safeFileName).ToLowerInvariant();
            var contentType = extension switch
            {
                ".png" => "image/png",
                ".jpg" or ".jpeg" => "image/jpeg",
                ".gif" => "image/gif",
                ".webp" => "image/webp",
                _ => "application/octet-stream"
            };

            return PhysicalFile(filePath, contentType);
        }

        // Request type for translations
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
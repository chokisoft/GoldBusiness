using GoldBusiness.Application.Interfaces;
using GoldBusiness.Domain.DTOs;
using GoldBusiness.Domain.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

namespace GoldBusiness.WebApi.Controllers
{
    // ─── Request types (usan IFormFile → pertenecen al WebApi, no al Domain) ──

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

    // ─────────────────────────────────────────────────────────────────────────

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

        private static readonly string LogoBasePath =
            RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
                ? @"F:\Images\CompanyLogo"
                : "/Images/CompanyLogo";

        private static readonly string[] AllowedExtensions =
            [".png", ".jpg", ".jpeg", ".gif", ".webp"];

        public SystemConfigurationController(
            ISystemConfigurationService service,
            IStringLocalizer<ValidationMessages> localizer)
            : base(localizer)
        {
            _service = service;
        }

        /// <summary>Obtiene todas las configuraciones del sistema activas.</summary>
        /// <returns>Lista de configuraciones del sistema</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<SystemConfigurationDTO>), 200)]
        public async Task<ActionResult<IEnumerable<SystemConfigurationDTO>>> Get()
        {
            var lang = GetCurrentLanguage();
            return Ok(await _service.GetAllAsync(lang));
        }

        /// <summary>Obtiene una configuración del sistema por su ID.</summary>
        /// <param name="id">ID de la configuración</param>
        /// <returns>Configuración del sistema encontrada</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(SystemConfigurationDTO), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<SystemConfigurationDTO>> Get(int id)
        {
            var lang = GetCurrentLanguage();
            var result = await _service.GetByIdAsync(id, lang);
            return result == null ? NotFound() : Ok(result);
        }

        /// <summary>
        /// Crea una nueva configuración del sistema.
        /// Si el código ya existe y estaba cancelado, lo reactiva.
        /// </summary>
        /// <param name="dto">Datos de la configuración a crear</param>
        /// <returns>Configuración creada o reactivada</returns>
        [HttpPost]
        [ProducesResponseType(typeof(SystemConfigurationDTO), 201)]
        [ProducesResponseType(400)]
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

        /// <summary>Actualiza una configuración del sistema existente.</summary>
        /// <param name="id">ID de la configuración</param>
        /// <param name="dto">Datos actualizados</param>
        /// <returns>Configuración actualizada</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(SystemConfigurationDTO), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Put(int id, [FromBody] SystemConfigurationDTO dto)
        {
            var lang = GetCurrentLanguage();
            var usuario = GetCurrentUser();
            var result = await _service.UpdateAsync(id, dto, usuario, lang);
            return Ok(result);
        }

        /// <summary>Agrega o actualiza una traducción para la configuración del sistema.</summary>
        /// <param name="id">ID de la configuración</param>
        /// <param name="request">Datos de la traducción</param>
        /// <returns>Resultado de la operación</returns>
        [HttpPost("{id}/translations")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
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

        /// <summary>
        /// Sube el logo del negocio.
        /// Se guarda en F:\Images\CompanyLogo (Windows) o /Images/CompanyLogo (Linux).
        /// </summary>
        /// <param name="request">Archivo de imagen y código del sistema</param>
        /// <returns>Nombre del archivo guardado</returns>
        [HttpPost("upload-logo")]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(typeof(LogoUploadResult), 200)]
        [ProducesResponseType(400)]
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

            // ✅ UN LOGO POR NEGOCIO: Usar el código del sistema como nombre base
            var fileName = $"{safeCode}{extension}";
            var filePath = Path.Combine(LogoBasePath, fileName);

            // ✅ Eliminar logo anterior si existe (garantiza un solo logo por negocio)
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
        /// <param name="fileName">Nombre del archivo de logo</param>
        /// <returns>Imagen del logo</returns>
        [HttpGet("logo/{fileName}")]
        [AllowAnonymous]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
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
                ".png"            => "image/png",
                ".jpg" or ".jpeg" => "image/jpeg",
                ".gif"            => "image/gif",
                ".webp"           => "image/webp",
                _                 => "application/octet-stream"
            };

            return PhysicalFile(filePath, contentType);
        }

        // ─── Request types ───────────────────────────────────────────

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
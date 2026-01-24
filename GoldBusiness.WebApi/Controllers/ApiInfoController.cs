using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace GoldBusiness.WebApi.Controllers
{
    [ApiController]
    [Route("api")]
    [AllowAnonymous]
    [ApiExplorerSettings(GroupName = "v2")]
    public class ApiInfoController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _environment;

        public ApiInfoController(IConfiguration configuration, IWebHostEnvironment environment)
        {
            _configuration = configuration;
            _environment = environment;
        }

        /// <summary>
        /// Información general de la API
        /// </summary>
        /// <returns>Metadatos de la API</returns>
        [HttpGet("info")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<object> GetApiInfo()
        {
            // ✅ Obtener el idioma actual del contexto
            var currentLanguage = CultureInfo.CurrentUICulture.Name;

            // ✅ Diccionario con traducciones
            var descriptions = new Dictionary<string, string>
            {
                { "es", "Sistema ERP con soporte multilenguaje para gestión empresarial" },
                { "en", "Multi-language ERP system for business management" },
                { "fr", "Système ERP multilingue pour la gestion d'entreprise" }
            };

            var authenticationTexts = new Dictionary<string, string>
            {
                { "es", "Token JWT Bearer" },
                { "en", "JWT Bearer Token" },
                { "fr", "Jeton JWT Bearer" }
            };

            var documentationTexts = new Dictionary<string, string>
            {
                { "es", "Documentación disponible en" },
                { "en", "Documentation available at" },
                { "fr", "Documentation disponible sur" }
            };

            // ✅ Seleccionar texto según idioma (fallback a español)
            var language = currentLanguage.StartsWith("en") ? "en"
                         : currentLanguage.StartsWith("fr") ? "fr"
                         : "es";

            return Ok(new
            {
                name = "GoldBusiness API",
                version = _configuration["ApiVersion:Name"] ?? "v2.0",
                description = descriptions[language],
                supportedLanguages = new[] { "es", "en", "fr" },
                currentLanguage = language,
                authentication = new
                {
                    type = authenticationTexts[language],
                    scheme = "Bearer",
                    header = "Authorization"
                },
                documentation = new
                {
                    description = documentationTexts[language],
                    url = "/swagger"
                },
                environment = _environment.EnvironmentName,
                timestamp = DateTime.UtcNow
            });
        }
    }
}
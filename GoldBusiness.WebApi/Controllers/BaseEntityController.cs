using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.Globalization;
using System.Linq;

namespace GoldBusiness.WebApi.Controllers
{
    /// <summary>
    /// Controlador base con funcionalidades comunes para todas las entidades.
    /// </summary>
    public abstract class BaseEntityController : ControllerBase
    {
        protected readonly IStringLocalizer<GoldBusiness.Domain.Resources.ValidationMessages> _localizer;

        protected BaseEntityController(
            IStringLocalizer<GoldBusiness.Domain.Resources.ValidationMessages> localizer)
        {
            _localizer = localizer;
        }

        /// <summary>
        /// Obtiene el idioma actual de la request basado en Accept-Language (fallback a CultureInfo).
        /// </summary>
        protected string GetCurrentLanguage()
        {
            var supportedLanguages = new[] { "es", "en", "fr", "de", "pt" };

            try
            {
                if (Request?.Headers != null && Request.Headers.TryGetValue("Accept-Language", out var acceptLangValues))
                {
                    var first = acceptLangValues.ToString().Split(',').Select(s => s.Trim()).FirstOrDefault();
                    if (!string.IsNullOrWhiteSpace(first))
                    {
                        var lang = first.Split('-', System.StringSplitOptions.RemoveEmptyEntries)[0].ToLowerInvariant();
                        if (supportedLanguages.Contains(lang))
                            return lang;
                    }
                }
            }
            catch
            {
                // ignore header parsing errors and fallback to culture
            }

            var currentCulture = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName.ToLowerInvariant();
            return supportedLanguages.Contains(currentCulture) ? currentCulture : "es";
        }

        /// <summary>
        /// Obtiene el usuario actual del contexto.
        /// </summary>
        protected string GetCurrentUser()
        {
            return User?.Identity?.Name ?? "system";
        }

        /// <summary>
        /// Maneja errores de validación de código duplicado.
        /// </summary>
        protected BadRequestObjectResult HandleDuplicateCodeError(string fieldName, string errorMessage)
        {
            ModelState.AddModelError(fieldName, errorMessage);
            return BadRequest(ModelState);
        }

        /// <summary>
        /// Crea respuesta para registro reactivado.
        /// </summary>
        protected OkObjectResult CreateReactivatedResponse<T>(T data, string codigo)
        {
            var message = string.Format(_localizer["RegistroReactivado"].Value, codigo);
            return Ok(new
            {
                Data = data,
                Message = message,
                Reactivated = true
            });
        }

        /// <summary>
        /// Detecta si un registro fue reactivado basándose en las fechas de auditoría.
        /// </summary>
        protected bool WasReactivated(DateTime createdDate, DateTime? modifiedDate)
        {
            if (!modifiedDate.HasValue) return false;
            return (modifiedDate.Value - createdDate).TotalSeconds < 5;
        }
    }
}
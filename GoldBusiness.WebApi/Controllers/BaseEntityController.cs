using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.Globalization;

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
        /// Obtiene el idioma actual de la request basado en Accept-Language.
        /// </summary>
        protected string GetCurrentLanguage()
        {
            var currentCulture = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName.ToLowerInvariant();
            var supportedLanguages = new[] { "es", "en", "fr" };
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
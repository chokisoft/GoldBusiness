using System.ComponentModel.DataAnnotations;
using GoldBusiness.Domain.Resources;

namespace GoldBusiness.Domain.DTOs
{
    /// <summary>
    /// DTO (Data Transfer Object) para el proceso de autenticación/login.
    /// Contiene las credenciales del usuario con validaciones multiidioma.
    /// </summary>
    public class LoginDTO
    {
        /// <summary>
        /// Nombre de usuario para autenticación.
        /// Puede ser el username, email u otro identificador único del usuario.
        /// </summary>
        /// <remarks>
        /// Validaciones aplicadas:
        /// - Obligatorio (Required)
        /// - Máximo 256 caracteres
        /// Los mensajes de error y etiquetas se obtienen de los recursos localizados.
        /// </remarks>
        [Required(
            ErrorMessageResourceType = typeof(ValidationMessages),
            ErrorMessageResourceName = nameof(ValidationMessages.Required)
        )]
        [Display(
            Name = nameof(ValidationMessages.Field_Username),
            ResourceType = typeof(ValidationMessages)
        )]
        [StringLength(256,
            ErrorMessageResourceType = typeof(ValidationMessages),
            ErrorMessageResourceName = nameof(ValidationMessages.StringLengthMax)
        )]
        public string Username { get; set; } = string.Empty;

        /// <summary>
        /// Contraseña del usuario.
        /// Se transmite de forma segura y debe cumplir con las políticas de seguridad.
        /// </summary>
        /// <remarks>
        /// Validaciones aplicadas:
        /// - Obligatorio (Required) - Usa RequiredFemale porque "contraseña" es femenino
        /// - Longitud entre 6 y 100 caracteres
        /// - Tipo de dato Password (oculta el texto en formularios)
        /// Los mensajes de error y etiquetas se obtienen de los recursos localizados.
        /// </remarks>
        [Required(
            ErrorMessageResourceType = typeof(ValidationMessages),
            ErrorMessageResourceName = nameof(ValidationMessages.RequiredFemale)
        )]
        [Display(
            Name = nameof(ValidationMessages.Field_Password),
            ResourceType = typeof(ValidationMessages)
        )]
        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 6,
            ErrorMessageResourceType = typeof(ValidationMessages),
            ErrorMessageResourceName = nameof(ValidationMessages.StringLength)
        )]
        public string Password { get; set; } = string.Empty;
    }
}

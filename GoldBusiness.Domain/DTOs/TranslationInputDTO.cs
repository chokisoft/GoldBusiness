using System.ComponentModel.DataAnnotations;

namespace GoldBusiness.Domain.DTOs
{
    /// <summary>
    /// DTO para entrada de traducciones multiidioma.
    /// Permite agregar/actualizar traducciones en diferentes idiomas.
    /// </summary>
    public class TranslationInputDTO
    {
        /// <summary>
        /// Código ISO 639-1 del idioma.
        /// Ejemplos: "es" (español), "en" (inglés), "fr" (francés).
        /// </summary>
        [Required(ErrorMessage = "El idioma es obligatorio")]
        [Display(Name = "Idioma")]
        [StringLength(5, MinimumLength = 2, ErrorMessage = "El código de idioma debe tener entre 2 y 5 caracteres")]
        [RegularExpression(@"^[a-z]{2}(-[A-Z]{2})?$", ErrorMessage = "Formato inválido. Use: es, en, es-ES, etc.")]
        public string Language { get; set; } = string.Empty;

        /// <summary>
        /// Texto traducido principal (descripción, nombre, etc.).
        /// </summary>
        [Required(ErrorMessage = "El texto traducido es obligatorio")]
        [Display(Name = "Texto Traducido")]
        [StringLength(1024, ErrorMessage = "El texto no puede exceder 1024 caracteres")]
        public string TranslatedText { get; set; } = string.Empty;

        /// <summary>
        /// Texto secundario traducido (características, observaciones) - Opcional.
        /// </summary>
        [Display(Name = "Texto Secundario")]
        [StringLength(1024, ErrorMessage = "El texto secundario no puede exceder 1024 caracteres")]
        public string? SecondaryText { get; set; }
    }
}
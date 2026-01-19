using System.ComponentModel.DataAnnotations;

namespace GoldBusiness.Domain.DTOs
{
    /// <summary>
    /// DTO para Moneda - Tipos de moneda utilizados en el establecimiento.
    /// </summary>
    public class MonedaDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El establecimiento es obligatorio")]
        [Display(Name = "Establecimiento")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un establecimiento válido")]
        public int EstablecimientoId { get; set; }

        /// <summary>
        /// Código ISO 4217 de la moneda (3 caracteres).
        /// Ejemplos: "USD", "EUR", "CUP", "MLC".
        /// </summary>
        [Required(ErrorMessage = "El código es obligatorio")]
        [Display(Name = "Código")]
        [StringLength(3, MinimumLength = 3, ErrorMessage = "El código debe tener exactamente 3 caracteres")]
        [RegularExpression(@"^[A-Z]{3}$", ErrorMessage = "El código debe ser 3 letras mayúsculas (Ejemplo: USD)")]
        public string Codigo { get; set; } = string.Empty;

        [Required(ErrorMessage = "La descripción es obligatoria")]
        [Display(Name = "Descripción")]
        [StringLength(256, ErrorMessage = "La descripción no puede exceder 256 caracteres")]
        public string Descripcion { get; set; } = string.Empty;

        [Display(Name = "Cancelado")]
        public bool Cancelado { get; set; }

        public string CreadoPor { get; set; } = string.Empty;
        public DateTime FechaHoraCreado { get; set; }
        public string? ModificadoPor { get; set; }
        public DateTime? FechaHoraModificado { get; set; }

        public string EstablecimientoDescripcion { get; set; } = string.Empty;

        public string CodigoDescripcion => $"{Codigo} | {Descripcion}";
    }
}
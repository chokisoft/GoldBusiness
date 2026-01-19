using System.ComponentModel.DataAnnotations;

namespace GoldBusiness.Domain.DTOs
{
    /// <summary>
    /// DTO para Línea - Categoría principal de productos.
    /// Nivel superior de clasificación del inventario.
    /// </summary>
    public class LineaDTO
    {
        public int Id { get; set; }

        /// <summary>
        /// Código de la línea (2 caracteres).
        /// Ejemplo: "01" = Alimentos, "02" = Bebidas.
        /// </summary>
        [Required(ErrorMessage = "El código es obligatorio")]
        [Display(Name = "Código")]
        [StringLength(2, MinimumLength = 2, ErrorMessage = "El código debe tener exactamente 2 caracteres")]
        [RegularExpression(@"^\d{2}$", ErrorMessage = "El código debe ser numérico (00-99)")]
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

        /// <summary>
        /// Cantidad de sublíneas.
        /// </summary>
        [Display(Name = "Sublíneas")]
        public int CantidadSubLineas { get; set; }

        public string CodigoDescripcion => $"{Codigo} | {Descripcion}";
    }
}
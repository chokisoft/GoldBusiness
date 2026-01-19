using System.ComponentModel.DataAnnotations;

namespace GoldBusiness.Domain.DTOs
{
    /// <summary>
    /// DTO para SubLínea - Subcategoría de productos dentro de una línea.
    /// </summary>
    public class SubLineaDTO
    {
        public int Id { get; set; }

        /// <summary>
        /// Código de la sublínea (formato: ##-##).
        /// Ejemplo: "01-01" = Alimentos - Lácteos.
        /// </summary>
        [Required(ErrorMessage = "El código es obligatorio")]
        [Display(Name = "Código")]
        [StringLength(5, MinimumLength = 5, ErrorMessage = "El código debe tener exactamente 5 caracteres")]
        [RegularExpression(@"^\d{2}-\d{2}$", ErrorMessage = "Formato debe ser ##-## (Ejemplo: 01-01)")]
        public string Codigo { get; set; } = string.Empty;

        [Required(ErrorMessage = "La descripción es obligatoria")]
        [Display(Name = "Descripción")]
        [StringLength(256, ErrorMessage = "La descripción no puede exceder 256 caracteres")]
        public string Descripcion { get; set; } = string.Empty;

        [Required(ErrorMessage = "La línea es obligatoria")]
        [Display(Name = "Línea")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar una línea válida")]
        public int LineaId { get; set; }

        [Display(Name = "Cancelado")]
        public bool Cancelado { get; set; }

        public string CreadoPor { get; set; } = string.Empty;
        public DateTime FechaHoraCreado { get; set; }
        public string? ModificadoPor { get; set; }
        public DateTime? FechaHoraModificado { get; set; }

        public string LineaCodigo { get; set; } = string.Empty;
        public string LineaDescripcion { get; set; } = string.Empty;

        /// <summary>
        /// Cantidad de productos.
        /// </summary>
        [Display(Name = "Productos")]
        public int CantidadProductos { get; set; }

        public string CodigoDescripcion => $"{Codigo} | {Descripcion}";
    }
}
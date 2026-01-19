using System.ComponentModel.DataAnnotations;

namespace GoldBusiness.Domain.DTOs
{
    /// <summary>
    /// DTO para UnidadMedida - Unidad de medida para productos.
    /// Ejemplos: KG, LT, UND, MT.
    /// </summary>
    public class UnidadMedidaDTO
    {
        public int Id { get; set; }

        /// <summary>
        /// Código de la unidad (3 caracteres).
        /// </summary>
        [Required(ErrorMessage = "El código es obligatorio")]
        [Display(Name = "Código")]
        [StringLength(3, MinimumLength = 3, ErrorMessage = "El código debe tener exactamente 3 caracteres")]
        [RegularExpression(@"^[A-Z]{2,3}$", ErrorMessage = "El código debe ser 2-3 letras mayúsculas")]
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
        /// Cantidad de productos.
        /// </summary>
        [Display(Name = "Productos")]
        public int CantidadProductos { get; set; }

        public string CodigoDescripcion => $"{Codigo} | {Descripcion}";
    }
}
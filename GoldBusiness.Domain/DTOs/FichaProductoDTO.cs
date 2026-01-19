using System.ComponentModel.DataAnnotations;

namespace GoldBusiness.Domain.DTOs
{
    /// <summary>
    /// DTO para FichaProducto - Receta o BOM (Bill of Materials).
    /// Define los componentes necesarios para ensamblar un producto.
    /// </summary>
    public class FichaProductoDTO
    {
        public int Id { get; set; }

        /// <summary>
        /// ID del producto final (producto ensamblado).
        /// </summary>
        [Required(ErrorMessage = "El producto es obligatorio")]
        [Display(Name = "Producto")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un producto válido")]
        public int ProductoId { get; set; }

        /// <summary>
        /// ID del componente/ingrediente.
        /// </summary>
        [Required(ErrorMessage = "El componente es obligatorio")]
        [Display(Name = "Componente")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un componente válido")]
        public int ComponenteId { get; set; }

        [Required(ErrorMessage = "La localidad es obligatoria")]
        [Display(Name = "Localidad")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar una localidad válida")]
        public int LocalidadId { get; set; }

        /// <summary>
        /// Cantidad del componente necesaria.
        /// </summary>
        [Required(ErrorMessage = "La cantidad es obligatoria")]
        [Display(Name = "Cantidad")]
        [Range(0.001, double.MaxValue, ErrorMessage = "La cantidad debe ser mayor que cero")]
        [DisplayFormat(DataFormatString = "{0:N3}", ApplyFormatInEditMode = false)]
        public decimal Cantidad { get; set; }

        public string CreadoPor { get; set; } = string.Empty;
        public DateTime FechaHoraCreado { get; set; }
        public string? ModificadoPor { get; set; }
        public DateTime? FechaHoraModificado { get; set; }

        /// <summary>
        /// Datos de navegación.
        /// </summary>
        public string ProductoCodigo { get; set; } = string.Empty;
        public string ProductoDescripcion { get; set; } = string.Empty;
        public string ComponenteCodigo { get; set; } = string.Empty;
        public string ComponenteDescripcion { get; set; } = string.Empty;
        public string LocalidadDescripcion { get; set; } = string.Empty;
        public string UnidadMedida { get; set; } = string.Empty;
    }
}
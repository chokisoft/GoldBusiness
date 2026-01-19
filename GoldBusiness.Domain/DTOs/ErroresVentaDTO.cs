using System.ComponentModel.DataAnnotations;

namespace GoldBusiness.Domain.DTOs
{
    /// <summary>
    /// DTO para ErroresVenta - Registro de errores de venta corregidos.
    /// Almacena ajustes por errores en operaciones de venta.
    /// </summary>
    public class ErroresVentaDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El detalle de operación es obligatorio")]
        [Display(Name = "Detalle Operación")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un detalle válido")]
        public int OperacionesDetalleId { get; set; }

        [Required(ErrorMessage = "La localidad es obligatoria")]
        [Display(Name = "Localidad")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar una localidad válida")]
        public int LocalidadId { get; set; }

        [Required(ErrorMessage = "El producto es obligatorio")]
        [Display(Name = "Producto")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un producto válido")]
        public int ProductoId { get; set; }

        [Required(ErrorMessage = "La cantidad es obligatoria")]
        [Display(Name = "Cantidad")]
        [Range(0.01, double.MaxValue, ErrorMessage = "La cantidad debe ser mayor que cero")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = false)]
        public decimal Cantidad { get; set; }

        [Required(ErrorMessage = "El costo es obligatorio")]
        [Display(Name = "Costo")]
        [DataType(DataType.Currency)]
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        [Range(0, double.MaxValue, ErrorMessage = "El costo debe ser mayor o igual a cero")]
        public decimal Costo { get; set; }

        [Display(Name = "Importe Costo")]
        [DataType(DataType.Currency)]
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        public decimal ImporteCosto { get; set; }

        public string CreadoPor { get; set; } = string.Empty;
        public DateTime FechaHoraCreado { get; set; }
        public string? ModificadoPor { get; set; }
        public DateTime? FechaHoraModificado { get; set; }

        /// <summary>
        /// Datos de navegación.
        /// </summary>
        public string LocalidadDescripcion { get; set; } = string.Empty;
        public string ProductoCodigo { get; set; } = string.Empty;
        public string ProductoDescripcion { get; set; } = string.Empty;
        public string NoDocumento { get; set; } = string.Empty;
        public DateTime FechaOperacion { get; set; }
    }
}
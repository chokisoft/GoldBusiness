using System.ComponentModel.DataAnnotations;

namespace GoldBusiness.Domain.DTOs
{
    /// <summary>
    /// DTO para OperacionesDetalle - Línea de detalle de una operación de inventario.
    /// Registra movimientos de productos por localidad.
    /// </summary>
    public class OperacionesDetalleDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "La operación es obligatoria")]
        [Display(Name = "Operación")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar una operación válida")]
        public int OperacionesEncabezadoId { get; set; }

        [Required(ErrorMessage = "El producto es obligatorio")]
        [Display(Name = "Producto")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un producto válido")]
        public int ProductoId { get; set; }

        [Required(ErrorMessage = "La localidad es obligatoria")]
        [Display(Name = "Localidad")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar una localidad válida")]
        public int LocalidadId { get; set; }

        [Required(ErrorMessage = "La cantidad es obligatoria")]
        [Display(Name = "Cantidad")]
        [Range(0.001, double.MaxValue, ErrorMessage = "La cantidad debe ser mayor que cero")]
        [DisplayFormat(DataFormatString = "{0:N3}", ApplyFormatInEditMode = false)]
        public decimal Cantidad { get; set; }

        [Display(Name = "Costo")]
        [DataType(DataType.Currency)]
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        [Range(0, double.MaxValue, ErrorMessage = "El costo debe ser mayor o igual a cero")]
        public decimal Costo { get; set; }

        [Display(Name = "Importe Costo")]
        [DataType(DataType.Currency)]
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        public decimal ImporteCosto { get; set; }

        [Display(Name = "Venta")]
        [DataType(DataType.Currency)]
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        [Range(0, double.MaxValue, ErrorMessage = "El precio debe ser mayor o igual a cero")]
        public decimal Venta { get; set; }

        [Display(Name = "Importe Venta")]
        [DataType(DataType.Currency)]
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        public decimal ImporteVenta { get; set; }

        [Display(Name = "Existencia")]
        [DisplayFormat(DataFormatString = "{0:N3}", ApplyFormatInEditMode = false)]
        public decimal Existencia { get; set; }

        [Display(Name = "Cancelado")]
        public bool Cancelado { get; set; }

        public string CreadoPor { get; set; } = string.Empty;
        public DateTime FechaHoraCreado { get; set; }
        public string? ModificadoPor { get; set; }
        public DateTime? FechaHoraModificado { get; set; }

        /// <summary>
        /// Datos de navegación.
        /// </summary>
        public string ProductoCodigo { get; set; } = string.Empty;
        public string ProductoDescripcion { get; set; } = string.Empty;
        public string LocalidadDescripcion { get; set; } = string.Empty;
        public string UnidadMedida { get; set; } = string.Empty;
    }
}
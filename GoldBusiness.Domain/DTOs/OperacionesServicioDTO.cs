using System.ComponentModel.DataAnnotations;

namespace GoldBusiness.Domain.DTOs
{
    /// <summary>
    /// DTO para OperacionesServicio - Componentes de servicios en operaciones.
    /// Registra los insumos consumidos al prestar un servicio.
    /// </summary>
    public class OperacionesServicioDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El detalle de operación es obligatorio")]
        [Display(Name = "Detalle Operación")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un detalle válido")]
        public int OperacionesDetalleId { get; set; }

        [Required(ErrorMessage = "El producto es obligatorio")]
        [Display(Name = "Producto")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un producto válido")]
        public int ProductoId { get; set; }

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

        [Display(Name = "Precio Venta")]
        [DataType(DataType.Currency)]
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        [Range(0, double.MaxValue, ErrorMessage = "El precio de venta debe ser mayor o igual a cero")]
        public decimal Venta { get; set; }

        [Display(Name = "Importe Venta")]
        [DataType(DataType.Currency)]
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        public decimal ImporteVenta { get; set; }

        public string CreadoPor { get; set; } = string.Empty;
        public DateTime FechaHoraCreado { get; set; }
        public string? ModificadoPor { get; set; }
        public DateTime? FechaHoraModificado { get; set; }

        /// <summary>
        /// Datos de navegación.
        /// </summary>
        public string ProductoCodigo { get; set; } = string.Empty;
        public string ProductoDescripcion { get; set; } = string.Empty;
        public string UnidadMedida { get; set; } = string.Empty;

        [Display(Name = "Margen Bruto")]
        [DataType(DataType.Currency)]
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        public decimal MargenBruto => ImporteVenta - ImporteCosto;

        [Display(Name = "% Margen")]
        [DisplayFormat(DataFormatString = "{0:N2}%", ApplyFormatInEditMode = false)]
        public decimal PorcentajeMargen
        {
            get
            {
                if (ImporteCosto == 0 || Costo == 0) return 0;
                return ((ImporteVenta - ImporteCosto) / ImporteCosto) * 100;
            }
        }
    }
}
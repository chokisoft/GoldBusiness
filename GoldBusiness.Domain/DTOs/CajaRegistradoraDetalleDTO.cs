using System.ComponentModel.DataAnnotations;

namespace GoldBusiness.Domain.DTOs
{
    /// <summary>
    /// DTO para CajaRegistradoraDetalle - Línea de producto en una orden POS.
    /// Registra cada artículo vendido en una caja/mesa.
    /// </summary>
    public class CajaRegistradoraDetalleDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "La caja registradora es obligatoria")]
        [Display(Name = "Caja Registradora")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar una caja válida")]
        public int CajaRegistradoraId { get; set; }

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
        [Range(0.001, double.MaxValue, ErrorMessage = "La cantidad debe ser mayor que cero")]
        [DisplayFormat(DataFormatString = "{0:N3}", ApplyFormatInEditMode = false)]
        public decimal Cantidad { get; set; }

        [Required(ErrorMessage = "El precio es obligatorio")]
        [Display(Name = "Precio Venta")]
        [DataType(DataType.Currency)]
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        [Range(0, double.MaxValue, ErrorMessage = "El precio debe ser mayor o igual a cero")]
        public decimal Venta { get; set; }

        [Display(Name = "Importe Venta")]
        [DataType(DataType.Currency)]
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        public decimal ImporteVenta { get; set; }

        public string CreadoPor { get; set; } = string.Empty;
        public DateTime FechaHoraCreado { get; set; }

        /// <summary>
        /// Datos de navegación.
        /// </summary>
        public string LocalidadDescripcion { get; set; } = string.Empty;
        public string ProductoCodigo { get; set; } = string.Empty;
        public string ProductoDescripcion { get; set; } = string.Empty;
        public string UnidadMedida { get; set; } = string.Empty;
    }
}
using System.ComponentModel.DataAnnotations;

namespace GoldBusiness.Domain.DTOs
{
    /// <summary>
    /// DTO para Producto - Artículo o servicio del inventario.
    /// Contiene información de precios, stock y clasificación.
    /// </summary>
    public class ProductoDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El establecimiento es obligatorio")]
        [Display(Name = "Establecimiento")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un establecimiento válido")]
        public int EstablecimientoId { get; set; }

        /// <summary>
        /// Código único del producto (hasta 13 caracteres para códigos de barras).
        /// </summary>
        [Required(ErrorMessage = "El código es obligatorio")]
        [Display(Name = "Código")]
        [StringLength(13, ErrorMessage = "El código no puede exceder 13 caracteres")]
        public string Codigo { get; set; } = string.Empty;

        [Required(ErrorMessage = "La descripción es obligatoria")]
        [Display(Name = "Descripción")]
        [StringLength(256, ErrorMessage = "La descripción no puede exceder 256 caracteres")]
        public string Descripcion { get; set; } = string.Empty;

        [Required(ErrorMessage = "La unidad de medida es obligatoria")]
        [Display(Name = "Unidad de Medida")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar una unidad válida")]
        public int UnidadMedidaId { get; set; }

        [Required(ErrorMessage = "El proveedor es obligatorio")]
        [Display(Name = "Proveedor")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un proveedor válido")]
        public int ProveedorId { get; set; }

        [Required(ErrorMessage = "El precio de venta es obligatorio")]
        [Display(Name = "Precio Venta")]
        [DataType(DataType.Currency)]
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        [Range(0, double.MaxValue, ErrorMessage = "El precio debe ser mayor o igual a cero")]
        public decimal PrecioVenta { get; set; }

        [Required(ErrorMessage = "El precio de costo es obligatorio")]
        [Display(Name = "Precio Costo")]
        [DataType(DataType.Currency)]
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        [Range(0, double.MaxValue, ErrorMessage = "El costo debe ser mayor o igual a cero")]
        public decimal PrecioCosto { get; set; }

        /// <summary>
        /// Porcentaje de IVA aplicable.
        /// </summary>
        [Display(Name = "IVA (%)")]
        [Range(-0.01, 99.99, ErrorMessage = "El IVA debe estar entre -0.01 y 99.99")]
        [DisplayFormat(DataFormatString = "{0:N2}%", ApplyFormatInEditMode = false)]
        public decimal Iva { get; set; }

        [Display(Name = "Código Referencia")]
        [StringLength(50, ErrorMessage = "El código de referencia no puede exceder 50 caracteres")]
        public string? CodigoReferencia { get; set; }

        [Display(Name = "Stock Mínimo")]
        [Range(0, double.MaxValue, ErrorMessage = "El stock mínimo debe ser mayor o igual a cero")]
        public decimal StockMinimo { get; set; }

        /// <summary>
        /// Indica si es un servicio (no maneja inventario físico).
        /// </summary>
        [Display(Name = "Es Servicio")]
        public bool Servicio { get; set; }

        [Required(ErrorMessage = "La sublínea es obligatoria")]
        [Display(Name = "Sublínea")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar una sublínea válida")]
        public int SubLineaId { get; set; }

        [Display(Name = "Imagen")]
        public byte[]? Imagen { get; set; }

        [Display(Name = "Características")]
        [StringLength(1024, ErrorMessage = "Las características no pueden exceder 1024 caracteres")]
        [DataType(DataType.MultilineText)]
        public string? Caracteristicas { get; set; }

        [Display(Name = "Cancelado")]
        public bool Cancelado { get; set; }

        public string CreadoPor { get; set; } = string.Empty;
        public DateTime FechaHoraCreado { get; set; }
        public string? ModificadoPor { get; set; }
        public DateTime? FechaHoraModificado { get; set; }

        /// <summary>
        /// Datos de navegación.
        /// </summary>
        public string EstablecimientoDescripcion { get; set; } = string.Empty;
        public string UnidadMedidaCodigo { get; set; } = string.Empty;
        public string ProveedorDescripcion { get; set; } = string.Empty;
        public string SubLineaDescripcion { get; set; } = string.Empty;
        public string LineaDescripcion { get; set; } = string.Empty;

        /// <summary>
        /// Propiedades calculadas.
        /// </summary>
        public decimal PrecioVentaConIva => PrecioVenta * (1 + (Iva / 100));
        public decimal MargenBeneficio => PrecioCosto > 0 ? ((PrecioVenta - PrecioCosto) / PrecioCosto) * 100 : 0;
        public bool TieneImagen => Imagen != null && Imagen.Length > 0;
        public string TipoProducto => Servicio ? "Servicio" : "Producto";
        public string CodigoDescripcion => $"{Codigo} | {Descripcion}";
    }
}
using System.ComponentModel.DataAnnotations;

namespace GoldBusiness.Domain.DTOs
{
    /// <summary>
    /// DTO para Producto - Artículo o servicio del inventario.
    /// </summary>
    public class ProductoDTO
    {
        public int Id { get; set; }

        [Required(
            ErrorMessageResourceType = typeof(GoldBusiness.Domain.Resources.ValidationMessages),
            ErrorMessageResourceName = nameof(GoldBusiness.Domain.Resources.ValidationMessages.EstablecimientoObligatorio)
        )]
        [Display(
            Name = nameof(GoldBusiness.Domain.Resources.ValidationMessages.Field_Establecimiento),
            ResourceType = typeof(GoldBusiness.Domain.Resources.ValidationMessages)
        )]
        [Range(1, int.MaxValue,
            ErrorMessageResourceType = typeof(GoldBusiness.Domain.Resources.ValidationMessages),
            ErrorMessageResourceName = nameof(GoldBusiness.Domain.Resources.ValidationMessages.EstablecimientoSeleccion)
        )]
        public int EstablecimientoId { get; set; }

        public string EstablecimientoCodigo { get; set; } = string.Empty;
        public string EstablecimientoDescripcion { get; set; } = string.Empty;

        [Required(
            ErrorMessageResourceType = typeof(GoldBusiness.Domain.Resources.ValidationMessages),
            ErrorMessageResourceName = nameof(GoldBusiness.Domain.Resources.ValidationMessages.CodigoObligatorio)
        )]
        [Display(
            Name = nameof(GoldBusiness.Domain.Resources.ValidationMessages.Field_Codigo),
            ResourceType = typeof(GoldBusiness.Domain.Resources.ValidationMessages)
        )]
        [StringLength(13,
            ErrorMessageResourceType = typeof(GoldBusiness.Domain.Resources.ValidationMessages),
            ErrorMessageResourceName = nameof(GoldBusiness.Domain.Resources.ValidationMessages.ProductoCodigoLongitud)
        )]
        public string Codigo { get; set; } = string.Empty;

        [Required(
            ErrorMessageResourceType = typeof(GoldBusiness.Domain.Resources.ValidationMessages),
            ErrorMessageResourceName = nameof(GoldBusiness.Domain.Resources.ValidationMessages.DescripcionObligatoria)
        )]
        [Display(
            Name = nameof(GoldBusiness.Domain.Resources.ValidationMessages.Field_Descripcion),
            ResourceType = typeof(GoldBusiness.Domain.Resources.ValidationMessages)
        )]
        [StringLength(256,
            ErrorMessageResourceType = typeof(GoldBusiness.Domain.Resources.ValidationMessages),
            ErrorMessageResourceName = nameof(GoldBusiness.Domain.Resources.ValidationMessages.DescripcionLongitud)
        )]
        public string Descripcion { get; set; } = string.Empty;

        [Required(
            ErrorMessageResourceType = typeof(GoldBusiness.Domain.Resources.ValidationMessages),
            ErrorMessageResourceName = nameof(GoldBusiness.Domain.Resources.ValidationMessages.UnidadMedidaObligatoria)
        )]
        [Display(
            Name = nameof(GoldBusiness.Domain.Resources.ValidationMessages.Field_UnidadMedida),
            ResourceType = typeof(GoldBusiness.Domain.Resources.ValidationMessages)
        )]
        [Range(1, int.MaxValue,
            ErrorMessageResourceType = typeof(GoldBusiness.Domain.Resources.ValidationMessages),
            ErrorMessageResourceName = nameof(GoldBusiness.Domain.Resources.ValidationMessages.UnidadMedidaSeleccion)
        )]
        public int UnidadMedidaId { get; set; }

        public string UnidadMedidaCodigo { get; set; } = string.Empty;
        public string UnidadMedidaDescripcion { get; set; } = string.Empty;

        [Required(
            ErrorMessageResourceType = typeof(GoldBusiness.Domain.Resources.ValidationMessages),
            ErrorMessageResourceName = nameof(GoldBusiness.Domain.Resources.ValidationMessages.ProveedorObligatorio)
        )]
        [Display(
            Name = nameof(GoldBusiness.Domain.Resources.ValidationMessages.Field_Proveedor),
            ResourceType = typeof(GoldBusiness.Domain.Resources.ValidationMessages)
        )]
        [Range(1, int.MaxValue,
            ErrorMessageResourceType = typeof(GoldBusiness.Domain.Resources.ValidationMessages),
            ErrorMessageResourceName = nameof(GoldBusiness.Domain.Resources.ValidationMessages.ProveedorSeleccion)
        )]
        public int ProveedorId { get; set; }

        public string ProveedorCodigo { get; set; } = string.Empty;
        public string ProveedorDescripcion { get; set; } = string.Empty;

        [Required(
            ErrorMessageResourceType = typeof(GoldBusiness.Domain.Resources.ValidationMessages),
            ErrorMessageResourceName = nameof(GoldBusiness.Domain.Resources.ValidationMessages.PrecioVentaObligatorio)
        )]
        [Display(
            Name = nameof(GoldBusiness.Domain.Resources.ValidationMessages.Field_PrecioVenta),
            ResourceType = typeof(GoldBusiness.Domain.Resources.ValidationMessages)
        )]
        [Range(0, double.MaxValue,
            ErrorMessageResourceType = typeof(GoldBusiness.Domain.Resources.ValidationMessages),
            ErrorMessageResourceName = nameof(GoldBusiness.Domain.Resources.ValidationMessages.PrecioVentaRango)
        )]
        public decimal PrecioVenta { get; set; }

        [Required(
            ErrorMessageResourceType = typeof(GoldBusiness.Domain.Resources.ValidationMessages),
            ErrorMessageResourceName = nameof(GoldBusiness.Domain.Resources.ValidationMessages.PrecioCostoObligatorio)
        )]
        [Display(
            Name = nameof(GoldBusiness.Domain.Resources.ValidationMessages.Field_PrecioCosto),
            ResourceType = typeof(GoldBusiness.Domain.Resources.ValidationMessages)
        )]
        [Range(0, double.MaxValue,
            ErrorMessageResourceType = typeof(GoldBusiness.Domain.Resources.ValidationMessages),
            ErrorMessageResourceName = nameof(GoldBusiness.Domain.Resources.ValidationMessages.PrecioCostoRango)
        )]
        public decimal PrecioCosto { get; set; }

        [Display(
            Name = nameof(GoldBusiness.Domain.Resources.ValidationMessages.Field_Iva),
            ResourceType = typeof(GoldBusiness.Domain.Resources.ValidationMessages)
        )]
        [Range(-0.01, 99.99,
            ErrorMessageResourceType = typeof(GoldBusiness.Domain.Resources.ValidationMessages),
            ErrorMessageResourceName = nameof(GoldBusiness.Domain.Resources.ValidationMessages.IvaRango)
        )]
        public decimal Iva { get; set; }

        [Display(
            Name = nameof(GoldBusiness.Domain.Resources.ValidationMessages.Field_CodigoReferencia),
            ResourceType = typeof(GoldBusiness.Domain.Resources.ValidationMessages)
        )]
        [StringLength(50,
            ErrorMessageResourceType = typeof(GoldBusiness.Domain.Resources.ValidationMessages),
            ErrorMessageResourceName = nameof(GoldBusiness.Domain.Resources.ValidationMessages.CodigoReferenciaLongitud)
        )]
        public string CodigoReferencia { get; set; } = string.Empty;

        [Display(
            Name = nameof(GoldBusiness.Domain.Resources.ValidationMessages.Field_StockMinimo),
            ResourceType = typeof(GoldBusiness.Domain.Resources.ValidationMessages)
        )]
        [Range(0, double.MaxValue,
            ErrorMessageResourceType = typeof(GoldBusiness.Domain.Resources.ValidationMessages),
            ErrorMessageResourceName = nameof(GoldBusiness.Domain.Resources.ValidationMessages.StockMinimoRango)
        )]
        public decimal StockMinimo { get; set; }

        [Display(
            Name = nameof(GoldBusiness.Domain.Resources.ValidationMessages.Field_Servicio),
            ResourceType = typeof(GoldBusiness.Domain.Resources.ValidationMessages)
        )]
        public bool Servicio { get; set; }

        [Required(
            ErrorMessageResourceType = typeof(GoldBusiness.Domain.Resources.ValidationMessages),
            ErrorMessageResourceName = nameof(GoldBusiness.Domain.Resources.ValidationMessages.SubLineaObligatoria)
        )]
        [Display(
            Name = nameof(GoldBusiness.Domain.Resources.ValidationMessages.Field_Linea),
            ResourceType = typeof(GoldBusiness.Domain.Resources.ValidationMessages)
        )]
        [Range(1, int.MaxValue,
            ErrorMessageResourceType = typeof(GoldBusiness.Domain.Resources.ValidationMessages),
            ErrorMessageResourceName = nameof(GoldBusiness.Domain.Resources.ValidationMessages.SubLineaSeleccion)
        )]
        public int SubLineaId { get; set; }

        public string SubLineaCodigo { get; set; } = string.Empty;
        public string SubLineaDescripcion { get; set; } = string.Empty;
        public string LineaCodigo { get; set; } = string.Empty;
        public string LineaDescripcion { get; set; } = string.Empty;

        [Display(
            Name = nameof(GoldBusiness.Domain.Resources.ValidationMessages.Field_Imagen),
            ResourceType = typeof(GoldBusiness.Domain.Resources.ValidationMessages)
        )]
        public byte[]? Imagen { get; set; }

        [Display(
            Name = nameof(GoldBusiness.Domain.Resources.ValidationMessages.Field_Caracteristicas),
            ResourceType = typeof(GoldBusiness.Domain.Resources.ValidationMessages)
        )]
        [StringLength(1024,
            ErrorMessageResourceType = typeof(GoldBusiness.Domain.Resources.ValidationMessages),
            ErrorMessageResourceName = nameof(GoldBusiness.Domain.Resources.ValidationMessages.CaracteristicasLongitud)
        )]
        public string Caracteristicas { get; set; } = string.Empty;

        [Display(
            Name = nameof(GoldBusiness.Domain.Resources.ValidationMessages.Field_Cancelado),
            ResourceType = typeof(GoldBusiness.Domain.Resources.ValidationMessages)
        )]
        public bool Cancelado { get; set; }

        public string CreadoPor { get; set; } = string.Empty;
        public DateTime FechaHoraCreado { get; set; }
        public string? ModificadoPor { get; set; }
        public DateTime? FechaHoraModificado { get; set; }
    }
}
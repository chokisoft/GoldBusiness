using System.ComponentModel.DataAnnotations;

namespace GoldBusiness.Domain.DTOs
{
    /// <summary>
    /// DTO para FichaProducto - Composición de productos (BOM - Bill of Materials).
    /// </summary>
    public class FichaProductoDTO
    {
        public int Id { get; set; }

        [Required(
            ErrorMessageResourceType = typeof(GoldBusiness.Domain.Resources.ValidationMessages),
            ErrorMessageResourceName = nameof(GoldBusiness.Domain.Resources.ValidationMessages.ProductoIdObligatorio)
        )]
        [Display(
            Name = nameof(GoldBusiness.Domain.Resources.ValidationMessages.Field_Producto),
            ResourceType = typeof(GoldBusiness.Domain.Resources.ValidationMessages)
        )]
        public int ProductoId { get; set; }

        public string ProductoCodigo { get; set; } = string.Empty;
        public string ProductoDescripcion { get; set; } = string.Empty;

        [Required(
            ErrorMessageResourceType = typeof(GoldBusiness.Domain.Resources.ValidationMessages),
            ErrorMessageResourceName = nameof(GoldBusiness.Domain.Resources.ValidationMessages.LocalidadObligatoria)
        )]
        [Display(
            Name = nameof(GoldBusiness.Domain.Resources.ValidationMessages.Field_Localidad),
            ResourceType = typeof(GoldBusiness.Domain.Resources.ValidationMessages)
        )]
        [Range(1, int.MaxValue,
            ErrorMessageResourceType = typeof(GoldBusiness.Domain.Resources.ValidationMessages),
            ErrorMessageResourceName = nameof(GoldBusiness.Domain.Resources.ValidationMessages.LocalidadSeleccion)
        )]
        public int LocalidadId { get; set; }

        public string LocalidadCodigo { get; set; } = string.Empty;
        public string LocalidadDescripcion { get; set; } = string.Empty;

        [Required(
            ErrorMessageResourceType = typeof(GoldBusiness.Domain.Resources.ValidationMessages),
            ErrorMessageResourceName = nameof(GoldBusiness.Domain.Resources.ValidationMessages.ComponenteIdObligatorio)
        )]
        [Display(
            Name = nameof(GoldBusiness.Domain.Resources.ValidationMessages.Field_Componente),
            ResourceType = typeof(GoldBusiness.Domain.Resources.ValidationMessages)
        )]
        [Range(1, int.MaxValue,
            ErrorMessageResourceType = typeof(GoldBusiness.Domain.Resources.ValidationMessages),
            ErrorMessageResourceName = nameof(GoldBusiness.Domain.Resources.ValidationMessages.ComponenteSeleccion)
        )]
        public int ComponenteId { get; set; }

        public string ComponenteCodigo { get; set; } = string.Empty;
        public string ComponenteDescripcion { get; set; } = string.Empty;

        [Required(
            ErrorMessageResourceType = typeof(GoldBusiness.Domain.Resources.ValidationMessages),
            ErrorMessageResourceName = nameof(GoldBusiness.Domain.Resources.ValidationMessages.CantidadObligatoria)
        )]
        [Display(
            Name = nameof(GoldBusiness.Domain.Resources.ValidationMessages.Field_Cantidad),
            ResourceType = typeof(GoldBusiness.Domain.Resources.ValidationMessages)
        )]
        [Range(0.01, double.MaxValue,
            ErrorMessageResourceType = typeof(GoldBusiness.Domain.Resources.ValidationMessages),
            ErrorMessageResourceName = nameof(GoldBusiness.Domain.Resources.ValidationMessages.CantidadRango)
        )]
        public decimal Cantidad { get; set; }

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
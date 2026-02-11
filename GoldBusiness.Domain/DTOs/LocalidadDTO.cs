using System.ComponentModel.DataAnnotations;

namespace GoldBusiness.Domain.DTOs
{
    /// <summary>
    /// DTO para Localidad - Ubicación física dentro de un establecimiento.
    /// </summary>
    public class LocalidadDTO
    {
        public int Id { get; set; }

        [Required(
            ErrorMessageResourceType = typeof(GoldBusiness.Domain.Resources.ValidationMessages),
            ErrorMessageResourceName = nameof(GoldBusiness.Domain.Resources.ValidationMessages.EstablecimientoIdObligatorio)
        )]
        public int EstablecimientoId { get; set; }

        public string EstablecimientoCodigo { get; set; } = string.Empty;
        public string EstablecimientoDescripcion { get; set; } = string.Empty;

        /// <summary>
        /// Código de la localidad (9 caracteres).
        /// </summary>
        [Required(
            ErrorMessageResourceType = typeof(GoldBusiness.Domain.Resources.ValidationMessages),
            ErrorMessageResourceName = nameof(GoldBusiness.Domain.Resources.ValidationMessages.CodigoObligatorio)
        )]
        [Display(
            Name = nameof(GoldBusiness.Domain.Resources.ValidationMessages.Field_Codigo),
            ResourceType = typeof(GoldBusiness.Domain.Resources.ValidationMessages)
        )]
        [StringLength(9, MinimumLength = 9,
            ErrorMessageResourceType = typeof(GoldBusiness.Domain.Resources.ValidationMessages),
            ErrorMessageResourceName = nameof(GoldBusiness.Domain.Resources.ValidationMessages.LocalidadCodigoLongitud)
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

        public bool Almacen { get; set; }

        [Required(
            ErrorMessageResourceType = typeof(GoldBusiness.Domain.Resources.ValidationMessages),
            ErrorMessageResourceName = nameof(GoldBusiness.Domain.Resources.ValidationMessages.CuentaInventarioIdObligatorio)
        )]
        public int CuentaInventarioId { get; set; }

        public string CuentaInventarioCodigo { get; set; } = string.Empty;
        public string CuentaInventarioDescripcion { get; set; } = string.Empty;

        [Required(
            ErrorMessageResourceType = typeof(GoldBusiness.Domain.Resources.ValidationMessages),
            ErrorMessageResourceName = nameof(GoldBusiness.Domain.Resources.ValidationMessages.CuentaCostoIdObligatorio)
        )]
        public int CuentaCostoId { get; set; }

        public string CuentaCostoCodigo { get; set; } = string.Empty;
        public string CuentaCostoDescripcion { get; set; } = string.Empty;

        [Required(
            ErrorMessageResourceType = typeof(GoldBusiness.Domain.Resources.ValidationMessages),
            ErrorMessageResourceName = nameof(GoldBusiness.Domain.Resources.ValidationMessages.CuentaVentaIdObligatorio)
        )]
        public int CuentaVentaId { get; set; }

        public string CuentaVentaCodigo { get; set; } = string.Empty;
        public string CuentaVentaDescripcion { get; set; } = string.Empty;

        [Required(
            ErrorMessageResourceType = typeof(GoldBusiness.Domain.Resources.ValidationMessages),
            ErrorMessageResourceName = nameof(GoldBusiness.Domain.Resources.ValidationMessages.CuentaDevolucionIdObligatorio)
        )]
        public int CuentaDevolucionId { get; set; }

        public string CuentaDevolucionCodigo { get; set; } = string.Empty;
        public string CuentaDevolucionDescripcion { get; set; } = string.Empty;

        public bool Activo { get; set; }
        public bool Cancelado { get; set; }

        public string CreadoPor { get; set; } = string.Empty;
        public DateTime FechaHoraCreado { get; set; }
        public string? ModificadoPor { get; set; }
        public DateTime? FechaHoraModificado { get; set; }
    }
}
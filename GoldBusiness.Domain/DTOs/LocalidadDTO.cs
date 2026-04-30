using System.ComponentModel.DataAnnotations;
using GoldBusiness.Domain.Enums;

namespace GoldBusiness.Domain.DTOs
{
    /// <summary>
    /// DTO para Localidad - Ubicación operativa dentro de un establecimiento.
    /// Basado en estándares ERP (SAP Storage Locations, Oracle Subinventories).
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

        // ??????????????????????????????????????????????????????????????????
        // TIPO Y CAPACIDADES OPERATIVAS (Estándares ERP)
        // ??????????????????????????????????????????????????????????????????

        /// <summary>
        /// Tipo de localidad operativa
        /// </summary>
        [Required(ErrorMessage = "El tipo de localidad es obligatorio.")]
        public TipoLocalidad Tipo { get; set; }

        public string TipoDescripcion { get; set; } = string.Empty;

        /// <summary>
        /// Permite operaciones de venta
        /// </summary>
        public bool PermiteVentas { get; set; }

        /// <summary>
        /// Permite operaciones de compra
        /// </summary>
        public bool PermiteCompras { get; set; }

        /// <summary>
        /// Permite transferencias de inventario
        /// </summary>
        public bool PermiteTransferencias { get; set; }

        /// <summary>
        /// Permite ajustes de inventario
        /// </summary>
        public bool PermiteAjustes { get; set; }

        /// <summary>
        /// Requiere control de lotes
        /// </summary>
        public bool RequiereControlLotes { get; set; }

        /// <summary>
        /// Requiere control de números de serie
        /// </summary>
        public bool RequiereNumerosSerie { get; set; }

        // ??????????????????????????????????????????????????????????????????
        // CUENTAS CONTABLES (GL Accounts)
        // ??????????????????????????????????????????????????????????????????

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

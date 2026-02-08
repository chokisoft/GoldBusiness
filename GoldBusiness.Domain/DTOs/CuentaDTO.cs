using System.ComponentModel.DataAnnotations;

namespace GoldBusiness.Domain.DTOs
{
    /// <summary>
    /// DTO para Cuenta - Cuenta contable individual.
    /// Nivel más detallado donde se registran transacciones.
    /// </summary>
    public class CuentaDTO
    {
        public int Id { get; set; }

        /// <summary>
        /// Código de la cuenta (formato: ########).
        /// Ejemplo: "10000010" = EFECTIVO EN CAJA.
        /// </summary>
        [Required(
            ErrorMessageResourceType = typeof(GoldBusiness.Domain.Resources.ValidationMessages),
            ErrorMessageResourceName = nameof(GoldBusiness.Domain.Resources.ValidationMessages.CodigoObligatorio)
        )]
        [Display(
            Name = nameof(GoldBusiness.Domain.Resources.ValidationMessages.Field_Codigo),
            ResourceType = typeof(GoldBusiness.Domain.Resources.ValidationMessages)
        )]
        [StringLength(8, MinimumLength = 8,
            ErrorMessageResourceType = typeof(GoldBusiness.Domain.Resources.ValidationMessages),
            ErrorMessageResourceName = nameof(GoldBusiness.Domain.Resources.ValidationMessages.CuentaCodigoLongitud)
        )]
        [RegularExpression(@"^\d{8}$",
            ErrorMessageResourceType = typeof(GoldBusiness.Domain.Resources.ValidationMessages),
            ErrorMessageResourceName = nameof(GoldBusiness.Domain.Resources.ValidationMessages.CuentaCodigoFormato)
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
            ErrorMessageResourceName = nameof(GoldBusiness.Domain.Resources.ValidationMessages.SubGrupoCuentaObligatorio)
        )]
        [Display(
            Name = nameof(GoldBusiness.Domain.Resources.ValidationMessages.Field_SubGrupoCuenta),
            ResourceType = typeof(GoldBusiness.Domain.Resources.ValidationMessages)
        )]
        [Range(1, int.MaxValue,
            ErrorMessageResourceType = typeof(GoldBusiness.Domain.Resources.ValidationMessages),
            ErrorMessageResourceName = nameof(GoldBusiness.Domain.Resources.ValidationMessages.SubGrupoCuentaSeleccion)
        )]

        public int SystemConfigurationId { get; set; }
        public int SubGrupoCuentaId { get; set; }

        [Display(
            Name = nameof(GoldBusiness.Domain.Resources.ValidationMessages.Field_Cancelado),
            ResourceType = typeof(GoldBusiness.Domain.Resources.ValidationMessages)
        )]
        public bool Cancelado { get; set; }

        public string CreadoPor { get; set; } = string.Empty;
        public DateTime FechaHoraCreado { get; set; }
        public string? ModificadoPor { get; set; }
        public DateTime? FechaHoraModificado { get; set; }

        /// <summary>
        /// Jerarquía completa.
        /// </summary>
        public string SubGrupoCuentaCodigo { get; set; } = string.Empty;
        public string SubGrupoCuentaDescripcion { get; set; } = string.Empty;
        public string GrupoCuentaCodigo { get; set; } = string.Empty;
        public string GrupoCuentaDescripcion { get; set; } = string.Empty;

        public string CodigoDescripcion => $"{Codigo} | {Descripcion}";
    }
}
using System.ComponentModel.DataAnnotations;

namespace GoldBusiness.Domain.DTOs
{
    /// <summary>
    /// DTO para SubGrupoCuenta - Nivel intermedio del plan de cuentas.
    /// Agrupa cuentas relacionadas dentro de un GrupoCuenta.
    /// </summary>
    public class SubGrupoCuentaDTO
    {
        /// <summary>
        /// Identificador único del subgrupo de cuenta.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Código del subgrupo (formato: #####).
        /// Ejemplo: "01001" = ACTIVO CIRCULANTE.
        /// </summary>
        [Required(
            ErrorMessageResourceType = typeof(GoldBusiness.Domain.Resources.ValidationMessages),
            ErrorMessageResourceName = nameof(GoldBusiness.Domain.Resources.ValidationMessages.CodigoObligatorio)
        )]
        [Display(
            Name = nameof(GoldBusiness.Domain.Resources.ValidationMessages.Field_Codigo),
            ResourceType = typeof(GoldBusiness.Domain.Resources.ValidationMessages)
        )]
        [StringLength(5, MinimumLength = 5,
            ErrorMessageResourceType = typeof(GoldBusiness.Domain.Resources.ValidationMessages),
            ErrorMessageResourceName = nameof(GoldBusiness.Domain.Resources.ValidationMessages.SubGrupoCuentaCodigoLongitud)
        )]
        [RegularExpression(@"^\d{5}$",
            ErrorMessageResourceType = typeof(GoldBusiness.Domain.Resources.ValidationMessages),
            ErrorMessageResourceName = nameof(GoldBusiness.Domain.Resources.ValidationMessages.SubGrupoCuentaCodigoFormato)
        )]
        public string Codigo { get; set; } = string.Empty;

        /// <summary>
        /// ID del grupo de cuenta al que pertenece.
        /// </summary>
        [Required(
            ErrorMessageResourceType = typeof(GoldBusiness.Domain.Resources.ValidationMessages),
            ErrorMessageResourceName = nameof(GoldBusiness.Domain.Resources.ValidationMessages.GrupoCuentaObligatorio)
        )]
        [Display(
            Name = nameof(GoldBusiness.Domain.Resources.ValidationMessages.Field_GrupoCuenta),
            ResourceType = typeof(GoldBusiness.Domain.Resources.ValidationMessages)
        )]
        [Range(1, int.MaxValue,
            ErrorMessageResourceType = typeof(GoldBusiness.Domain.Resources.ValidationMessages),
            ErrorMessageResourceName = nameof(GoldBusiness.Domain.Resources.ValidationMessages.GrupoCuentaSeleccion)
        )]
        public int GrupoCuentaId { get; set; }

        /// <summary>
        /// Descripción del subgrupo de cuenta.
        /// </summary>
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

        /// <summary>
        /// Indica si las cuentas de este subgrupo son deudoras.
        /// </summary>
        [Display(
            Name = nameof(GoldBusiness.Domain.Resources.ValidationMessages.Field_Deudora),
            ResourceType = typeof(GoldBusiness.Domain.Resources.ValidationMessages)
        )]
        public bool Deudora { get; set; }

        /// <summary>
        /// Indica si el subgrupo está cancelado.
        /// </summary>
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
        /// Jerarquía - Información del grupo padre.
        /// </summary>
        public string GrupoCuentaCodigo { get; set; } = string.Empty;
        public string GrupoCuentaDescripcion { get; set; } = string.Empty;

        /// <summary>
        /// Cantidad de cuentas hijas.
        /// </summary>
        [Display(
            Name = nameof(GoldBusiness.Domain.Resources.ValidationMessages.Field_Cuentas),
            ResourceType = typeof(GoldBusiness.Domain.Resources.ValidationMessages)
        )]
        public int CantidadCuentas { get; set; }

        public string CodigoDescripcion => $"{Codigo} | {Descripcion}";
    }
}
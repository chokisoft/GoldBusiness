using System.ComponentModel.DataAnnotations;

namespace GoldBusiness.Domain.DTOs
{
    /// <summary>
    /// DTO para GrupoCuenta - Nivel superior del plan de cuentas.
    /// Representa grupos contables principales: Activo, Pasivo, Patrimonio, Ingresos, Gastos.
    /// </summary>
    public class GrupoCuentaDTO
    {
        /// <summary>
        /// Identificador único del grupo de cuenta.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Código del grupo (2 dígitos exactos).
        /// Ejemplos: "01" = ACTIVO, "02" = PASIVO, "03" = PATRIMONIO.
        /// </summary>
        [Required(
            ErrorMessageResourceType = typeof(GoldBusiness.Domain.Resources.ValidationMessages),
            ErrorMessageResourceName = nameof(GoldBusiness.Domain.Resources.ValidationMessages.CodigoObligatorio)
        )]
        [Display(
            Name = nameof(GoldBusiness.Domain.Resources.ValidationMessages.Field_Codigo),
            ResourceType = typeof(GoldBusiness.Domain.Resources.ValidationMessages)
        )]
        [StringLength(2, MinimumLength = 2,
            ErrorMessageResourceType = typeof(GoldBusiness.Domain.Resources.ValidationMessages),
            ErrorMessageResourceName = nameof(GoldBusiness.Domain.Resources.ValidationMessages.CodigoLongitud)
        )]
        [RegularExpression(@"^\d{2}$",
            ErrorMessageResourceType = typeof(GoldBusiness.Domain.Resources.ValidationMessages),
            ErrorMessageResourceName = nameof(GoldBusiness.Domain.Resources.ValidationMessages.CodigoFormato)
        )]
        public string Codigo { get; set; } = string.Empty;

        /// <summary>
        /// Descripción del grupo de cuenta.
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
            ErrorMessageResourceName = nameof(GoldBusiness.Domain.Resources.ValidationMessages.StringLengthMax)
        )]
        public string Descripcion { get; set; } = string.Empty;

        /// <summary>
        /// Indica si el grupo está cancelado/inactivo.
        /// </summary>
        [Display(
            Name = nameof(GoldBusiness.Domain.Resources.ValidationMessages.Field_Cancelado),
            ResourceType = typeof(GoldBusiness.Domain.Resources.ValidationMessages)
        )]
        public bool Cancelado { get; set; }

        /// <summary>
        /// Usuario que creó el registro.
        /// </summary>
        [Display(
            Name = nameof(GoldBusiness.Domain.Resources.ValidationMessages.Field_CreadoPor),
            ResourceType = typeof(GoldBusiness.Domain.Resources.ValidationMessages)
        )]
        public string CreadoPor { get; set; } = string.Empty;

        /// <summary>
        /// Fecha y hora de creación.
        /// </summary>
        [Display(
            Name = nameof(GoldBusiness.Domain.Resources.ValidationMessages.Field_FechaCreacion),
            ResourceType = typeof(GoldBusiness.Domain.Resources.ValidationMessages)
        )]
        public DateTime FechaHoraCreado { get; set; }

        /// <summary>
        /// Usuario que modificó el registro.
        /// </summary>
        [Display(
            Name = nameof(GoldBusiness.Domain.Resources.ValidationMessages.Field_ModificadoPor),
            ResourceType = typeof(GoldBusiness.Domain.Resources.ValidationMessages)
        )]
        public string? ModificadoPor { get; set; }

        /// <summary>
        /// Fecha y hora de modificación.
        /// </summary>
        [Display(
            Name = nameof(GoldBusiness.Domain.Resources.ValidationMessages.Field_FechaModificacion),
            ResourceType = typeof(GoldBusiness.Domain.Resources.ValidationMessages)
        )]
        public DateTime? FechaHoraModificado { get; set; }

        /// <summary>
        /// Cantidad de subgrupos asociados.
        /// </summary>
        [Display(
            Name = nameof(GoldBusiness.Domain.Resources.ValidationMessages.Field_Subgrupos),
            ResourceType = typeof(GoldBusiness.Domain.Resources.ValidationMessages)
        )]
        public int CantidadSubGrupos { get; set; }

        /// <summary>
        /// Formato: "Código | Descripción".
        /// </summary>
        public string CodigoDescripcion => $"{Codigo} | {Descripcion}";
    }
}
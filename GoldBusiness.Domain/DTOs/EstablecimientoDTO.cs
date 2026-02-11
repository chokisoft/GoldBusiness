using System.ComponentModel.DataAnnotations;

namespace GoldBusiness.Domain.DTOs
{
    /// <summary>
    /// DTO para Establecimiento - Sede o punto de operación del negocio.
    /// </summary>
    public class EstablecimientoDTO
    {
        public int Id { get; set; }

        /// <summary>
        /// Código del establecimiento (6 caracteres).
        /// </summary>
        [Required(
            ErrorMessageResourceType = typeof(GoldBusiness.Domain.Resources.ValidationMessages),
            ErrorMessageResourceName = nameof(GoldBusiness.Domain.Resources.ValidationMessages.CodigoObligatorio)
        )]
        [Display(
            Name = nameof(GoldBusiness.Domain.Resources.ValidationMessages.Field_Codigo),
            ResourceType = typeof(GoldBusiness.Domain.Resources.ValidationMessages)
        )]
        [StringLength(6, MinimumLength = 6,
            ErrorMessageResourceType = typeof(GoldBusiness.Domain.Resources.ValidationMessages),
            ErrorMessageResourceName = nameof(GoldBusiness.Domain.Resources.ValidationMessages.EstablecimientoCodigoLongitud)
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
            ErrorMessageResourceName = nameof(GoldBusiness.Domain.Resources.ValidationMessages.NegocioIdObligatorio)
        )]
        public int NegocioId { get; set; }

        public string NegocioDescripcion { get; set; } = string.Empty;

        public bool Activo { get; set; }
        public bool Cancelado { get; set; }

        public string CreadoPor { get; set; } = string.Empty;
        public DateTime FechaHoraCreado { get; set; }
        public string? ModificadoPor { get; set; }
        public DateTime? FechaHoraModificado { get; set; }
    }
}
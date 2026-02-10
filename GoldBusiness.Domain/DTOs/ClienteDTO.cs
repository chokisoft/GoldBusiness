using System.ComponentModel.DataAnnotations;

namespace GoldBusiness.Domain.DTOs
{
    /// <summary>
    /// DTO para Cliente - Representa un cliente del negocio.
    /// Almacena información fiscal, bancaria y de contacto.
    /// </summary>
    public class ClienteDTO
    {
        public int Id { get; set; }

        /// <summary>
        /// Código único del cliente (8 caracteres).
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
            ErrorMessageResourceName = nameof(GoldBusiness.Domain.Resources.ValidationMessages.ClienteCodigoLongitud)
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

        [Display(
            Name = nameof(GoldBusiness.Domain.Resources.ValidationMessages.Field_Nif),
            ResourceType = typeof(GoldBusiness.Domain.Resources.ValidationMessages)
        )]
        [StringLength(11,
            ErrorMessageResourceType = typeof(GoldBusiness.Domain.Resources.ValidationMessages),
            ErrorMessageResourceName = nameof(GoldBusiness.Domain.Resources.ValidationMessages.NifLongitud)
        )]
        public string? Nif { get; set; }

        [Display(
            Name = nameof(GoldBusiness.Domain.Resources.ValidationMessages.Field_Iban),
            ResourceType = typeof(GoldBusiness.Domain.Resources.ValidationMessages)
        )]
        [StringLength(27,
            ErrorMessageResourceType = typeof(GoldBusiness.Domain.Resources.ValidationMessages),
            ErrorMessageResourceName = nameof(GoldBusiness.Domain.Resources.ValidationMessages.IbanLongitud)
        )]
        public string? Iban { get; set; }

        [Display(
            Name = nameof(GoldBusiness.Domain.Resources.ValidationMessages.Field_BicoSwift),
            ResourceType = typeof(GoldBusiness.Domain.Resources.ValidationMessages)
        )]
        [StringLength(11,
            ErrorMessageResourceType = typeof(GoldBusiness.Domain.Resources.ValidationMessages),
            ErrorMessageResourceName = nameof(GoldBusiness.Domain.Resources.ValidationMessages.BicoSwiftLongitud)
        )]
        public string? BicoSwift { get; set; }

        [Display(
            Name = nameof(GoldBusiness.Domain.Resources.ValidationMessages.Field_Iva),
            ResourceType = typeof(GoldBusiness.Domain.Resources.ValidationMessages)
        )]
        [Range(-0.01, 99.99,
            ErrorMessageResourceType = typeof(GoldBusiness.Domain.Resources.ValidationMessages),
            ErrorMessageResourceName = nameof(GoldBusiness.Domain.Resources.ValidationMessages.IvaRango)
        )]
        [DisplayFormat(DataFormatString = "{0:N2}%", ApplyFormatInEditMode = false)]
        public decimal Iva { get; set; }

        [Display(
            Name = nameof(GoldBusiness.Domain.Resources.ValidationMessages.Field_Direccion),
            ResourceType = typeof(GoldBusiness.Domain.Resources.ValidationMessages)
        )]
        [StringLength(256,
            ErrorMessageResourceType = typeof(GoldBusiness.Domain.Resources.ValidationMessages),
            ErrorMessageResourceName = nameof(GoldBusiness.Domain.Resources.ValidationMessages.DireccionLongitud)
        )]
        public string? Direccion { get; set; }

        [Display(
            Name = nameof(GoldBusiness.Domain.Resources.ValidationMessages.Field_Municipio),
            ResourceType = typeof(GoldBusiness.Domain.Resources.ValidationMessages)
        )]
        [StringLength(50,
            ErrorMessageResourceType = typeof(GoldBusiness.Domain.Resources.ValidationMessages),
            ErrorMessageResourceName = nameof(GoldBusiness.Domain.Resources.ValidationMessages.MunicipioLongitud)
        )]
        public string? Municipio { get; set; }

        [Display(
            Name = nameof(GoldBusiness.Domain.Resources.ValidationMessages.Field_Provincia),
            ResourceType = typeof(GoldBusiness.Domain.Resources.ValidationMessages)
        )]
        [StringLength(50,
            ErrorMessageResourceType = typeof(GoldBusiness.Domain.Resources.ValidationMessages),
            ErrorMessageResourceName = nameof(GoldBusiness.Domain.Resources.ValidationMessages.ProvinciaLongitud)
        )]
        public string? Provincia { get; set; }

        [Display(
            Name = nameof(GoldBusiness.Domain.Resources.ValidationMessages.Field_CodPostal),
            ResourceType = typeof(GoldBusiness.Domain.Resources.ValidationMessages)
        )]
        [StringLength(5,
            ErrorMessageResourceType = typeof(GoldBusiness.Domain.Resources.ValidationMessages),
            ErrorMessageResourceName = nameof(GoldBusiness.Domain.Resources.ValidationMessages.CodPostalLongitud)
        )]
        public string? CodPostal { get; set; }

        [Display(
            Name = nameof(GoldBusiness.Domain.Resources.ValidationMessages.Field_Web),
            ResourceType = typeof(GoldBusiness.Domain.Resources.ValidationMessages)
        )]
        [Url(
            ErrorMessageResourceType = typeof(GoldBusiness.Domain.Resources.ValidationMessages),
            ErrorMessageResourceName = nameof(GoldBusiness.Domain.Resources.ValidationMessages.WebFormato)
        )]
        [StringLength(256,
            ErrorMessageResourceType = typeof(GoldBusiness.Domain.Resources.ValidationMessages),
            ErrorMessageResourceName = nameof(GoldBusiness.Domain.Resources.ValidationMessages.WebLongitud)
        )]
        public string? Web { get; set; }

        [Display(
            Name = nameof(GoldBusiness.Domain.Resources.ValidationMessages.Field_Email1),
            ResourceType = typeof(GoldBusiness.Domain.Resources.ValidationMessages)
        )]
        [EmailAddress(
            ErrorMessageResourceType = typeof(GoldBusiness.Domain.Resources.ValidationMessages),
            ErrorMessageResourceName = nameof(GoldBusiness.Domain.Resources.ValidationMessages.EmailFormato)
        )]
        [StringLength(256,
            ErrorMessageResourceType = typeof(GoldBusiness.Domain.Resources.ValidationMessages),
            ErrorMessageResourceName = nameof(GoldBusiness.Domain.Resources.ValidationMessages.EmailLongitud)
        )]
        public string? Email1 { get; set; }

        [Display(
            Name = nameof(GoldBusiness.Domain.Resources.ValidationMessages.Field_Email2),
            ResourceType = typeof(GoldBusiness.Domain.Resources.ValidationMessages)
        )]
        [EmailAddress(
            ErrorMessageResourceType = typeof(GoldBusiness.Domain.Resources.ValidationMessages),
            ErrorMessageResourceName = nameof(GoldBusiness.Domain.Resources.ValidationMessages.EmailFormato)
        )]
        [StringLength(256,
            ErrorMessageResourceType = typeof(GoldBusiness.Domain.Resources.ValidationMessages),
            ErrorMessageResourceName = nameof(GoldBusiness.Domain.Resources.ValidationMessages.EmailLongitud)
        )]
        public string? Email2 { get; set; }

        [Display(
            Name = nameof(GoldBusiness.Domain.Resources.ValidationMessages.Field_Telefono1),
            ResourceType = typeof(GoldBusiness.Domain.Resources.ValidationMessages)
        )]
        [Phone(
            ErrorMessageResourceType = typeof(GoldBusiness.Domain.Resources.ValidationMessages),
            ErrorMessageResourceName = nameof(GoldBusiness.Domain.Resources.ValidationMessages.TelefonoFormatoInvalido)
        )]
        [RegularExpression(@"^\d{4}-\d{4}$",
            ErrorMessageResourceType = typeof(GoldBusiness.Domain.Resources.ValidationMessages),
            ErrorMessageResourceName = nameof(GoldBusiness.Domain.Resources.ValidationMessages.TelefonoFormato)
        )]
        [StringLength(50,
            ErrorMessageResourceType = typeof(GoldBusiness.Domain.Resources.ValidationMessages),
            ErrorMessageResourceName = nameof(GoldBusiness.Domain.Resources.ValidationMessages.TelefonoLongitud)
        )]
        public string? Telefono1 { get; set; }

        [Display(
            Name = nameof(GoldBusiness.Domain.Resources.ValidationMessages.Field_Telefono2),
            ResourceType = typeof(GoldBusiness.Domain.Resources.ValidationMessages)
        )]
        [Phone(
            ErrorMessageResourceType = typeof(GoldBusiness.Domain.Resources.ValidationMessages),
            ErrorMessageResourceName = nameof(GoldBusiness.Domain.Resources.ValidationMessages.TelefonoFormatoInvalido)
        )]
        [RegularExpression(@"^\d{4}-\d{4}$",
            ErrorMessageResourceType = typeof(GoldBusiness.Domain.Resources.ValidationMessages),
            ErrorMessageResourceName = nameof(GoldBusiness.Domain.Resources.ValidationMessages.TelefonoFormato)
        )]
        [StringLength(50,
            ErrorMessageResourceType = typeof(GoldBusiness.Domain.Resources.ValidationMessages),
            ErrorMessageResourceName = nameof(GoldBusiness.Domain.Resources.ValidationMessages.TelefonoLongitud)
        )]
        public string? Telefono2 { get; set; }

        [Display(
            Name = nameof(GoldBusiness.Domain.Resources.ValidationMessages.Field_Fax1),
            ResourceType = typeof(GoldBusiness.Domain.Resources.ValidationMessages)
        )]
        [StringLength(50,
            ErrorMessageResourceType = typeof(GoldBusiness.Domain.Resources.ValidationMessages),
            ErrorMessageResourceName = nameof(GoldBusiness.Domain.Resources.ValidationMessages.FaxLongitud)
        )]
        public string? Fax1 { get; set; }

        [Display(
            Name = nameof(GoldBusiness.Domain.Resources.ValidationMessages.Field_Fax2),
            ResourceType = typeof(GoldBusiness.Domain.Resources.ValidationMessages)
        )]
        [StringLength(50,
            ErrorMessageResourceType = typeof(GoldBusiness.Domain.Resources.ValidationMessages),
            ErrorMessageResourceName = nameof(GoldBusiness.Domain.Resources.ValidationMessages.FaxLongitud)
        )]
        public string? Fax2 { get; set; }

        [Display(
            Name = nameof(GoldBusiness.Domain.Resources.ValidationMessages.Field_Cancelado),
            ResourceType = typeof(GoldBusiness.Domain.Resources.ValidationMessages)
        )]
        public bool Cancelado { get; set; }

        [Display(
            Name = nameof(GoldBusiness.Domain.Resources.ValidationMessages.Field_CreadoPor),
            ResourceType = typeof(GoldBusiness.Domain.Resources.ValidationMessages)
        )]
        public string CreadoPor { get; set; } = string.Empty;

        [Display(
            Name = nameof(GoldBusiness.Domain.Resources.ValidationMessages.Field_FechaCreacion),
            ResourceType = typeof(GoldBusiness.Domain.Resources.ValidationMessages)
        )]
        public DateTime FechaHoraCreado { get; set; }

        [Display(
            Name = nameof(GoldBusiness.Domain.Resources.ValidationMessages.Field_ModificadoPor),
            ResourceType = typeof(GoldBusiness.Domain.Resources.ValidationMessages)
        )]
        public string? ModificadoPor { get; set; }

        [Display(
            Name = nameof(GoldBusiness.Domain.Resources.ValidationMessages.Field_FechaModificacion),
            ResourceType = typeof(GoldBusiness.Domain.Resources.ValidationMessages)
        )]
        public DateTime? FechaHoraModificado { get; set; }

        /// <summary>
        /// Formato: "Código | Descripción".
        /// </summary>
        public string CodigoDescripcion => $"{Codigo} | {Descripcion}";
    }
}
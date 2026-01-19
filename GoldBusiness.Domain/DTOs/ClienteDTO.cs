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
        [Required(ErrorMessage = "El código es obligatorio")]
        [Display(Name = "Código")]
        [StringLength(8, MinimumLength = 8, ErrorMessage = "El código debe tener exactamente 8 caracteres")]
        public string Codigo { get; set; } = string.Empty;

        [Required(ErrorMessage = "La descripción es obligatoria")]
        [Display(Name = "Descripción")]
        [StringLength(256, ErrorMessage = "La descripción no puede exceder 256 caracteres")]
        public string Descripcion { get; set; } = string.Empty;

        [Display(Name = "NIF/CIF")]
        [StringLength(11, ErrorMessage = "El NIF no puede exceder 11 caracteres")]
        public string? Nif { get; set; }

        [Display(Name = "IBAN")]
        [StringLength(27, ErrorMessage = "El IBAN no puede exceder 27 caracteres")]
        public string? Iban { get; set; }

        [Display(Name = "BIC/SWIFT")]
        [StringLength(11, ErrorMessage = "El BIC/SWIFT no puede exceder 11 caracteres")]
        public string? BicoSwift { get; set; }

        [Display(Name = "IVA (%)")]
        [Range(-0.01, 99.99, ErrorMessage = "El IVA debe estar entre -0.01 y 99.99")]
        [DisplayFormat(DataFormatString = "{0:N2}%", ApplyFormatInEditMode = false)]
        public decimal Iva { get; set; }

        [Display(Name = "Dirección")]
        [StringLength(256, ErrorMessage = "La dirección no puede exceder 256 caracteres")]
        public string? Direccion { get; set; }

        [Display(Name = "Municipio")]
        [StringLength(50, ErrorMessage = "El municipio no puede exceder 50 caracteres")]
        public string? Municipio { get; set; }

        [Display(Name = "Provincia")]
        [StringLength(50, ErrorMessage = "La provincia no puede exceder 50 caracteres")]
        public string? Provincia { get; set; }

        [Display(Name = "Código Postal")]
        [StringLength(5, ErrorMessage = "El código postal no puede exceder 5 caracteres")]
        public string? CodPostal { get; set; }

        [Display(Name = "Sitio Web")]
        [Url(ErrorMessage = "La URL no es válida")]
        [StringLength(256, ErrorMessage = "La URL no puede exceder 256 caracteres")]
        public string? Web { get; set; }

        [Display(Name = "Email Principal")]
        [EmailAddress(ErrorMessage = "El email no es válido")]
        [StringLength(256, ErrorMessage = "El email no puede exceder 256 caracteres")]
        public string? Email1 { get; set; }

        [Display(Name = "Email Secundario")]
        [EmailAddress(ErrorMessage = "El email no es válido")]
        [StringLength(256, ErrorMessage = "El email no puede exceder 256 caracteres")]
        public string? Email2 { get; set; }

        [Display(Name = "Teléfono Principal")]
        [Phone(ErrorMessage = "El teléfono no es válido")]
        [RegularExpression(@"^\d{4}-\d{4}$", ErrorMessage = "El formato debe ser ####-####")]
        [StringLength(50, ErrorMessage = "El teléfono no puede exceder 50 caracteres")]
        public string? Telefono1 { get; set; }

        [Display(Name = "Teléfono Secundario")]
        [Phone(ErrorMessage = "El teléfono no es válido")]
        [RegularExpression(@"^\d{4}-\d{4}$", ErrorMessage = "El formato debe ser ####-####")]
        [StringLength(50, ErrorMessage = "El teléfono no puede exceder 50 caracteres")]
        public string? Telefono2 { get; set; }

        [Display(Name = "Fax Principal")]
        [StringLength(50, ErrorMessage = "El fax no puede exceder 50 caracteres")]
        public string? Fax1 { get; set; }

        [Display(Name = "Fax Secundario")]
        [StringLength(50, ErrorMessage = "El fax no puede exceder 50 caracteres")]
        public string? Fax2 { get; set; }

        [Display(Name = "Cancelado")]
        public bool Cancelado { get; set; }

        public string CreadoPor { get; set; } = string.Empty;
        public DateTime FechaHoraCreado { get; set; }
        public string? ModificadoPor { get; set; }
        public DateTime? FechaHoraModificado { get; set; }

        /// <summary>
        /// Formato: "Código | Descripción".
        /// </summary>
        public string CodigoDescripcion => $"{Codigo} | {Descripcion}";
    }
}
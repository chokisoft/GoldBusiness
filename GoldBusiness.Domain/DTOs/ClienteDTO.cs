using GoldBusiness.Domain.Enums;
using GoldBusiness.Domain.Resources;
using System;
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

        [Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = nameof(ValidationMessages.CodigoObligatorio))]
        [StringLength(8, MinimumLength = 8, ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = nameof(ValidationMessages.CodigoLongitud))]
        public string Codigo { get; set; } = string.Empty;

        [Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = nameof(ValidationMessages.Required))]
        [StringLength(256, ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = nameof(ValidationMessages.StringLengthMax))]
        public string Descripcion { get; set; } = string.Empty;

        // ═══════════════════════════════════════════════════════════════
        // 🧾 DATOS FISCALES
        // ═══════════════════════════════════════════════════════════════

        [StringLength(30, ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = nameof(ValidationMessages.IdentificadorFiscalLongitud))]
        public string? IdentificadorFiscal { get; set; }

        public TipoIdentificacionFiscal? TipoIdentificadorFiscal { get; set; }

        public RegimenFiscal? RegimenFiscal { get; set; }

        [Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = nameof(ValidationMessages.Required))]
        [Range(-0.01, 99.99, ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = nameof(ValidationMessages.IvaRango))]
        public decimal TasaIva { get; set; }

        public bool ExentoIva { get; set; }

        public bool Extranjero { get; set; }

        [StringLength(3, ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = nameof(ValidationMessages.CodigoPaisIsoLongitud))]
        public string? CodigoPaisIso { get; set; }

        public bool ValidarIdentificadorFiscal { get; set; }

        public bool InversionSujetoPasivo { get; set; }

        // ═══════════════════════════════════════════════════════════════
        // 🏦 DATOS BANCARIOS
        // ═══════════════════════════════════════════════════════════════

        [StringLength(27, ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = nameof(ValidationMessages.IbanLongitud))]
        public string? Iban { get; set; }

        [StringLength(11, ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = nameof(ValidationMessages.BicoSwiftLongitud))]
        public string? BicoSwift { get; set; }

        // ═══════════════════════════════════════════════════════════════
        // 📍 LOCALIZACIÓN
        // ═══════════════════════════════════════════════════════════════

        [StringLength(256, ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = nameof(ValidationMessages.StringLengthMax))]
        public string? Direccion { get; set; }

        public int? PaisId { get; set; }
        public string? PaisDescripcion { get; set; }

        public int? ProvinciaId { get; set; }
        public string? ProvinciaDescripcion { get; set; }

        public int? MunicipioId { get; set; }
        public string? MunicipioDescripcion { get; set; }

        public int? CodigoPostalId { get; set; }
        public string? CodigoPostalCodigo { get; set; }

        // ═══════════════════════════════════════════════════════════════
        // 📞 CONTACTO
        // ═══════════════════════════════════════════════════════════════

        [StringLength(256, ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = nameof(ValidationMessages.StringLengthMax))]
        public string? Web { get; set; }

        [EmailAddress(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = nameof(ValidationMessages.EmailFormato))]
        [StringLength(256, ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = nameof(ValidationMessages.StringLengthMax))]
        public string? Email { get; set; }

        [StringLength(50, ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = nameof(ValidationMessages.TelefonoLongitud))]
        public string? Telefono { get; set; }

        // ═══════════════════════════════════════════════════════════════
        // 🔧 AUDITORÍA
        // ═══════════════════════════════════════════════════════════════

        public bool Cancelado { get; set; }
        public string CreadoPor { get; set; } = string.Empty;
        public DateTime FechaHoraCreado { get; set; }
        public string? ModificadoPor { get; set; }
        public DateTime? FechaHoraModificado { get; set; }
    }
}
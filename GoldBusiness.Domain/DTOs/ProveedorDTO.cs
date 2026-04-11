using GoldBusiness.Domain.Enums;
using GoldBusiness.Domain.Resources;
using System;
using System.ComponentModel.DataAnnotations;

namespace GoldBusiness.Domain.DTOs
{
    /// <summary>
    /// DTO para Proveedor - Representa un proveedor del negocio.
    /// Almacena información fiscal, bancaria y de contacto.
    /// </summary>
    public class ProveedorDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = nameof(ValidationMessages.CodigoObligatorio))
        ]
        [StringLength(5, MinimumLength = 5, ErrorMessageResourceType = typeof(ValidationMessages),
            ErrorMessageResourceName = nameof(ValidationMessages.CodigoLongitud))
        ]
        public string Codigo { get; set; } = string.Empty;

        [Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = nameof(ValidationMessages.Required))
        ]
        [StringLength(256, ErrorMessageResourceType = typeof(ValidationMessages),
            ErrorMessageResourceName = nameof(ValidationMessages.StringLengthMax))
        ]
        public string Descripcion { get; set; } = string.Empty;

        /// <summary>
        /// NIF/CIF del proveedor.
        /// </summary>
        [StringLength(11, ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = nameof(ValidationMessages.NifLongitud))]
        public string? Nif { get; set; }

        [StringLength(27, ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = nameof(ValidationMessages.IbanLongitud))]
        public string? Iban { get; set; }

        [StringLength(11, ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = nameof(ValidationMessages.BicoSwiftLongitud))]
        public string? BicoSwift { get; set; }

        [Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = nameof(ValidationMessages.Required))]
        [Range(-0.01, 99.99, ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = nameof(ValidationMessages.IvaRango))]
        public decimal Iva { get; set; }

        [StringLength(256, ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = nameof(ValidationMessages.StringLengthMax))]
        public string? Direccion { get; set; }

        // Relaciones geográficas por ID
        public int? PaisId { get; set; }
        public string? PaisDescripcion { get; set; }

        public int? ProvinciaId { get; set; }
        public string? ProvinciaDescripcion { get; set; }

        public int? MunicipioId { get; set; }
        public string? MunicipioDescripcion { get; set; }

        public int? CodigoPostalId { get; set; }
        public string? CodigoPostalCodigo { get; set; }

        [StringLength(256, ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = nameof(ValidationMessages.StringLengthMax))]
        public string? Web { get; set; }

        [EmailAddress(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = nameof(ValidationMessages.EmailFormato))]
        [StringLength(256, ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = nameof(ValidationMessages.StringLengthMax))]
        public string? Email1 { get; set; }

        [EmailAddress(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = nameof(ValidationMessages.EmailFormato))]
        [StringLength(256, ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = nameof(ValidationMessages.StringLengthMax))]
        public string? Email2 { get; set; }

        [StringLength(50, ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = nameof(ValidationMessages.TelefonoLongitud))]
        public string? Telefono1 { get; set; }

        [StringLength(50, ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = nameof(ValidationMessages.TelefonoLongitud))]
        public string? Telefono2 { get; set; }

        [StringLength(50, ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = nameof(ValidationMessages.FaxLongitud))]
        public string? Fax1 { get; set; }

        [StringLength(50, ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = nameof(ValidationMessages.FaxLongitud))]
        public string? Fax2 { get; set; }

        // ═══════════════════════════════════════════════════════════════
        // 🧾 DATOS FISCALES EXTENDIDOS
        // ═══════════════════════════════════════════════════════════════
        
        public TipoIdentificacionFiscal? TipoIdentificadorFiscal { get; set; }
        
        public RegimenFiscal? RegimenFiscal { get; set; }
        
        public bool ExentoIva { get; set; }
        
        public bool Extranjero { get; set; }
        
        [StringLength(3, ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = nameof(ValidationMessages.CodigoPaisIsoLongitud))]
        public string? CodigoPaisIso { get; set; }
        
        public bool ValidarIdentificadorFiscal { get; set; }
        
        public bool InversionSujetoPasivo { get; set; }

        // Auditoría
        public bool Cancelado { get; set; }
        public string CreadoPor { get; set; } = string.Empty;
        public DateTime FechaHoraCreado { get; set; }
        public string? ModificadoPor { get; set; }
        public DateTime? FechaHoraModificado { get; set; }
    }
}
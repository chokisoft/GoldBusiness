using System.ComponentModel.DataAnnotations;
using GoldBusiness.Domain.Resources;

namespace GoldBusiness.Domain.DTOs
{
    /// <summary>
    /// DTO para la configuración general del sistema.
    /// Almacena datos del negocio, licencia y cuentas contables por defecto.
    /// </summary>
    public class SystemConfigurationDTO
    {
        public int Id { get; set; }

        [Required(
            ErrorMessageResourceType = typeof(ValidationMessages),
            ErrorMessageResourceName = nameof(ValidationMessages.CodigoObligatorio)
        )]
        [Display(
            Name = nameof(ValidationMessages.Field_Codigo),
            ResourceType = typeof(ValidationMessages)
        )]
        // Exactamente 3 caracteres alfanuméricos
        [StringLength(3, MinimumLength = 3,
            ErrorMessageResourceType = typeof(ValidationMessages),
            ErrorMessageResourceName = nameof(ValidationMessages.CodigoLongitud)
        )]
        [RegularExpression(@"^[A-Za-z0-9]{3}$",
            ErrorMessageResourceType = typeof(ValidationMessages),
            ErrorMessageResourceName = nameof(ValidationMessages.CodigoFormatoAlfanumerico)
        )]
        public string CodigoSistema { get; set; } = string.Empty;

        [Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = nameof(ValidationMessages.Required))]
        [Display(Name = nameof(ValidationMessages.Field_Licencia), ResourceType = typeof(ValidationMessages))]
        [StringLength(400,
            ErrorMessageResourceType = typeof(ValidationMessages),
            ErrorMessageResourceName = nameof(ValidationMessages.StringLengthMax)
        )]
        public string Licencia { get; set; } = string.Empty;

        [Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = nameof(ValidationMessages.Required))]
        [Display(Name = nameof(ValidationMessages.Field_Descripcion), ResourceType = typeof(ValidationMessages))]
        [StringLength(256,
            ErrorMessageResourceType = typeof(ValidationMessages),
            ErrorMessageResourceName = nameof(ValidationMessages.StringLengthMax)
        )]
        public string NombreNegocio { get; set; } = string.Empty;

        [Display(Name = nameof(ValidationMessages.Field_Direccion), ResourceType = typeof(ValidationMessages))]
        [StringLength(512, ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = nameof(ValidationMessages.StringLengthMax))]
        public string? Direccion { get; set; }

        // NUEVO: IDs dependientes
        [Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = nameof(ValidationMessages.Required))]
        public int PaisId { get; set; }

        [Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = nameof(ValidationMessages.Required))]
        public int ProvinciaId { get; set; }

        [Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = nameof(ValidationMessages.Required))]
        public int MunicipioId { get; set; }

        [Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = nameof(ValidationMessages.Required))]
        public int CodigoPostalId { get; set; }

        // Propiedades de presentación (para el cliente)
        public string? Municipio { get; set; }
        public string? Provincia { get; set; }
        public string? CodPostal { get; set; }

        [Display(Name = nameof(ValidationMessages.Field_Imagen), ResourceType = typeof(ValidationMessages))]
        [StringLength(500, ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = nameof(ValidationMessages.StringLengthMax))]
        public string? Imagen { get; set; }

        [Display(Name = nameof(ValidationMessages.Field_Web), ResourceType = typeof(ValidationMessages))]
        [Url(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = nameof(ValidationMessages.WebFormato))]
        [StringLength(256, ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = nameof(ValidationMessages.StringLengthMax))]
        public string? Web { get; set; }

        [Display(Name = nameof(ValidationMessages.Field_Email1), ResourceType = typeof(ValidationMessages))]
        [EmailAddress(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = nameof(ValidationMessages.EmailFormato))]
        [StringLength(256, ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = nameof(ValidationMessages.StringLengthMax))]
        public string? Email { get; set; }

        [Display(Name = nameof(ValidationMessages.Field_Telefono1), ResourceType = typeof(ValidationMessages))]
        [StringLength(20, ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = nameof(ValidationMessages.TelefonoLongitud))]
        public string? Telefono { get; set; }

        [Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = nameof(ValidationMessages.Required))]
        [Display(Name = nameof(ValidationMessages.Field_CuentaPagar), ResourceType = typeof(ValidationMessages))]
        [Range(1, int.MaxValue, ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = nameof(ValidationMessages.Required))]
        public int? CuentaPagarId { get; set; }

        [Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = nameof(ValidationMessages.Required))]
        [Display(Name = nameof(ValidationMessages.Field_CuentaCobrar), ResourceType = typeof(ValidationMessages))]
        [Range(1, int.MaxValue, ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = nameof(ValidationMessages.Required))]
        public int? CuentaCobrarId { get; set; }

        [Required(ErrorMessageResourceType = typeof(ValidationMessages), ErrorMessageResourceName = nameof(ValidationMessages.Required))]
        [Display(Name = nameof(ValidationMessages.Field_FechaCreacion), ResourceType = typeof(ValidationMessages))]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Caducidad { get; set; }

        public string CreadoPor { get; set; } = string.Empty;
        public DateTime FechaHoraCreado { get; set; }
        public string? ModificadoPor { get; set; }
        public DateTime? FechaHoraModificado { get; set; }

        public string? CuentaPagarCodigo { get; set; } = string.Empty;
        public string? CuentaPagarDescripcion { get; set; } = string.Empty;
        public string? CuentaCobrarCodigo { get; set; } = string.Empty;
        public string? CuentaCobrarDescripcion { get; set; } = string.Empty;

        // Estado (mapeo frontend: Activo = !Cancelado)
        public bool Activo { get; set; } = true;
        public bool Cancelado { get; set; } = false;

        // ─── Propiedades calculadas ───────────────────────────────
        public bool EstaVigente => Caducidad > DateTime.UtcNow;
        public bool EstaVencida => !EstaVigente;
        public bool ProximoAVencer => Caducidad <= DateTime.UtcNow.AddDays(30) && EstaVigente;
        public int DiasRestantes => EstaVigente ? (Caducidad - DateTime.UtcNow).Days : 0;
        public bool TieneImagen => !string.IsNullOrEmpty(Imagen);
        public string EstadoLicencia => EstaVencida ? "Vencida" : (ProximoAVencer ? "Por Vencer" : "Vigente");
        public bool TieneCuentasConfiguradas => CuentaPagarId.HasValue && CuentaCobrarId.HasValue;
    }

    /// <summary>
    /// Resultado devuelto tras subir el logo de la empresa.
    /// </summary>
    public record LogoUploadResult(string FileName);
}
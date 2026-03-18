using System.ComponentModel.DataAnnotations;

namespace GoldBusiness.Domain.DTOs
{
    /// <summary>
    /// DTO para la configuración general del sistema.
    /// Almacena datos del negocio, licencia y cuentas contables por defecto.
    /// </summary>
    public class SystemConfigurationDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El código del sistema es obligatorio")]
        [Display(Name = "Código Sistema")]
        [StringLength(3, ErrorMessage = "El código no puede exceder 3 caracteres")]
        public string CodigoSistema { get; set; } = string.Empty;

        [Required(ErrorMessage = "La licencia es obligatoria")]
        [Display(Name = "Licencia")]
        [StringLength(100, ErrorMessage = "La licencia no puede exceder 100 caracteres")]
        public string Licencia { get; set; } = string.Empty;

        [Required(ErrorMessage = "El nombre del negocio es obligatorio")]
        [Display(Name = "Nombre del Negocio")]
        [StringLength(256, ErrorMessage = "El nombre no puede exceder 256 caracteres")]
        public string NombreNegocio { get; set; } = string.Empty;

        [Display(Name = "Dirección")]
        [StringLength(512, ErrorMessage = "La dirección no puede exceder 512 caracteres")]
        public string? Direccion { get; set; }

        // NUEVO: IDs dependientes
        [Required(ErrorMessage = "El país es obligatorio")]
        public int PaisId { get; set; }

        [Required(ErrorMessage = "La provincia es obligatoria")]
        public int ProvinciaId { get; set; }

        [Required(ErrorMessage = "El municipio es obligatorio")]
        public int MunicipioId { get; set; }

        [Required(ErrorMessage = "El código postal es obligatorio")]
        public int CodigoPostalId { get; set; }

        // Propiedades de presentación (para el cliente)
        public string? Municipio { get; set; }
        public string? Provincia { get; set; }
        public string? CodPostal { get; set; }

        [Display(Name = "Logo del Negocio")]
        [StringLength(500, ErrorMessage = "El nombre de archivo no puede exceder 500 caracteres")]
        public string? Imagen { get; set; }

        [Display(Name = "Sitio Web")]
        [Url(ErrorMessage = "La URL no es válida")]
        [StringLength(256, ErrorMessage = "La URL no puede exceder 256 caracteres")]
        public string? Web { get; set; }

        [Display(Name = "Email")]
        [EmailAddress(ErrorMessage = "El email no es válido")]
        [StringLength(256, ErrorMessage = "El email no puede exceder 256 caracteres")]
        public string? Email { get; set; }

        [Display(Name = "Teléfono")]
        [Phone(ErrorMessage = "El teléfono no es válido")]
        [StringLength(20, ErrorMessage = "El teléfono no puede exceder 20 caracteres")]
        public string? Telefono { get; set; }

        [Required(ErrorMessage = "La cuenta por pagar es obligatoria")]
        [Display(Name = "Cuenta Por Pagar")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar una cuenta válida")]
        public int? CuentaPagarId { get; set; }

        [Required(ErrorMessage = "La cuenta por cobrar es obligatoria")]
        [Display(Name = "Cuenta Por Cobrar")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar una cuenta válida")]
        public int? CuentaCobrarId { get; set; }

        [Required(ErrorMessage = "La fecha de caducidad es obligatoria")]
        [Display(Name = "Caducidad")]
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
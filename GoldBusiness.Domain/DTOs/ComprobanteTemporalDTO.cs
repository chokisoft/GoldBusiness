using System.ComponentModel.DataAnnotations;

namespace GoldBusiness.Domain.DTOs
{
    /// <summary>
    /// DTO para ComprobanteTemporal - Borrador de asiento contable.
    /// Almacena movimientos temporales antes de generar el comprobante definitivo.
    /// </summary>
    public class ComprobanteTemporalDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El establecimiento es obligatorio")]
        [Display(Name = "Establecimiento")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un establecimiento válido")]
        public int EstablecimientoId { get; set; }

        [Required(ErrorMessage = "El código de transacción es obligatorio")]
        [Display(Name = "Código Transacción")]
        [StringLength(10, ErrorMessage = "El código no puede exceder 10 caracteres")]
        public string CodigoTransaccion { get; set; } = string.Empty;

        [Required(ErrorMessage = "La transacción es obligatoria")]
        [Display(Name = "Transacción")]
        [StringLength(256, ErrorMessage = "La transacción no puede exceder 256 caracteres")]
        public string Transaccion { get; set; } = string.Empty;

        [Required(ErrorMessage = "El número de documento es obligatorio")]
        [Display(Name = "No. Documento")]
        [StringLength(50, ErrorMessage = "El número de documento no puede exceder 50 caracteres")]
        public string NoDocumento { get; set; } = string.Empty;

        [Required(ErrorMessage = "La fecha es obligatoria")]
        [Display(Name = "Fecha")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Fecha { get; set; }

        [Required(ErrorMessage = "La cuenta es obligatoria")]
        [Display(Name = "Cuenta")]
        [StringLength(8, ErrorMessage = "La cuenta no puede exceder 8 caracteres")]
        public string Cuenta { get; set; } = string.Empty;

        [Display(Name = "Departamento")]
        [StringLength(50, ErrorMessage = "El departamento no puede exceder 50 caracteres")]
        public string? Departamento { get; set; }

        /// <summary>
        /// Indica si afecta inventario.
        /// </summary>
        [Display(Name = "Afecta Inventario")]
        public bool Inventario { get; set; }

        [Display(Name = "Descripción")]
        [StringLength(512, ErrorMessage = "La descripción no puede exceder 512 caracteres")]
        [DataType(DataType.MultilineText)]
        public string? Descripcion { get; set; }

        [Display(Name = "Débito")]
        [DataType(DataType.Currency)]
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        public decimal? Debito { get; set; }

        [Display(Name = "Crédito")]
        [DataType(DataType.Currency)]
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        public decimal? Credito { get; set; }

        [Display(Name = "Parcial MLC")]
        [DataType(DataType.Currency)]
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        public decimal? Parcial { get; set; }

        public string CreadoPor { get; set; } = string.Empty;
        public DateTime FechaHoraCreado { get; set; }

        /// <summary>
        /// Datos de navegación.
        /// </summary>
        public string EstablecimientoDescripcion { get; set; } = string.Empty;
    }
}
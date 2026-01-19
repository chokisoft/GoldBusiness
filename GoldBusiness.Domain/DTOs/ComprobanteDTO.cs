using System.ComponentModel.DataAnnotations;

namespace GoldBusiness.Domain.DTOs
{
    /// <summary>
    /// DTO para Comprobante - Asiento contable.
    /// Representa un comprobante de diario con sus movimientos.
    /// </summary>
    public class ComprobanteDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El establecimiento es obligatorio")]
        [Display(Name = "Establecimiento")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un establecimiento válido")]
        public int EstablecimientoId { get; set; }

        [Required(ErrorMessage = "El número de comprobante es obligatorio")]
        [Display(Name = "No. Comprobante")]
        [StringLength(50, ErrorMessage = "El número de comprobante no puede exceder 50 caracteres")]
        public string NoComprobante { get; set; } = string.Empty;

        [Required(ErrorMessage = "La fecha es obligatoria")]
        [Display(Name = "Fecha")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Fecha { get; set; }

        [Display(Name = "Observaciones")]
        [StringLength(1024, ErrorMessage = "Las observaciones no pueden exceder 1024 caracteres")]
        [DataType(DataType.MultilineText)]
        public string? Observaciones { get; set; }

        /// <summary>
        /// Indica si fue generado automáticamente por el sistema.
        /// </summary>
        [Display(Name = "Automático")]
        public bool Automatico { get; set; }

        /// <summary>
        /// Indica si el comprobante está posteado (cerrado).
        /// </summary>
        [Display(Name = "Posteado")]
        public bool Posteado { get; set; }

        [Display(Name = "Cancelado")]
        public bool Cancelado { get; set; }

        public string CreadoPor { get; set; } = string.Empty;
        public DateTime FechaHoraCreado { get; set; }
        public string? ModificadoPor { get; set; }
        public DateTime? FechaHoraModificado { get; set; }

        /// <summary>
        /// Datos de navegación.
        /// </summary>
        public string EstablecimientoDescripcion { get; set; } = string.Empty;

        /// <summary>
        /// Propiedades calculadas.
        /// </summary>
        [Display(Name = "Cantidad Detalles")]
        public int CantidadDetalles { get; set; }

        [Display(Name = "Total Débito")]
        [DataType(DataType.Currency)]
        [DisplayFormat(DataFormatString = "{0:C2}")]
        public decimal TotalDebito { get; set; }

        [Display(Name = "Total Crédito")]
        [DataType(DataType.Currency)]
        [DisplayFormat(DataFormatString = "{0:C2}")]
        public decimal TotalCredito { get; set; }

        public bool EstaBalanceado => Math.Abs(TotalDebito - TotalCredito) < 0.01m;
        public string Estado => Cancelado ? "Cancelado" : (Posteado ? "Posteado" : "Pendiente");
        public string TipoGeneracion => Automatico ? "Automático" : "Manual";
    }
}
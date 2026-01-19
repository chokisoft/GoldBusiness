using System.ComponentModel.DataAnnotations;

namespace GoldBusiness.Domain.DTOs
{
    /// <summary>
    /// DTO para ComprobanteDetalle - Línea de movimiento contable.
    /// Representa cada débito o crédito en un comprobante.
    /// </summary>
    public class ComprobanteDetalleDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El comprobante es obligatorio")]
        [Display(Name = "Comprobante")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un comprobante válido")]
        public int ComprobanteId { get; set; }

        [Required(ErrorMessage = "La cuenta es obligatoria")]
        [Display(Name = "Cuenta")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar una cuenta válida")]
        public int CuentaId { get; set; }

        [Display(Name = "Departamento")]
        [StringLength(50, ErrorMessage = "El departamento no puede exceder 50 caracteres")]
        public string? Departamento { get; set; }

        /// <summary>
        /// Importe débito (debe).
        /// </summary>
        [Display(Name = "Débito")]
        [DataType(DataType.Currency)]
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        [Range(0, double.MaxValue, ErrorMessage = "El débito debe ser mayor o igual a cero")]
        public decimal Debito { get; set; }

        /// <summary>
        /// Importe crédito (haber).
        /// </summary>
        [Display(Name = "Crédito")]
        [DataType(DataType.Currency)]
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        [Range(0, double.MaxValue, ErrorMessage = "El crédito debe ser mayor o igual a cero")]
        public decimal Credito { get; set; }

        /// <summary>
        /// Parcial en moneda extranjera.
        /// </summary>
        [Display(Name = "Parcial MLC")]
        [DataType(DataType.Currency)]
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        public decimal Parcial { get; set; }

        [Display(Name = "Nota")]
        [StringLength(512, ErrorMessage = "La nota no puede exceder 512 caracteres")]
        [DataType(DataType.MultilineText)]
        public string? Nota { get; set; }

        [Display(Name = "Cancelado")]
        public bool Cancelado { get; set; }

        public string CreadoPor { get; set; } = string.Empty;
        public DateTime FechaHoraCreado { get; set; }
        public string? ModificadoPor { get; set; }
        public DateTime? FechaHoraModificado { get; set; }

        /// <summary>
        /// Datos de navegación.
        /// </summary>
        public string CuentaCodigo { get; set; } = string.Empty;
        public string CuentaDescripcion { get; set; } = string.Empty;
    }
}
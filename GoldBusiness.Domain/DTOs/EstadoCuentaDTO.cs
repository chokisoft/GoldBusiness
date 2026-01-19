using System.ComponentModel.DataAnnotations;

namespace GoldBusiness.Domain.DTOs
{
    /// <summary>
    /// DTO para EstadoCuenta - Movimiento contable en una cuenta.
    /// Representa cada asiento que afecta el saldo de una cuenta.
    /// </summary>
    public class EstadoCuentaDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El establecimiento es obligatorio")]
        [Display(Name = "Establecimiento")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un establecimiento válido")]
        public int EstablecimientoId { get; set; }

        [Required(ErrorMessage = "La cuenta es obligatoria")]
        [Display(Name = "Cuenta")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar una cuenta válida")]
        public int CuentaId { get; set; }

        [Required(ErrorMessage = "La fecha es obligatoria")]
        [Display(Name = "Fecha")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Fecha { get; set; }

        [Display(Name = "Débito")]
        [DataType(DataType.Currency)]
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        public decimal Debito { get; set; }

        [Display(Name = "Crédito")]
        [DataType(DataType.Currency)]
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        public decimal Credito { get; set; }

        [Display(Name = "Saldo")]
        [DataType(DataType.Currency)]
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        public decimal Saldo { get; set; }

        [Display(Name = "Referencia")]
        [StringLength(256, ErrorMessage = "La referencia no puede exceder 256 caracteres")]
        public string? Referencia { get; set; }

        public string CreadoPor { get; set; } = string.Empty;
        public DateTime FechaHoraCreado { get; set; }

        /// <summary>
        /// Datos de navegación.
        /// </summary>
        public string EstablecimientoDescripcion { get; set; } = string.Empty;
        public string CuentaCodigo { get; set; } = string.Empty;
        public string CuentaDescripcion { get; set; } = string.Empty;

        /// <summary>
        /// Movimiento neto (débito - crédito).
        /// </summary>
        public decimal Movimiento => Debito - Credito;
    }
}
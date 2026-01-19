using System.ComponentModel.DataAnnotations;

namespace GoldBusiness.Domain.DTOs
{
    /// <summary>
    /// DTO para IdTurno - Turno de caja registradora.
    /// Representa un turno de trabajo de un cajero con apertura y cierre.
    /// </summary>
    public class IdTurnoDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "La fecha es obligatoria")]
        [Display(Name = "Fecha")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Fecha { get; set; }

        [Required(ErrorMessage = "El cajero es obligatorio")]
        [Display(Name = "Cajero")]
        [StringLength(256, ErrorMessage = "El nombre del cajero no puede exceder 256 caracteres")]
        public string Cajero { get; set; } = string.Empty;

        [Required(ErrorMessage = "La hora de inicio es obligatoria")]
        [Display(Name = "Hora Inicio")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}", ApplyFormatInEditMode = false)]
        public DateTime Inicio { get; set; }

        /// <summary>
        /// Fondo inicial de caja.
        /// </summary>
        [Display(Name = "Fondo Inicial")]
        [DataType(DataType.Currency)]
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        [Range(0, double.MaxValue, ErrorMessage = "El fondo debe ser mayor o igual a cero")]
        public decimal? Fondo { get; set; }

        /// <summary>
        /// Extracción de efectivo al cerrar.
        /// </summary>
        [Display(Name = "Extracción")]
        [DataType(DataType.Currency)]
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        [Range(0, double.MaxValue, ErrorMessage = "La extracción debe ser mayor o igual a cero")]
        public decimal? Extraccion { get; set; }

        [Display(Name = "Hora Cierre")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}", ApplyFormatInEditMode = false)]
        public DateTime? Cierre { get; set; }

        public string CreadoPor { get; set; } = string.Empty;
        public DateTime FechaHoraCreado { get; set; }
        public string? ModificadoPor { get; set; }
        public DateTime? FechaHoraModificado { get; set; }

        /// <summary>
        /// Propiedades calculadas.
        /// </summary>
        public bool EstaCerrado => Cierre.HasValue;
        public TimeSpan? Duracion => Cierre.HasValue ? Cierre.Value - Inicio : null;
        public string Estado => EstaCerrado ? "Cerrado" : "Abierto";
    }
}
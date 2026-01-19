using System.ComponentModel.DataAnnotations;

namespace GoldBusiness.Domain.DTOs
{
    /// <summary>
    /// DTO para ConceptoAjuste - Motivo de ajustes de inventario.
    /// Define conceptos contables para ajustes positivos o negativos.
    /// </summary>
    public class ConceptoAjusteDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El código es obligatorio")]
        [Display(Name = "Código")]
        [StringLength(10, ErrorMessage = "El código no puede exceder 10 caracteres")]
        public string Codigo { get; set; } = string.Empty;

        [Required(ErrorMessage = "La descripción es obligatoria")]
        [Display(Name = "Descripción")]
        [StringLength(256, ErrorMessage = "La descripción no puede exceder 256 caracteres")]
        public string Descripcion { get; set; } = string.Empty;

        /// <summary>
        /// Cuenta contable asociada al ajuste.
        /// </summary>
        [Required(ErrorMessage = "La cuenta es obligatoria")]
        [Display(Name = "Cuenta")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar una cuenta válida")]
        public int CuentaId { get; set; }

        [Display(Name = "Cancelado")]
        public bool Cancelado { get; set; }

        public string CreadoPor { get; set; } = string.Empty;
        public DateTime FechaHoraCreado { get; set; }
        public string? ModificadoPor { get; set; }
        public DateTime? FechaHoraModificado { get; set; }

        /// <summary>
        /// Datos de la cuenta.
        /// </summary>
        public string CuentaCodigo { get; set; } = string.Empty;
        public string CuentaDescripcion { get; set; } = string.Empty;
    }
}
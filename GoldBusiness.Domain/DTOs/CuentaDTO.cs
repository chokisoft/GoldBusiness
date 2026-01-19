using System.ComponentModel.DataAnnotations;

namespace GoldBusiness.Domain.DTOs
{
    /// <summary>
    /// DTO para Cuenta - Cuenta contable individual.
    /// Nivel más detallado donde se registran transacciones.
    /// </summary>
    public class CuentaDTO
    {
        public int Id { get; set; }

        /// <summary>
        /// Código de la cuenta (formato: ##-##-##).
        /// Ejemplo: "01-01-01" = CAJA GENERAL.
        /// </summary>
        [Required(ErrorMessage = "El código es obligatorio")]
        [Display(Name = "Código")]
        [StringLength(8, ErrorMessage = "El código no puede exceder 8 caracteres")]
        [RegularExpression(@"^\d{2}-\d{2}-\d{2}$", ErrorMessage = "Formato debe ser ##-##-## (Ejemplo: 01-01-01)")]
        public string Codigo { get; set; } = string.Empty;

        [Required(ErrorMessage = "La descripción es obligatoria")]
        [Display(Name = "Descripción")]
        [StringLength(256, ErrorMessage = "La descripción no puede exceder 256 caracteres")]
        public string Descripcion { get; set; } = string.Empty;

        [Required(ErrorMessage = "El subgrupo es obligatorio")]
        [Display(Name = "Subgrupo")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un subgrupo válido")]
        public int SubGrupoCuentaId { get; set; }

        [Display(Name = "Cancelado")]
        public bool Cancelado { get; set; }

        public string CreadoPor { get; set; } = string.Empty;
        public DateTime FechaHoraCreado { get; set; }
        public string? ModificadoPor { get; set; }
        public DateTime? FechaHoraModificado { get; set; }

        /// <summary>
        /// Jerarquía completa.
        /// </summary>
        public string SubGrupoCuentaCodigo { get; set; } = string.Empty;
        public string SubGrupoCuentaDescripcion { get; set; } = string.Empty;
        public string GrupoCuentaCodigo { get; set; } = string.Empty;
        public string GrupoCuentaDescripcion { get; set; } = string.Empty;

        public string CodigoDescripcion => $"{Codigo} | {Descripcion}";
    }
}
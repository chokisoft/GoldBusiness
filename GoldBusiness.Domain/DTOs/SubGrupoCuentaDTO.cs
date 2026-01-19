using System.ComponentModel.DataAnnotations;

namespace GoldBusiness.Domain.DTOs
{
    /// <summary>
    /// DTO para SubGrupoCuenta - Segundo nivel del plan de cuentas.
    /// Agrupa cuentas contables específicas dentro de un grupo.
    /// </summary>
    public class SubGrupoCuentaDTO
    {
        public int Id { get; set; }

        /// <summary>
        /// Código del subgrupo (formato: ##-##).
        /// Ejemplo: "01-01" = ACTIVO CIRCULANTE.
        /// </summary>
        [Required(ErrorMessage = "El código es obligatorio")]
        [Display(Name = "Código")]
        [StringLength(5, MinimumLength = 5, ErrorMessage = "El código debe tener exactamente 5 caracteres")]
        [RegularExpression(@"^\d{2}-\d{2}$", ErrorMessage = "El código debe tener el formato ##-## (Ejemplo: 01-01)")]
        public string Codigo { get; set; } = string.Empty;

        /// <summary>
        /// Descripción del subgrupo.
        /// </summary>
        [Required(ErrorMessage = "La descripción es obligatoria")]
        [Display(Name = "Descripción")]
        [StringLength(256, ErrorMessage = "La descripción no puede exceder 256 caracteres")]
        public string Descripcion { get; set; } = string.Empty;

        /// <summary>
        /// ID del grupo padre.
        /// </summary>
        [Required(ErrorMessage = "El grupo de cuenta es obligatorio")]
        [Display(Name = "Grupo de Cuenta")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un grupo válido")]
        public int GrupoCuentaId { get; set; }

        /// <summary>
        /// Indica si las cuentas de este subgrupo son deudoras.
        /// </summary>
        [Display(Name = "¿Es Deudora?")]
        public bool Deudora { get; set; }  // ✅ AGREGADO

        [Display(Name = "Cancelado")]
        public bool Cancelado { get; set; }

        public string CreadoPor { get; set; } = string.Empty;
        public DateTime FechaHoraCreado { get; set; }
        public string? ModificadoPor { get; set; }
        public DateTime? FechaHoraModificado { get; set; }

        /// <summary>
        /// Datos del grupo padre.
        /// </summary>
        public string GrupoCuentaCodigo { get; set; } = string.Empty;
        public string GrupoCuentaDescripcion { get; set; } = string.Empty;

        /// <summary>
        /// Cantidad de cuentas.
        /// </summary>
        [Display(Name = "Cuentas")]
        public int CantidadCuentas { get; set; }

        public string CodigoDescripcion => $"{Codigo} | {Descripcion}";
    }
}
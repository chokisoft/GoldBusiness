using System.ComponentModel.DataAnnotations;

namespace GoldBusiness.Domain.DTOs
{
    /// <summary>
    /// DTO para GrupoCuenta - Nivel superior del plan de cuentas.
    /// Representa grupos contables principales: Activo, Pasivo, Patrimonio, Ingresos, Gastos.
    /// </summary>
    public class GrupoCuentaDTO
    {
        /// <summary>
        /// Identificador único del grupo de cuenta.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Código del grupo (2 dígitos exactos).
        /// Ejemplos: "01" = ACTIVO, "02" = PASIVO, "03" = PATRIMONIO.
        /// </summary>
        [Required(ErrorMessage = "El código es obligatorio")]
        [Display(Name = "Código")]
        [StringLength(2, MinimumLength = 2, ErrorMessage = "El código debe tener exactamente 2 caracteres")]
        [RegularExpression(@"^\d{2}$", ErrorMessage = "El código debe contener solo números (00-99)")]
        public string Codigo { get; set; } = string.Empty;

        /// <summary>
        /// Descripción del grupo de cuenta.
        /// </summary>
        [Required(ErrorMessage = "La descripción es obligatoria")]
        [Display(Name = "Descripción")]
        [StringLength(256, ErrorMessage = "La descripción no puede exceder 256 caracteres")]
        public string Descripcion { get; set; } = string.Empty;

        /// <summary>
        /// Indica si el grupo está cancelado/inactivo.
        /// </summary>
        [Display(Name = "Cancelado")]
        public bool Cancelado { get; set; }

        /// <summary>
        /// Usuario que creó el registro.
        /// </summary>
        [Display(Name = "Creado Por")]
        public string CreadoPor { get; set; } = string.Empty;

        /// <summary>
        /// Fecha y hora de creación.
        /// </summary>
        [Display(Name = "Fecha Creación")]
        public DateTime FechaHoraCreado { get; set; }

        /// <summary>
        /// Usuario que modificó el registro.
        /// </summary>
        [Display(Name = "Modificado Por")]
        public string? ModificadoPor { get; set; }

        /// <summary>
        /// Fecha y hora de modificación.
        /// </summary>
        [Display(Name = "Fecha Modificación")]
        public DateTime? FechaHoraModificado { get; set; }

        /// <summary>
        /// Cantidad de subgrupos asociados.
        /// </summary>
        [Display(Name = "Subgrupos")]
        public int CantidadSubGrupos { get; set; }

        /// <summary>
        /// Formato: "Código | Descripción".
        /// </summary>
        public string CodigoDescripcion => $"{Codigo} | {Descripcion}";
    }
}
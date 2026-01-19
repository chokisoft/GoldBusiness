using System.ComponentModel.DataAnnotations;

namespace GoldBusiness.Domain.DTOs
{
    /// <summary>
    /// DTO para Localidad - Ubicación física dentro de un establecimiento.
    /// Representa almacenes, bodegas, salones de venta, etc.
    /// </summary>
    public class LocalidadDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El establecimiento es obligatorio")]
        [Display(Name = "Establecimiento")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un establecimiento válido")]
        public int EstablecimientoId { get; set; }

        /// <summary>
        /// Código único de la localidad (9 caracteres).
        /// Formato: código establecimiento + secuencia.
        /// </summary>
        [Required(ErrorMessage = "El código es obligatorio")]
        [Display(Name = "Código")]
        [StringLength(9, MinimumLength = 9, ErrorMessage = "El código debe tener exactamente 9 caracteres")]
        public string Codigo { get; set; } = string.Empty;

        [Required(ErrorMessage = "La descripción es obligatoria")]
        [Display(Name = "Descripción")]
        [StringLength(256, ErrorMessage = "La descripción no puede exceder 256 caracteres")]
        public string Descripcion { get; set; } = string.Empty;

        /// <summary>
        /// Cuentas contables asociadas a esta localidad.
        /// </summary>
        [Required(ErrorMessage = "La cuenta de inventario es obligatoria")]
        [Display(Name = "Cuenta Inventario")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar una cuenta válida")]
        public int CuentaInventarioId { get; set; }

        [Required(ErrorMessage = "La cuenta de costo es obligatoria")]
        [Display(Name = "Cuenta Costo")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar una cuenta válida")]
        public int CuentaCostoId { get; set; }

        [Required(ErrorMessage = "La cuenta de venta es obligatoria")]
        [Display(Name = "Cuenta Venta")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar una cuenta válida")]
        public int CuentaVentaId { get; set; }

        [Required(ErrorMessage = "La cuenta de devolución es obligatoria")]
        [Display(Name = "Cuenta Devolución")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar una cuenta válida")]
        public int CuentaDevolucionId { get; set; }

        [Display(Name = "Activo")]
        public bool Activo { get; set; }

        [Display(Name = "Cancelado")]
        public bool Cancelado { get; set; }

        public string CreadoPor { get; set; } = string.Empty;
        public DateTime FechaHoraCreado { get; set; }
        public string? ModificadoPor { get; set; }
        public DateTime? FechaHoraModificado { get; set; }

        /// <summary>
        /// Datos de navegación.
        /// </summary>
        public string EstablecimientoCodigo { get; set; } = string.Empty;
        public string EstablecimientoDescripcion { get; set; } = string.Empty;
        public string CuentaInventarioCodigo { get; set; } = string.Empty;
        public string CuentaCostoCodigo { get; set; } = string.Empty;
        public string CuentaVentaCodigo { get; set; } = string.Empty;
        public string CuentaDevolucionCodigo { get; set; } = string.Empty;

        public string CodigoDescripcion => $"{Codigo} | {Descripcion}";
        public string Estado => Cancelado ? "Cancelado" : (Activo ? "Activo" : "Inactivo");
    }
}
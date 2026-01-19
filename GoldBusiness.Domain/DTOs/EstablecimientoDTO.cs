using System.ComponentModel.DataAnnotations;

namespace GoldBusiness.Domain.DTOs
{
    /// <summary>
    /// DTO para Establecimiento - Representa sucursales o puntos de venta del negocio.
    /// Agrupa localidades físicas donde se realizan operaciones.
    /// </summary>
    public class EstablecimientoDTO
    {
        public int Id { get; set; }

        /// <summary>
        /// ID de la configuración (negocio) al que pertenece.
        /// </summary>
        [Required(ErrorMessage = "El negocio es obligatorio")]
        [Display(Name = "Negocio")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un negocio válido")]
        public int NegocioId { get; set; }

        /// <summary>
        /// Código único del establecimiento (6 caracteres).
        /// Ejemplo: "EST001", "SUC001".
        /// </summary>
        [Required(ErrorMessage = "El código es obligatorio")]
        [Display(Name = "Código")]
        [StringLength(6, MinimumLength = 6, ErrorMessage = "El código debe tener exactamente 6 caracteres")]
        public string Codigo { get; set; } = string.Empty;

        /// <summary>
        /// Descripción del establecimiento.
        /// Ejemplo: "Sucursal Centro", "Almacén Principal".
        /// </summary>
        [Required(ErrorMessage = "La descripción es obligatoria")]
        [Display(Name = "Descripción")]
        [StringLength(256, ErrorMessage = "La descripción no puede exceder 256 caracteres")]
        public string Descripcion { get; set; } = string.Empty;

        /// <summary>
        /// Indica si el establecimiento está activo para operaciones.
        /// </summary>
        [Display(Name = "Activo")]
        public bool Activo { get; set; }

        /// <summary>
        /// Indica si el establecimiento está cancelado.
        /// </summary>
        [Display(Name = "Cancelado")]
        public bool Cancelado { get; set; }

        public string CreadoPor { get; set; } = string.Empty;
        public DateTime FechaHoraCreado { get; set; }
        public string? ModificadoPor { get; set; }
        public DateTime? FechaHoraModificado { get; set; }

        /// <summary>
        /// Datos del negocio padre.
        /// </summary>
        public string NegocioDescripcion { get; set; } = string.Empty;

        /// <summary>
        /// Cantidad de localidades.
        /// </summary>
        [Display(Name = "Localidades")]
        public int CantidadLocalidades { get; set; }

        /// <summary>
        /// Cantidad de productos.
        /// </summary>
        [Display(Name = "Productos")]
        public int CantidadProductos { get; set; }

        public string CodigoDescripcion => $"{Codigo} | {Descripcion}";
        public string Estado => Cancelado ? "Cancelado" : (Activo ? "Activo" : "Inactivo");
    }
}
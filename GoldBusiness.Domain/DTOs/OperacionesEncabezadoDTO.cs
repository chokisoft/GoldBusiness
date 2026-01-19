using System.ComponentModel.DataAnnotations;

namespace GoldBusiness.Domain.DTOs
{
    /// <summary>
    /// DTO para OperacionesEncabezado - Cabecera de operaciones de inventario.
    /// Representa una transacción de compra, venta, ajuste, etc.
    /// </summary>
    public class OperacionesEncabezadoDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El establecimiento es obligatorio")]
        [Display(Name = "Establecimiento")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un establecimiento válido")]
        public int EstablecimientoId { get; set; }

        [Required(ErrorMessage = "La transacción es obligatoria")]
        [Display(Name = "Transacción")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar una transacción válida")]
        public int TransaccionId { get; set; }

        [Required(ErrorMessage = "El número de documento es obligatorio")]
        [Display(Name = "No. Documento")]
        [StringLength(50, ErrorMessage = "El número de documento no puede exceder 50 caracteres")]
        public string NoDocumento { get; set; } = string.Empty;

        [Required(ErrorMessage = "La fecha es obligatoria")]
        [Display(Name = "Fecha")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Fecha { get; set; }

        [Display(Name = "Proveedor")]
        public int? ProveedorId { get; set; }

        [Display(Name = "Cliente")]
        public int? ClienteId { get; set; }

        [Display(Name = "No. Primario")]
        [StringLength(50, ErrorMessage = "El número primario no puede exceder 50 caracteres")]
        public string? NoPrimario { get; set; }

        [Display(Name = "Referencia")]
        public int? ReferenciaId { get; set; }

        [Display(Name = "Concepto de Ajuste")]
        public int? ConceptoAjusteId { get; set; }

        [Display(Name = "Concepto")]
        [StringLength(512, ErrorMessage = "El concepto no puede exceder 512 caracteres")]
        [DataType(DataType.MultilineText)]
        public string? Concepto { get; set; }

        [Display(Name = "Observaciones")]
        [StringLength(1024, ErrorMessage = "Las observaciones no pueden exceder 1024 caracteres")]
        [DataType(DataType.MultilineText)]
        public string? Observaciones { get; set; }

        [Display(Name = "Efectivo")]
        public bool Efectivo { get; set; }

        [Display(Name = "Contabilizada")]
        public bool Contabilizada { get; set; }

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
        public string TransaccionDescripcion { get; set; } = string.Empty;
        public string? ProveedorDescripcion { get; set; }
        public string? ClienteDescripcion { get; set; }
        public string? ConceptoAjusteDescripcion { get; set; }

        /// <summary>
        /// Propiedades calculadas.
        /// </summary>
        [Display(Name = "Cantidad Detalles")]
        public int CantidadDetalles { get; set; }

        [Display(Name = "Total Costo")]
        [DataType(DataType.Currency)]
        [DisplayFormat(DataFormatString = "{0:C2}")]
        public decimal TotalImporteCosto { get; set; }

        [Display(Name = "Total Venta")]
        [DataType(DataType.Currency)]
        [DisplayFormat(DataFormatString = "{0:C2}")]
        public decimal TotalImporteVenta { get; set; }

        public string Estado => Cancelado ? "Cancelado" : (Contabilizada ? "Contabilizada" : "Pendiente");
    }
}
using System.ComponentModel.DataAnnotations;

namespace GoldBusiness.Domain.DTOs
{
    /// <summary>
    /// DTO para Saldo - Existencia actual de producto en una localidad.
    /// Representa el inventario físico disponible.
    /// </summary>
    public class SaldoDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "La localidad es obligatoria")]
        [Display(Name = "Localidad")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar una localidad válida")]
        public int LocalidadId { get; set; }

        [Required(ErrorMessage = "El producto es obligatorio")]
        [Display(Name = "Producto")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un producto válido")]
        public int ProductoId { get; set; }

        /// <summary>
        /// Cantidad en existencia.
        /// </summary>
        [Display(Name = "Existencia")]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = false)]
        public decimal Existencia { get; set; }

        /// <summary>
        /// Fecha de la última actualización del saldo.
        /// </summary>
        [Display(Name = "Fecha")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}", ApplyFormatInEditMode = false)]
        public DateTime Fecha { get; set; }

        public string CreadoPor { get; set; } = string.Empty;
        public DateTime FechaHoraCreado { get; set; }
        public string? ModificadoPor { get; set; }
        public DateTime? FechaHoraModificado { get; set; }

        /// <summary>
        /// Datos de navegación.
        /// </summary>
        public string LocalidadCodigo { get; set; } = string.Empty;
        public string LocalidadDescripcion { get; set; } = string.Empty;
        public string ProductoCodigo { get; set; } = string.Empty;
        public string ProductoDescripcion { get; set; } = string.Empty;
        public string UnidadMedida { get; set; } = string.Empty;
        public decimal StockMinimo { get; set; }

        /// <summary>
        /// Propiedades calculadas.
        /// </summary>
        public bool BajoStockMinimo => Existencia < StockMinimo;
        public bool SinExistencia => Existencia <= 0;
        public string EstadoStock => SinExistencia ? "Sin Stock" : (BajoStockMinimo ? "Stock Bajo" : "Normal");
    }
}
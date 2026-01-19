using System.ComponentModel.DataAnnotations;

namespace GoldBusiness.Domain.DTOs
{
    /// <summary>
    /// DTO para SaldoAnterior - Histórico de saldos de inventario.
    /// Registra existencias en puntos específicos del tiempo.
    /// </summary>
    public class SaldoAnteriorDTO
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

        [Display(Name = "Existencia")]
        [DisplayFormat(DataFormatString = "{0:N3}", ApplyFormatInEditMode = false)]
        public decimal Existencia { get; set; }

        [Display(Name = "Importe Costo")]
        [DataType(DataType.Currency)]
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        public decimal ImporteCosto { get; set; }

        [Display(Name = "Precio Costo")]
        [DataType(DataType.Currency)]
        [DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
        public decimal PrecioCosto { get; set; }

        [Required(ErrorMessage = "La fecha es obligatoria")]
        [Display(Name = "Fecha")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Fecha { get; set; }

        public string CreadoPor { get; set; } = string.Empty;
        public DateTime FechaHoraCreado { get; set; }
        public string? ModificadoPor { get; set; }
        public DateTime? FechaHoraModificado { get; set; }

        /// <summary>
        /// Datos de navegación.
        /// </summary>
        public string LocalidadDescripcion { get; set; } = string.Empty;
        public string ProductoCodigo { get; set; } = string.Empty;
        public string ProductoDescripcion { get; set; } = string.Empty;
        public string UnidadMedida { get; set; } = string.Empty;

        /// <summary>
        /// Costo unitario promedio.
        /// </summary>
        public decimal CostoUnitario => Existencia != 0 ? ImporteCosto / Existencia : 0;
    }
}
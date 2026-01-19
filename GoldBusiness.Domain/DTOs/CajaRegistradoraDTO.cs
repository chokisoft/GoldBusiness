using System.ComponentModel.DataAnnotations;

namespace GoldBusiness.Domain.DTOs
{
    /// <summary>
    /// DTO para CajaRegistradora - Orden o mesa de venta.
    /// Agrupa productos vendidos en una transacción POS.
    /// </summary>
    public class CajaRegistradoraDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El turno es obligatorio")]
        [Display(Name = "Turno")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un turno válido")]
        public int IdTurnoId { get; set; }

        /// <summary>
        /// Número de mesa (opcional para restaurantes).
        /// </summary>
        [Display(Name = "Mesa")]
        [Range(1, int.MaxValue, ErrorMessage = "El número de mesa debe ser mayor que cero")]
        public int? Mesa { get; set; }

        /// <summary>
        /// Indica si la orden está cerrada (facturada).
        /// </summary>
        [Display(Name = "Cerrado")]
        public bool Cerrado { get; set; }

        public string CreadoPor { get; set; } = string.Empty;
        public DateTime FechaHoraCreado { get; set; }
        public string? ModificadoPor { get; set; }
        public DateTime? FechaHoraModificado { get; set; }

        /// <summary>
        /// Datos de navegación.
        /// </summary>
        public string TurnoCajero { get; set; } = string.Empty;
        public DateTime TurnoFecha { get; set; }

        /// <summary>
        /// Propiedades calculadas.
        /// </summary>
        [Display(Name = "Cantidad Artículos")]
        public int CantidadArticulos { get; set; }

        [Display(Name = "Total Venta")]
        [DataType(DataType.Currency)]
        [DisplayFormat(DataFormatString = "{0:C2}")]
        public decimal TotalVenta { get; set; }

        public string Estado => Cerrado ? "Cerrado" : "Abierto";
        public string Identificador => Mesa.HasValue ? $"Mesa {Mesa}" : $"Orden #{Id}";
    }
}
using System.ComponentModel.DataAnnotations;

namespace GoldBusiness.Domain.DTOs
{
    /// <summary>
    /// DTO para Transacción - Tipo de operación de inventario.
    /// Ejemplos: Compra, Venta, Ajuste, Devolución, Transferencia.
    /// </summary>
    public class TransaccionDTO
    {
        public int Id { get; set; }

        /// <summary>
        /// Código de la transacción (hasta 10 caracteres).
        /// </summary>
        [Required(ErrorMessage = "El código es obligatorio")]
        [Display(Name = "Código")]
        [StringLength(10, ErrorMessage = "El código no puede exceder 10 caracteres")]
        public string Codigo { get; set; } = string.Empty;

        [Required(ErrorMessage = "La descripción es obligatoria")]
        [Display(Name = "Descripción")]
        [StringLength(256, ErrorMessage = "La descripción no puede exceder 256 caracteres")]
        public string Descripcion { get; set; } = string.Empty;

        public string CreadoPor { get; set; } = string.Empty;
        public DateTime FechaHoraCreado { get; set; }
        public string? ModificadoPor { get; set; }
        public DateTime? FechaHoraModificado { get; set; }
    }
}
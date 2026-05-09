namespace GoldBusiness.Domain.DTOs
{
    /// <summary>
    /// DTO para actividades recientes del dashboard
    /// Soporta múltiples tipos: cuentas, ventas, compras, clientes, proveedores, productos, etc.
    /// </summary>
    public class RecentActivityDTO
    {
        public int Id { get; set; }
        public string Icon { get; set; } = string.Empty;
        public string ActionType { get; set; } = string.Empty;
        public string TargetName { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string EntityType { get; set; } = string.Empty; // "cuenta", "venta", "compra", "cliente", "proveedor", "producto"
        public decimal? Amount { get; set; } // Para ventas/compras
    }
}
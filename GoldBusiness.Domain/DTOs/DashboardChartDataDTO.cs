namespace GoldBusiness.Domain.DTOs
{
    /// <summary>
    /// DTO que contiene todos los datos para las gráficas del dashboard
    /// </summary>
    public class DashboardChartDataDTO
    {
        public SalesChartDTO SalesChart { get; set; } = new();
        public TopProductsChartDTO TopProducts { get; set; } = new();
        public InventoryStatusChartDTO InventoryStatus { get; set; } = new();
        public ReceivablePayableChartDTO ReceivablePayable { get; set; } = new();
        public ClientTrendChartDTO ClientTrend { get; set; } = new();
    }

    /// <summary>
    /// Datos para gráfica de ventas vs compras (últimos 12 meses)
    /// </summary>
    public class SalesChartDTO
    {
        public List<string> Labels { get; set; } = new(); // ["Ene", "Feb", "Mar", ...]
        public List<decimal> Sales { get; set; } = new();
        public List<decimal> Purchases { get; set; } = new();
    }

    /// <summary>
    /// Top 10 productos más vendidos
    /// </summary>
    public class TopProductsChartDTO
    {
        public List<string> ProductNames { get; set; } = new();
        public List<int> Quantities { get; set; } = new();
        public List<decimal> TotalSales { get; set; } = new();
    }

    /// <summary>
    /// Estado del inventario (en stock, bajo stock, sin stock)
    /// </summary>
    public class InventoryStatusChartDTO
    {
        public int InStock { get; set; }
        public int LowStock { get; set; }
        public int OutOfStock { get; set; }
    }

    /// <summary>
    /// Distribución de cuentas por cobrar y pagar
    /// </summary>
    public class ReceivablePayableChartDTO
    {
        // Cuentas por Cobrar
        public decimal ReceivableCurrent { get; set; } // Al día
        public decimal ReceivableOverdue1_30 { get; set; } // Vencidas 1-30 días
        public decimal ReceivableOverdue30Plus { get; set; } // Vencidas +30 días

        // Cuentas por Pagar
        public decimal PayableCurrent { get; set; } // Al día
        public decimal PayableOverdue1_30 { get; set; } // Vencidas 1-30 días
        public decimal PayableOverdue30Plus { get; set; } // Vencidas +30 días
    }

    /// <summary>
    /// Tendencia de nuevos clientes (últimos 6 meses)
    /// </summary>
    public class ClientTrendChartDTO
    {
        public List<string> Labels { get; set; } = new(); // Meses
        public List<int> NewClients { get; set; } = new();
    }
}

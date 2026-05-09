namespace GoldBusiness.Domain.DTOs
{
    public class DashboardStatsDTO
    {
        // ═══════════════════════════════════════════════════════════════
        // 📊 CONTABILIDAD (Existente)
        // ═══════════════════════════════════════════════════════════════
        public int TotalAccounts { get; set; }
        public string AccountsChange { get; set; } = string.Empty;
        public string AccountsChangeType { get; set; } = string.Empty;

        public int ActiveUsers { get; set; }
        public string UsersChange { get; set; } = string.Empty;
        public string UsersChangeType { get; set; } = string.Empty;

        public int AccountGroups { get; set; }
        public string GroupsChange { get; set; } = string.Empty;
        public string GroupsChangeType { get; set; } = string.Empty;

        public int PendingTasks { get; set; }
        public string TasksChange { get; set; } = string.Empty;
        public string TasksChangeType { get; set; } = string.Empty;

        // ═══════════════════════════════════════════════════════════════
        // 📦 INVENTARIO
        // ═══════════════════════════════════════════════════════════════
        public int TotalProducts { get; set; }
        public string ProductsChange { get; set; } = string.Empty;
        public string ProductsChangeType { get; set; } = string.Empty;

        public int LowStockProducts { get; set; }
        public decimal TotalInventoryValue { get; set; }
        
        // ═══════════════════════════════════════════════════════════════
        // 👥 CLIENTES Y PROVEEDORES
        // ═══════════════════════════════════════════════════════════════
        public int TotalClients { get; set; }
        public string ClientsChange { get; set; } = string.Empty;
        public string ClientsChangeType { get; set; } = string.Empty;

        public int NewClientsLastMonth { get; set; }
        
        public int TotalSuppliers { get; set; }
        public string SuppliersChange { get; set; } = string.Empty;
        public string SuppliersChangeType { get; set; } = string.Empty;

        // ═══════════════════════════════════════════════════════════════
        // 💰 FINANZAS
        // ═══════════════════════════════════════════════════════════════
        public decimal TotalReceivable { get; set; }
        public decimal TotalPayable { get; set; }
        public decimal OverdueReceivable { get; set; }
        public decimal OverduePayable { get; set; }
        
        // ═══════════════════════════════════════════════════════════════
        // 🛒 OPERACIONES
        // ═══════════════════════════════════════════════════════════════
        public decimal SalesToday { get; set; }
        public decimal SalesThisMonth { get; set; }
        public string SalesMonthChange { get; set; } = string.Empty;
        public string SalesMonthChangeType { get; set; } = string.Empty;

        public int TransactionsToday { get; set; }
        public decimal AverageTicket { get; set; }
    }
}
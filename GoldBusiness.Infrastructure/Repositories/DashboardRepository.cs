using GoldBusiness.Domain.DTOs;
using GoldBusiness.Domain.Entities;
using GoldBusiness.Infrastructure.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GoldBusiness.Infrastructure.Repositories
{
    public class DashboardRepository(
        ApplicationDbContext context,
        UserManager<ApplicationUser> userManager,
        ILogger<DashboardRepository> logger) : IDashboardRepository
    {
        private readonly ApplicationDbContext _context = context;
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly ILogger<DashboardRepository> _logger = logger;

        public async Task<DashboardStatsDTO> GetStatsAsync()
        {
            try
            {
                var today = DateTime.UtcNow.Date;
                var lastMonth = today.AddMonths(-1);
                var firstDayOfMonth = new DateTime(today.Year, today.Month, 1);

                _logger.LogInformation("?? Obteniendo estad�sticas simplificadas del dashboard...");

                // CONTABILIDAD
                var totalAccounts = await _context.Cuenta.Where(c => !c.Cancelado).CountAsync();
                var activeUsers = await _userManager.Users.CountAsync();
                var accountGroups = await _context.GrupoCuenta.Where(g => !g.Cancelado).CountAsync();
                var pendingTasks = await _context.Transaccion.CountAsync();

                // INVENTARIO
                var totalProducts = await _context.Producto.Where(p => !p.Cancelado).CountAsync();
                var lowStockProducts = await _context.Saldo
                    .Include(s => s.Producto)
                    .Where(s => !s.Producto.Cancelado && s.Existencia < s.Producto.StockMinimo && s.Producto.StockMinimo > 0)
                    .CountAsync();
                var totalInventoryValue = await _context.Saldo
                    .Include(s => s.Producto)
                    .Where(s => !s.Producto.Cancelado)
                    .Select(s => (decimal?)(s.Existencia * s.Producto.PrecioCosto))
                    .SumAsync() ?? 0m;

                // CLIENTES Y PROVEEDORES
                var totalClients = await _context.Cliente.Where(c => !c.Cancelado).CountAsync();
                var totalSuppliers = await _context.Proveedor.Where(p => !p.Cancelado).CountAsync();

                // FINANZAS
                var cuentasCobrar = await _context.CuentaCobrarPagar.Where(c => c.ClienteId.HasValue && !c.Cancelado).ToListAsync();
                var cuentasPagar = await _context.CuentaCobrarPagar.Where(c => c.ProveedorId.HasValue && !c.Cancelado).ToListAsync();
                
                var totalReceivable = cuentasCobrar.Sum(c => c.Importe);
                var totalPayable = cuentasPagar.Sum(c => c.Importe);
                var overdueReceivable = cuentasCobrar.Where(c => c.Fecha < today).Sum(c => c.Importe);
                var overduePayable = cuentasPagar.Where(c => c.Fecha < today).Sum(c => c.Importe);

                // OPERACIONES
                var ventasHoy = await _context.OperacionesEncabezado
                    .Where(o => o.ClienteId.HasValue && o.Fecha.Date == today && !o.Cancelado)
                    .SelectMany(o => o.OperacionesDetalle)
                    .Select(d => (decimal?)d.ImporteVenta)
                    .SumAsync() ?? 0m;

                var ventasMes = await _context.OperacionesEncabezado
                    .Where(o => o.ClienteId.HasValue && o.Fecha >= firstDayOfMonth && !o.Cancelado)
                    .SelectMany(o => o.OperacionesDetalle)
                    .Select(d => (decimal?)d.ImporteVenta)
                    .SumAsync() ?? 0m;

                var transactionsToday = await _context.OperacionesEncabezado.Where(o => o.Fecha.Date == today && !o.Cancelado).CountAsync();

                return new DashboardStatsDTO
                {
                    TotalAccounts = totalAccounts,
                    AccountsChange = "+0%",
                    AccountsChangeType = "neutral",
                    ActiveUsers = activeUsers,
                    UsersChange = $"+{activeUsers}",
                    UsersChangeType = "positive",
                    AccountGroups = accountGroups,
                    GroupsChange = "+0%",
                    GroupsChangeType = "neutral",
                    PendingTasks = pendingTasks,
                    TasksChange = "+0%",
                    TasksChangeType = "neutral",
                    TotalProducts = totalProducts,
                    ProductsChange = "+0%",
                    ProductsChangeType = "neutral",
                    LowStockProducts = lowStockProducts,
                    TotalInventoryValue = totalInventoryValue,
                    TotalClients = totalClients,
                    ClientsChange = "+0%",
                    ClientsChangeType = "neutral",
                    NewClientsLastMonth = 0,
                    TotalSuppliers = totalSuppliers,
                    SuppliersChange = "+0%",
                    SuppliersChangeType = "neutral",
                    TotalReceivable = totalReceivable,
                    TotalPayable = totalPayable,
                    OverdueReceivable = overdueReceivable,
                    OverduePayable = overduePayable,
                    SalesToday = ventasHoy,
                    SalesThisMonth = ventasMes,
                    SalesMonthChange = "+0%",
                    SalesMonthChangeType = "neutral",
                    TransactionsToday = transactionsToday,
                    AverageTicket = transactionsToday > 0 ? (decimal)ventasHoy / (decimal)transactionsToday : 0m
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "? Error en GetStatsAsync");
                throw;
            }
        }

        public async Task<List<RecentActivityDTO>> GetRecentActivitiesAsync(string language, int count = 15)
        {
            try
            {
                var activities = new List<RecentActivityDTO>();

                // Ventas recientes
                var ventas = await _context.OperacionesEncabezado
                    .Include(o => o.Cliente).ThenInclude(c => c.Translations)
                    .Include(o => o.OperacionesDetalle)
                    .Where(o => o.ClienteId.HasValue && !o.Cancelado)
                    .OrderByDescending(o => o.FechaHoraCreado)
                    .Take(5)
                    .ToListAsync();

                foreach (var v in ventas)
                {
                    activities.Add(new RecentActivityDTO
                    {
                        Id = v.Id,
                        Icon = "??",
                        ActionType = "saleCompleted",
                        EntityType = "venta",
                        TargetName = v.Cliente?.Descripcion ?? "Cliente",
                        CreatedAt = v.FechaHoraCreado,
                        UserName = v.CreadoPor ?? "Sistema",
                        Amount = v.OperacionesDetalle.Sum(d => d.ImporteVenta)
                    });
                }

                // Productos recientes
                var productos = await _context.Producto
                    .Where(p => !p.Cancelado)
                    .OrderByDescending(p => p.FechaHoraCreado)
                    .Take(5)
                    .ToListAsync();

                foreach (var p in productos)
                {
                    activities.Add(new RecentActivityDTO
                    {
                        Id = p.Id,
                        Icon = "??",
                        ActionType = "productCreated",
                        EntityType = "producto",
                        TargetName = p.Descripcion,
                        CreatedAt = p.FechaHoraCreado,
                        UserName = p.CreadoPor ?? "Sistema"
                    });
                }

                return activities.OrderByDescending(a => a.CreatedAt).Take(count).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "? Error en GetRecentActivitiesAsync");
                return new List<RecentActivityDTO>();
            }
        }

        public async Task<DashboardChartDataDTO> GetChartDataAsync(string language)
        {
            return new DashboardChartDataDTO();
        }

        public async Task<DashboardAlertsDTO> GetAlertsAsync(string language)
        {
            try
            {
                var alerts = new List<DashboardAlertDTO>();
                var today = DateTime.UtcNow.Date;

                var lowStock = await _context.Saldo
                    .Include(s => s.Producto)
                    .Where(s => !s.Producto.Cancelado && s.Existencia < s.Producto.StockMinimo && s.Producto.StockMinimo > 0)
                    .CountAsync();

                if (lowStock > 0)
                {
                    alerts.Add(new DashboardAlertDTO
                    {
                        Type = "warning",
                        Icon = "??",
                        MessageKey = "dashboard.alerts.lowStock",
                        Message = $"{lowStock} productos con stock bajo",
                        Count = lowStock,
                        ActionRoute = "/inventario/productos"
                    });
                }

                return new DashboardAlertsDTO
                {
                    Alerts = alerts,
                    TotalAlerts = alerts.Count,
                    CriticalAlerts = 0
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "? Error en GetAlertsAsync");
                return new DashboardAlertsDTO();
            }
        }
    }
}

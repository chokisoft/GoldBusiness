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

                // ✅ SOLUCIÓN: Ejecutar operaciones SECUENCIALMENTE
                _logger.LogInformation("📊 Obteniendo estadísticas del dashboard...");

                // Cuentas
                var totalAccounts = await _context.Cuenta.CountAsync();
                var accountsLastMonth = await _context.Cuenta
                    .Where(c => c.FechaHoraCreado >= lastMonth && c.FechaHoraCreado < today)
                    .CountAsync();

                // Usuarios activos
                var activeUsers = await _userManager.Users.CountAsync();

                // Grupos de cuentas
                var accountGroups = await _context.GrupoCuenta.CountAsync();
                var groupsLastMonth = await _context.GrupoCuenta
                    .Where(g => g.FechaHoraCreado >= lastMonth && g.FechaHoraCreado < today)
                    .CountAsync();

                // Tareas/Transacciones
                var pendingTasks = await _context.Transaccion.CountAsync();
                var tasksLastMonth = await _context.Transaccion
                    .Where(t => t.FechaHoraCreado >= lastMonth && t.FechaHoraCreado < today)
                    .CountAsync();

                _logger.LogInformation($"📊 Stats obtenidos: Cuentas={totalAccounts}, Usuarios={activeUsers}, Grupos={accountGroups}, Tareas={pendingTasks}");

                // Calcular cambios
                var accountsChange = CalculateChange(totalAccounts, accountsLastMonth);
                var groupsChange = CalculateChange(accountGroups, groupsLastMonth);
                var tasksChange = CalculateChange(pendingTasks, tasksLastMonth);

                return new DashboardStatsDTO
                {
                    TotalAccounts = totalAccounts,
                    AccountsChange = accountsChange.Change,
                    AccountsChangeType = accountsChange.Type,

                    ActiveUsers = activeUsers,
                    UsersChange = activeUsers > 0 ? $"+{activeUsers}" : "0",
                    UsersChangeType = activeUsers > 0 ? "positive" : "neutral",

                    AccountGroups = accountGroups,
                    GroupsChange = groupsChange.Change,
                    GroupsChangeType = groupsChange.Type,

                    PendingTasks = pendingTasks,
                    TasksChange = tasksChange.Change,
                    TasksChangeType = tasksChange.Type
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Error en DashboardRepository.GetStatsAsync");
                throw;
            }
        }

        public async Task<List<RecentActivityDTO>> GetRecentActivitiesAsync(string language, int count = 5)
        {
            try
            {
                var activities = await _context.Cuenta
                    .Include(c => c.Translations)
                    .OrderByDescending(c => c.FechaHoraCreado)
                    .Take(count)
                    .Select(c => new
                    {
                        c.Id,
                        c.Codigo,
                        c.FechaHoraCreado,
                        c.CreadoPor,
                        Translation = c.Translations
                            .Where(t => t.Language == language)
                            .Select(t => t.Descripcion)
                            .FirstOrDefault()
                    })
                    .ToListAsync();

                _logger.LogInformation($"📋 Se obtuvieron {activities.Count} actividades recientes");

                return activities.Select(a => new RecentActivityDTO
                {
                    Id = a.Id,
                    Icon = "✅",
                    ActionType = "accountCreated",
                    TargetName = $"{a.Codigo} - {a.Translation ?? a.Codigo}",
                    CreatedAt = a.FechaHoraCreado,
                    UserName = a.CreadoPor ?? "Sistema"
                }).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Error en DashboardRepository.GetRecentActivitiesAsync");
                return new List<RecentActivityDTO>();
            }
        }

        private (string Change, string Type) CalculateChange(int currentValue, int previousValue)
        {
            if (previousValue == 0)
            {
                return currentValue > 0 ? ($"+{currentValue}", "positive") : ("0", "neutral");
            }

            var difference = currentValue - previousValue;
            var percentageChange = Math.Round((double)difference / previousValue * 100, 1);

            if (percentageChange > 0)
            {
                return ($"+{percentageChange}%", "positive");
            }
            else if (percentageChange < 0)
            {
                return ($"{percentageChange}%", "negative");
            }
            else
            {
                return ("0%", "neutral");
            }
        }
    }
}
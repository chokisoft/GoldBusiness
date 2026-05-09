using GoldBusiness.Application.Interfaces;
using GoldBusiness.Domain.DTOs;
using GoldBusiness.Infrastructure.Repositories;
using Microsoft.Extensions.Logging;

namespace GoldBusiness.Application.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly IDashboardRepository _repo;
        private readonly ILogger<DashboardService> _logger;

        public DashboardService(
            IDashboardRepository repo,
            ILogger<DashboardService> logger)
        {
            _repo = repo;
            _logger = logger;
        }

        public async Task<DashboardDataDTO> GetDashboardDataAsync(string language)
        {
            _logger.LogInformation($"📊 Obteniendo datos completos del dashboard para idioma: {language}");

            var dashboard = new DashboardDataDTO();

            try
            {
                dashboard.Stats = await _repo.GetStatsAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Error cargando estadísticas del dashboard. Se devolverán valores por defecto.");
                dashboard.Stats = new DashboardStatsDTO();
            }

            try
            {
                dashboard.RecentActivities = await _repo.GetRecentActivitiesAsync(language);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Error cargando actividades recientes. Se devolverá lista vacía.");
                dashboard.RecentActivities = new List<RecentActivityDTO>();
            }

            try
            {
                dashboard.Charts = await _repo.GetChartDataAsync(language);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Error cargando gráficas. Se devolverán datos vacíos.");
                dashboard.Charts = new DashboardChartDataDTO();
            }

            try
            {
                dashboard.Alerts = await _repo.GetAlertsAsync(language);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Error cargando alertas. Se devolverán datos vacíos.");
                dashboard.Alerts = new DashboardAlertsDTO();
            }

            _logger.LogInformation($"✅ Dashboard completo generado: {dashboard.RecentActivities.Count} actividades, {dashboard.Alerts.TotalAlerts} alertas");

            return dashboard;
        }

        public async Task<DashboardStatsDTO> GetStatsAsync()
        {
            return await _repo.GetStatsAsync();
        }

        public async Task<List<RecentActivityDTO>> GetRecentActivitiesAsync(string language)
        {
            return await _repo.GetRecentActivitiesAsync(language);
        }

        public async Task<DashboardChartDataDTO> GetChartDataAsync(string language)
        {
            return await _repo.GetChartDataAsync(language);
        }

        public async Task<DashboardAlertsDTO> GetAlertsAsync(string language)
        {
            return await _repo.GetAlertsAsync(language);
        }
    }
}
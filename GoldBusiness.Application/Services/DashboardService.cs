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
            _logger.LogInformation($"📊 Obteniendo datos del dashboard para idioma: {language}");

            var dashboard = new DashboardDataDTO
            {
                Stats = await _repo.GetStatsAsync(),
                RecentActivities = await _repo.GetRecentActivitiesAsync(language)
            };

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
    }
}
using GoldBusiness.Domain.DTOs;

namespace GoldBusiness.Infrastructure.Repositories
{
    public interface IDashboardRepository
    {
        Task<DashboardStatsDTO> GetStatsAsync();
        Task<List<RecentActivityDTO>> GetRecentActivitiesAsync(string language, int count = 15);
        Task<DashboardChartDataDTO> GetChartDataAsync(string language);
        Task<DashboardAlertsDTO> GetAlertsAsync(string language);
    }
}
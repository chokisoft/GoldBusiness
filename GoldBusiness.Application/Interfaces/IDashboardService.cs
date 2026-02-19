using GoldBusiness.Domain.DTOs;

namespace GoldBusiness.Application.Interfaces
{
    public interface IDashboardService
    {
        Task<DashboardDataDTO> GetDashboardDataAsync(string language);
        Task<DashboardStatsDTO> GetStatsAsync();
        Task<List<RecentActivityDTO>> GetRecentActivitiesAsync(string language);
    }
}
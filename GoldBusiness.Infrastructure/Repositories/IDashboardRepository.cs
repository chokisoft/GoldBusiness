using GoldBusiness.Domain.DTOs;

namespace GoldBusiness.Infrastructure.Repositories
{
    public interface IDashboardRepository
    {
        Task<DashboardStatsDTO> GetStatsAsync();
        Task<List<RecentActivityDTO>> GetRecentActivitiesAsync(string language, int count = 5);
    }
}
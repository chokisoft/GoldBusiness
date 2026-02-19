namespace GoldBusiness.Domain.DTOs
{
    public class DashboardDataDTO
    {
        public DashboardStatsDTO Stats { get; set; } = new();
        public List<RecentActivityDTO> RecentActivities { get; set; } = new();
    }
}
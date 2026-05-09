namespace GoldBusiness.Domain.DTOs
{
    /// <summary>
    /// DTO principal del dashboard que contiene todas las secciones
    /// </summary>
    public class DashboardDataDTO
    {
        public DashboardStatsDTO Stats { get; set; } = new();
        public List<RecentActivityDTO> RecentActivities { get; set; } = new();
        public DashboardChartDataDTO Charts { get; set; } = new();
        public DashboardAlertsDTO Alerts { get; set; } = new();
    }
}
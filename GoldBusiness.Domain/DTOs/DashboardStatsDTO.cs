namespace GoldBusiness.Domain.DTOs
{
    public class DashboardStatsDTO
    {
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
    }
}
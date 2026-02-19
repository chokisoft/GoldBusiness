namespace GoldBusiness.Domain.DTOs
{
    public class RecentActivityDTO
    {
        public int Id { get; set; }
        public string Icon { get; set; } = string.Empty;
        public string ActionType { get; set; } = string.Empty;
        public string TargetName { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public string UserName { get; set; } = string.Empty;
    }
}
using System.ComponentModel.DataAnnotations;

namespace GoldBusiness.Domain.DTOs
{
    public class UserAdminDTO
    {
        public string Id { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public List<string> Roles { get; set; } = new();
        public List<string> Permissions { get; set; } = new();
        public List<string> AccessLevels { get; set; } = new();
        public string AuthProvider { get; set; } = "Local";
        public bool IsActive { get; set; }
    }

    public class UserAdminUpsertDTO
    {
        [Required]
        [MaxLength(100)]
        public string UserName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [MaxLength(256)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MaxLength(180)]
        public string FullName { get; set; } = string.Empty;

        [MinLength(8)]
        public string? Password { get; set; }

        public List<string> Roles { get; set; } = new();
        public List<string> Permissions { get; set; } = new();
        public List<string> AccessLevels { get; set; } = new();

        [Required]
        [MaxLength(20)]
        public string AuthProvider { get; set; } = "Local";

        public bool IsActive { get; set; } = true;
    }

    public class PagedResultDTO<T>
    {
        public List<T> Items { get; set; } = new();
        public int Total { get; set; }
    }
}

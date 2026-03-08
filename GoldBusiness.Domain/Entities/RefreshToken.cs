using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GoldBusiness.Domain.Entities
{
    /// <summary>
    /// Entidad para almacenar refresh tokens de forma segura
    /// </summary>
    public class RefreshToken
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(450)]
        public string UserId { get; set; } = string.Empty;

        [Required]
        [MaxLength(500)]
        public string Token { get; set; } = string.Empty;

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Required]
        public DateTime ExpiresAt { get; set; }

        public DateTime? RevokedAt { get; set; }

        [MaxLength(100)]
        public string? RevokedByIp { get; set; }

        [MaxLength(100)]
        public string? ReplacedByToken { get; set; }

        [MaxLength(200)]
        public string? CreatedByIp { get; set; }

        [MaxLength(500)]
        public string? UserAgent { get; set; }

        // Navegación
        [ForeignKey(nameof(UserId))]
        public virtual ApplicationUser? User { get; set; }

        // Propiedades calculadas
        [NotMapped]
        public bool IsExpired => DateTime.UtcNow >= ExpiresAt;

        [NotMapped]
        public bool IsRevoked => RevokedAt != null;

        [NotMapped]
        public bool IsActive => !IsRevoked && !IsExpired;
    }
}
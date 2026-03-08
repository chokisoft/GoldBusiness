using System.ComponentModel.DataAnnotations;

namespace GoldBusiness.Domain.DTOs
{
    public class RefreshTokenRequest
    {
        [Required(ErrorMessage = "El refresh token es requerido")]
        public string RefreshToken { get; set; } = string.Empty;
    }
}
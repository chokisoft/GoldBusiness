using Microsoft.AspNetCore.Identity;

namespace GoldBusiness.Domain.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public bool IsActive { get; set; } = true; // 👈 valor por defecto
    }
}
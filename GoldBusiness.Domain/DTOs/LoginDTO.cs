using System.ComponentModel.DataAnnotations;
using GoldBusiness.Domain.Resources;

namespace GoldBusiness.Domain.DTOs
{
    public class LoginDTO
    {
        [Required(
            ErrorMessageResourceType = typeof(ValidationMessages),
            ErrorMessageResourceName = nameof(ValidationMessages.Required)
        )]
        [Display(
            Name = nameof(ValidationMessages.Field_Username),
            ResourceType = typeof(ValidationMessages)
        )]
        [StringLength(256,
            ErrorMessageResourceType = typeof(ValidationMessages),
            ErrorMessageResourceName = nameof(ValidationMessages.StringLengthMax)
        )]
        public string Username { get; set; } = string.Empty;

        [Required(
            ErrorMessageResourceType = typeof(ValidationMessages),
            ErrorMessageResourceName = nameof(ValidationMessages.RequiredFemale)
        )]
        [Display(
            Name = nameof(ValidationMessages.Field_Password),
            ResourceType = typeof(ValidationMessages)
        )]
        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 6,
            ErrorMessageResourceType = typeof(ValidationMessages),
            ErrorMessageResourceName = nameof(ValidationMessages.StringLength)
        )]
        public string Password { get; set; } = string.Empty;
    }
}

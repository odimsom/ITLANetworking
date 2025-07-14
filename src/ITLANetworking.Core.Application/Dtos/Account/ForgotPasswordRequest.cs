using System.ComponentModel.DataAnnotations;

namespace ITLANetworking.Core.Application.Dtos.Account
{
    public class ForgotPasswordRequest
    {
        public string UserName { get; set; } = string.Empty;
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        public string? Origin { get; set; }
    }
}

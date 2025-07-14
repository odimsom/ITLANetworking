using System.Text.Json.Serialization;

namespace ITLANetworking.Core.Application.Dtos.Account
{
    public class AuthenticationResponse
    {
        public string Id { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public string? ProfilePicture { get; set; }
        public List<string> Roles { get; set; } = new();
        public bool IsVerified { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }

        public string JWToken { get; set; } = string.Empty;

        [JsonIgnore]
        public string RefreshToken { get; set; } = string.Empty;

        public bool HasError { get; set; }
        public string? Error { get; set; }
    }
}

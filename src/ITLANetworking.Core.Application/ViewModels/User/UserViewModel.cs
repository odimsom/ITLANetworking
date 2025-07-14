namespace ITLANetworking.Core.Application.ViewModels.User
{
    public class UserViewModel
    {
        public string Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public string? ProfilePicture { get; set; }
        public string FullName => $"{FirstName} {LastName}";
        public bool IsFriend { get; set; }
        public bool HasPendingRequest { get; set; }
    }
}

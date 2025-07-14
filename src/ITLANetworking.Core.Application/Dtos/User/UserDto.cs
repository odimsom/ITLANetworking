namespace ITLANetworking.Core.Application.Dtos.User
{
    public class UserDto
    {
        public string Id { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public string? ProfilePicture { get; set; }
        public bool IsEmailConfirmed { get; set; }
        public bool IsActive { get; set; } = false; 
        public DateTime Created { get; set; }
        public string FullName => $"{FirstName} {LastName}";
    }
}

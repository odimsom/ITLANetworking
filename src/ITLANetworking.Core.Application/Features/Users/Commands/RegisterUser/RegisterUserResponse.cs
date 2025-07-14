namespace ITLANetworking.Core.Application.Features.Users.Commands.RegisterUser
{
    public class RegisterUserResponse
    {
        public string UserId { get; set; } = string.Empty;
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}

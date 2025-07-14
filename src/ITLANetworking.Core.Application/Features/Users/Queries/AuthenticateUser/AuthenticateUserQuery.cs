using ITLANetworking.Core.Application.Dtos.Account;
using MediatR;

namespace ITLANetworking.Core.Application.Features.Users.Queries.AuthenticateUser
{
    public class AuthenticateUserQuery : IRequest<AuthenticationResponse>
    {
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}

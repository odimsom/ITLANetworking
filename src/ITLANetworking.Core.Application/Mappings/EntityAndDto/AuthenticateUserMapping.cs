using AutoMapper;
using ITLANetworking.Core.Application.Dtos.Account;
using ITLANetworking.Core.Application.Features.Users.Queries.AuthenticateUser;

namespace ITLANetworking.Core.Application.Mappings.EntityAndDto
{
    public class AuthenticateUserMapping : Profile
    {
        public AuthenticateUserMapping()
        {
            CreateMap<AuthenticateUserQuery, AuthenticationRequest>()
                .ReverseMap();
        }
    }
}

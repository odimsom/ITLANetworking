using AutoMapper;
using ITLANetworking.Core.Application.Dtos.Account;
using ITLANetworking.Core.Application.Features.Users.Commands.RegisterUser;

namespace ITLANetworking.Core.Application.Mappings.EntityAndDto
{
    public class RegisterUserMapping : Profile
    {
        public RegisterUserMapping()
        {
            CreateMap<RegisterUserCommand, RegisterRequest>()
                .ReverseMap();
        }
    }
}

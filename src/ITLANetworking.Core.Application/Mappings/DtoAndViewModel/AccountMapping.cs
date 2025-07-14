using AutoMapper;
using ITLANetworking.Core.Application.Dtos.Account;
using ITLANetworking.Core.Application.ViewModels.User;

namespace ITLANetworking.Core.Application.Mappings.DtoAndViewModel
{
    public class AccountMapping : Profile
    {
        public AccountMapping()
        {
            CreateMap<LoginViewModel, AuthenticationRequest>()
                .ReverseMap();

            CreateMap<SaveUserViewModel, RegisterRequest>()
                .ReverseMap();

            CreateMap<ForgotPasswordViewModel, ForgotPasswordRequest>()
                .ReverseMap();

            CreateMap<ResetPasswordViewModel, ResetPasswordRequest>()
                .ReverseMap();
        }
    }
}

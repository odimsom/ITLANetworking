using AutoMapper;
using ITLANetworking.Core.Application.Dtos.Account;
using ITLANetworking.Core.Application.Dtos.Post;
using ITLANetworking.Core.Application.Dtos.User;
using ITLANetworking.Core.Application.ViewModels.Post;
using ITLANetworking.Core.Application.ViewModels.User;
using ITLANetworking.Core.Domain.Entities;
using ITLANetworking.Infrastructure.Identity.Entities;

namespace ITLANetworking.Core.Application.Mappings
{
    public class GeneralProfile : Profile
    {
        public GeneralProfile()
        {
            #region User Mappings
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<ApplicationUser, UserDto>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
                .ReverseMap();
            #endregion

            #region Account Mappings
            CreateMap<AuthenticationRequest, LoginViewModel>().ReverseMap();
            CreateMap<RegisterRequest, SaveUserViewModel>().ReverseMap();
            CreateMap<RegisterRequest, ApplicationUser>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName));
            #endregion

            #region Post Mappings
            CreateMap<Post, PostDto>().ReverseMap();
            CreateMap<SavePostDto, Post>().ReverseMap();
            CreateMap<SavePostViewModel, SavePostDto>()
                .ForMember(dest => dest.ImageUrl, opt => opt.Ignore())
                .ForMember(dest => dest.VideoUrl, opt => opt.MapFrom(src => src.VideoUrl));
            CreateMap<PostDto, PostViewModel>().ReverseMap();
            #endregion
        }
    }
}

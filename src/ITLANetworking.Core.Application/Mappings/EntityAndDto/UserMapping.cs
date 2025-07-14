using AutoMapper;
using ITLANetworking.Core.Application.Dtos.User;
using ITLANetworking.Core.Application.ViewModels.User;
using ITLANetworking.Core.Domain.Entities;

namespace ITLANetworking.Core.Application.Mappings.EntityAndDto
{
    public class UserMapping : Profile
    {
        public UserMapping()
        {
            CreateMap<User, UserDto>()
                .ReverseMap();
            CreateMap<UserDto, UserViewModel>()
              .ForMember(dest => dest.IsFriend, opt => opt.Ignore())
              .ForMember(dest => dest.HasPendingRequest, opt => opt.Ignore())
             .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id));

            CreateMap<UserViewModel, UserDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()))
                .ForMember(dest => dest.IsEmailConfirmed, opt => opt.Ignore())
                .ForMember(dest => dest.IsActive, opt => opt.Ignore())
                .ForMember(dest => dest.Created, opt => opt.Ignore());
        }
    }
}

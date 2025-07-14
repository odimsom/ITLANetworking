using AutoMapper;
using ITLANetworking.Core.Application.Dtos.Friendship;
using ITLANetworking.Core.Domain.Entities;

namespace ITLANetworking.Core.Application.Mappings.EntityAndDto
{
    public class FriendshipMapping : Profile
    {
        public FriendshipMapping()
        {
            // Map Friendship entity to FriendshipDto
            CreateMap<Friendship, FriendshipDto>()
                .ForMember(dest => dest.AddresseeId, opt => opt.MapFrom(src => src.ReceiverId))
                .ForMember(dest => dest.Addressee, opt => opt.MapFrom(src => src.Receiver))
                .ForMember(dest => dest.Created, opt => opt.MapFrom(src => src.RequestDate))
                .ForMember(dest => dest.MutualFriendsCount, opt => opt.Ignore());

            // Map FriendshipDto to Friendship entity
            CreateMap<FriendshipDto, Friendship>()
                .ForMember(dest => dest.ReceiverId, opt => opt.MapFrom(src => src.AddresseeId))
                .ForMember(dest => dest.Receiver, opt => opt.MapFrom(src => src.Addressee))
                .ForMember(dest => dest.RequestDate, opt => opt.MapFrom(src => src.Created))
                .ForMember(dest => dest.ResponseDate, opt => opt.Ignore());

            // Map Friendship entity to FriendshipRequestDto
            CreateMap<Friendship, FriendshipRequestDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.AddresseeId, opt => opt.MapFrom(src => src.ReceiverId))
                .ForMember(dest => dest.RequesterId, opt => opt.MapFrom(src => src.RequesterId))
                .ForMember(dest => dest.RequesterName, opt => opt.MapFrom(src => $"{src.Requester.FirstName} {src.Requester.LastName}"))
                .ForMember(dest => dest.RequesterUsername, opt => opt.MapFrom(src => src.Requester.FirstName))
                .ForMember(dest => dest.RequesterProfilePicture, opt => opt.MapFrom(src => src.Requester.ProfilePicture))
                .ForMember(dest => dest.ReceiverName, opt => opt.MapFrom(src => src.Receiver != null ? $"{src.Receiver.FirstName} {src.Receiver.LastName}" : string.Empty))
                .ForMember(dest => dest.ReceiverUsername, opt => opt.MapFrom(src => src.Receiver != null ? src.Receiver.FirstName + " " + src.Receiver.LastName : string.Empty))
                .ForMember(dest => dest.ReceiverProfilePicture, opt => opt.MapFrom(src => src.Receiver != null ? src.Receiver.ProfilePicture : null))
                .ForMember(dest => dest.RequestDate, opt => opt.MapFrom(src => src.RequestDate))
                .ForMember(dest => dest.MutualFriendsCount, opt => opt.Ignore());


            CreateMap<FriendshipRequestDto, Friendship>()
                .ForMember(dest => dest.RequesterId, opt => opt.MapFrom(src => src.RequesterId))
                .ForMember(dest => dest.ReceiverId, opt => opt.MapFrom(src => src.AddresseeId))
                .ForMember(dest => dest.RequestDate, opt => opt.MapFrom(src => src.RequestDate))
                .ForMember(dest => dest.ResponseDate, opt => opt.Ignore())
                .ForMember(dest => dest.Requester, opt => opt.Ignore())
                .ForMember(dest => dest.Receiver, opt => opt.Ignore())
                .ForMember(dest => dest.Status, opt => opt.Ignore());
        }
    }
}

using ITLANetworking.Core.Application.Dtos.User;
using MediatR;

namespace ITLANetworking.Core.Application.Features.Friends.Queries.GetFriends
{
    public class GetFriendsQuery : IRequest<List<UserDto>>
    {
        public string UserId { get; set; } = string.Empty;
    }
}

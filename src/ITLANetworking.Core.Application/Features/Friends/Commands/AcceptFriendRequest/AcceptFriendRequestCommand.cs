using MediatR;

namespace ITLANetworking.Core.Application.Features.Friends.Commands.AcceptFriendRequest
{
    public class AcceptFriendRequestCommand : IRequest<AcceptFriendRequestResponse>
    {
        public int FriendshipId { get; set; }
    }
}

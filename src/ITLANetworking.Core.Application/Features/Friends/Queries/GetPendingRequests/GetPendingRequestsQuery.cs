using ITLANetworking.Core.Application.Dtos.Friendship;
using MediatR;

namespace ITLANetworking.Core.Application.Features.Friends.Queries.GetPendingRequests
{
    public class GetPendingRequestsQuery : IRequest<List<FriendshipRequestDto>>
    {
        public string UserId { get; set; } = string.Empty;
    }
}

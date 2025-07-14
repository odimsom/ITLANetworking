using ITLANetworking.Core.Application.Dtos.Friendship;
using ITLANetworking.Core.Application.Interfaces.Services;
using MediatR;

namespace ITLANetworking.Core.Application.Features.Friends.Queries.GetPendingRequests
{
    public class GetPendingRequestsHandler : IRequestHandler<GetPendingRequestsQuery, List<FriendshipRequestDto>>
    {
        private readonly IFriendshipService _friendshipService;

        public GetPendingRequestsHandler(IFriendshipService friendshipService)
        {
            _friendshipService = friendshipService;
        }

        public async Task<List<FriendshipRequestDto>> Handle(GetPendingRequestsQuery request, CancellationToken cancellationToken)
        {
            var friendshipRequests = await _friendshipService.GetPendingRequestsByUserIdAsync(request.UserId);

            return friendshipRequests.Select(fr => new FriendshipRequestDto
            {
                Id = fr.Id,
                RequesterId = fr.Requester.Id.ToString(),
                AddresseeId = fr.Receiver.Id.ToString(),
                RequesterName = fr.Requester.FullName,
                RequesterUsername = fr.Requester.UserName,
                RequesterProfilePicture = fr.Requester.ProfilePicture,
                RequestDate = fr.Created
            }).ToList();
        }
    }
}
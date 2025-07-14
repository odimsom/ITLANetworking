using ITLANetworking.Core.Application.Dtos.User;
using ITLANetworking.Core.Application.Interfaces.Services;
using MediatR;

namespace ITLANetworking.Core.Application.Features.Friends.Queries.GetFriends
{
    public class GetFriendsHandler : IRequestHandler<GetFriendsQuery, List<UserDto>>
    {
        private readonly IFriendshipService _friendshipService;

        public GetFriendsHandler(IFriendshipService friendshipService)
        {
            _friendshipService = friendshipService;
        }

        public async Task<List<UserDto>> Handle(GetFriendsQuery request, CancellationToken cancellationToken)
        {
            var friendships = await _friendshipService.GetFriendsByUserIdAsync(request.UserId);
            var friends = new List<UserDto>();

            foreach (var friendship in friendships)
            {
                bool isRequester = friendship.Requester.Id.ToString() == request.UserId;

                var friend = isRequester ? friendship.Receiver : friendship.Requester;

                friends.Add(new UserDto
                {
                    Id = friend.Id.ToString(),
                    FirstName = friend.FirstName,
                    LastName = friend.LastName,
                    UserName = friend.UserName,
                    Email = friend.Email,
                    Phone = friend.Phone,
                    ProfilePicture = friend.ProfilePicture,
                    IsActive = true,
                    Created = friendship.Created
                });
            }
            return friends;
        }
    }
}

using ITLANetworking.Core.Application.Dtos.Friendship;
using ITLANetworking.Core.Application.Dtos.User;
using ITLANetworking.Core.Application.Interfaces.Services.Common;
using ITLANetworking.Core.Domain.Entities;

namespace ITLANetworking.Core.Application.Interfaces.Services
{
    public interface IFriendshipService : IGenericService<FriendshipDto, FriendshipDto, Friendship>
    {
        Task<List<FriendshipDto>> GetFriendsByUserIdAsync(string userId);
        Task<List<FriendshipDto>> GetPendingRequestsByUserIdAsync(string userId);
        Task<List<FriendshipDto>> GetSentRequestsByUserIdAsync(string userId);
        Task<List<FriendshipRequestDto>> GetPendingRequests(string userId);
        Task<List<FriendshipRequestDto>> GetSentRequests(string userId);
        Task<List<UserDto>> GetAvailableUsers(string userId, string? searchTerm = null);
        Task SendFriendRequestAsync(string requesterId, string addresseeId);
        Task AcceptFriendRequestAsync(int friendshipId);
        Task RejectFriendRequest(int friendshipId);
        Task RemoveFriend(string userId1, string userId2);
        Task<bool> AreFriendsAsync(string userId1, string userId2);
        Task<bool> HasPendingRequestAsync(string requesterId, string addresseeId);
    }
}

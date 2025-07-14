using ITLANetworking.Core.Domain.Entities;
using ITLANetworking.Core.Domain.Enums;

namespace ITLANetworking.Core.Domain.Interfaces.Repositories
{
    public interface IFriendshipRepository : IGenericRepository<Friendship>
    {
        Task<List<Friendship>> GetPendingRequestsAsync(string userId);
        Task<List<Friendship>> GetSentRequestsAsync(string userId);
        Task<List<User>> GetFriendsAsync(string userId);
        Task<bool> AreFriendsAsync(string userId1, string userId2);
        Task<bool> HasPendingRequestAsync(string requesterId, string receiverId);
        Task<int> GetMutualFriendsCountAsync(string userId1, string userId2);
    }
}

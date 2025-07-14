using ITLANetworking.Core.Domain.Entities;
using ITLANetworking.Core.Domain.Enums;
using ITLANetworking.Core.Domain.Interfaces.Repositories;
using ITLANetworking.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace ITLANetworking.Infrastructure.Persistence.Repositories
{
    public class FriendshipRepository : GenericRepository<Friendship>, IFriendshipRepository
    {
        private readonly ApplicationContext _context;

        public FriendshipRepository(ApplicationContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<Friendship>> GetPendingRequestsAsync(string userId)
        {
            return await _context.Friendships
                .Include(f => f.Requester)
                .Include(f => f.Receiver)
                .Where(f => f.ReceiverId == userId && f.Status == FriendshipStatus.Pending)
                .OrderByDescending(f => f.RequestDate)
                .ToListAsync();
        }

        public async Task<List<Friendship>> GetSentRequestsAsync(string userId)
        {
            return await _context.Friendships
                .Include(f => f.Requester)
                .Include(f => f.Receiver)
                .Where(f => f.RequesterId == userId && f.Status == FriendshipStatus.Pending)
                .OrderByDescending(f => f.RequestDate)
                .ToListAsync();
        }

        public async Task<List<User>> GetFriendsAsync(string userId)
        {
            var friendships = await _context.Friendships
                .Include(f => f.Requester)
                .Include(f => f.Receiver)
                .Where(f => (f.RequesterId == userId || f.ReceiverId == userId) &&
                           f.Status == FriendshipStatus.Accepted)
                .ToListAsync();

            return friendships
                .Select(f => f.RequesterId == userId ? f.Receiver : f.Requester)
                .ToList();
        }

        public async Task<bool> AreFriendsAsync(string userId1, string userId2)
        {
            return await _context.Friendships
                .AnyAsync(f =>
                    ((f.RequesterId == userId1 && f.ReceiverId == userId2) ||
                     (f.RequesterId == userId2 && f.ReceiverId == userId1)) &&
                    f.Status == FriendshipStatus.Accepted);
        }

        public async Task<bool> HasPendingRequestAsync(string requesterId, string receiverId)
        {
            return await _context.Friendships
                .AnyAsync(f =>
                    f.RequesterId == requesterId &&
                    f.ReceiverId == receiverId &&
                    f.Status == FriendshipStatus.Pending);
        }

        public async Task<int> GetMutualFriendsCountAsync(string userId1, string userId2)
        {
            var user1FriendIds = await GetFriendIdsAsync(userId1);
            var user2FriendIds = await GetFriendIdsAsync(userId2);

            return user1FriendIds.Intersect(user2FriendIds).Count();
        }

        private async Task<List<string>> GetFriendIdsAsync(string userId)
        {
            var friendships = await _context.Friendships
                .Where(f => (f.RequesterId == userId || f.ReceiverId == userId) &&
                           f.Status == FriendshipStatus.Accepted)
                .ToListAsync();

            return friendships.Select(f => f.RequesterId == userId ? f.ReceiverId : f.RequesterId).ToList();
        }
    }
}

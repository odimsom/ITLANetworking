using ITLANetworking.Core.Domain.Entities;
using ITLANetworking.Core.Domain.Interfaces.Repositories;
using ITLANetworking.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace ITLANetworking.Infrastructure.Persistence.Repositories
{
    public class PostRepository : GenericRepository<Post>, IPostRepository
    {
        private readonly ApplicationContext _context;

        public PostRepository(ApplicationContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<Post>> GetUserPostsAsync(string userId)
        {
            return await _context.Posts
                .Include(p => p.User)
                .Include(p => p.Comments)
                    .ThenInclude(c => c.User)
                .Include(p => p.Reactions)
                    .ThenInclude(r => r.User)
                .Where(p => p.UserId == userId)
                .OrderByDescending(p => p.Created)
                .ToListAsync();
        }

        public async Task<List<Post>> GetFriendsPostsAsync(string userId)
        {
            var friendshipQuery = _context.Friendships
                .Where(f => (f.RequesterId == userId || f.ReceiverId == userId) &&
                           f.Status == Core.Domain.Enums.FriendshipStatus.Accepted)
                .Select(f => f.RequesterId == userId ? f.ReceiverId : f.RequesterId);

            var friendIds = await friendshipQuery.ToListAsync();

            return await _context.Posts
                .Include(p => p.User)
                .Include(p => p.Comments)
                    .ThenInclude(c => c.User)
                .Include(p => p.Reactions)
                    .ThenInclude(r => r.User)
                .Where(p => friendIds.Contains(p.UserId))
                .OrderByDescending(p => p.Created)
                .ToListAsync();
        }

        public async Task<List<Post>> GetPostsWithCommentsAsync(string userId)
        {
            var postsWithUserComments = await _context.Comments
                .Where(c => c.UserId == userId)
                .Select(c => c.PostId)
                .Distinct()
                .ToListAsync();

            return await _context.Posts
                .Include(p => p.User)
                .Include(p => p.Comments)
                    .ThenInclude(c => c.User)
                .Include(p => p.Reactions)
                .Where(p => postsWithUserComments.Contains(p.Id))
                .OrderByDescending(p => p.Created)
                .ToListAsync();
        }

        public async Task<List<Post>> GetFeedPostsAsync(List<string> friendIds, int page = 1, int pageSize = 10)
        {
            return await _context.Posts
                .Include(p => p.User)
                .Include(p => p.Comments)
                    .ThenInclude(c => c.User)
                .Include(p => p.Reactions)
                    .ThenInclude(r => r.User)
                .Where(p => friendIds.Contains(p.UserId))
                .OrderByDescending(p => p.Created)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<Post?> GetPostWithDetailsAsync(int postId)
        {
            return await _context.Posts
                .Include(p => p.User)
                .Include(p => p.Comments)
                    .ThenInclude(c => c.User)
                .Include(p => p.Comments)
                    .ThenInclude(c => c.Replies)
                        .ThenInclude(r => r.User)
                .Include(p => p.Reactions)
                    .ThenInclude(r => r.User)
                .FirstOrDefaultAsync(p => p.Id == postId);
        }

        public async Task<int> GetPostCountByUserAsync(string userId)
        {
            return await _context.Posts.CountAsync(p => p.UserId == userId);
        }

        public async Task<List<Post>> GetRecentPostsAsync(int count = 20)
        {
            return await _context.Posts
                .Include(p => p.User)
                .Include(p => p.Comments)
                    .ThenInclude(c => c.User)
                .Include(p => p.Reactions)
                    .ThenInclude(r => r.User)
                .OrderByDescending(p => p.Created)
                .Take(count)
                .ToListAsync();
        }

        public async Task<List<Post>> SearchPostsAsync(string searchText, int maxResults = 50)
        {
            if (string.IsNullOrWhiteSpace(searchText))
                return new List<Post>();

            searchText = searchText.ToLower();

            return await _context.Posts
                .Include(p => p.User)
                .Include(p => p.Comments)
                    .ThenInclude(c => c.User)
                .Include(p => p.Reactions)
                .Where(p => p.Content.ToLower().Contains(searchText))
                .OrderByDescending(p => p.Created)
                .Take(maxResults)
                .ToListAsync();
        }

        public async Task<List<Post>> GetTrendingPostsAsync(int hoursWindow = 24, int maxResults = 20)
        {
            var cutoffDate = DateTime.UtcNow.AddHours(-hoursWindow);

            var posts = await _context.Posts
                .Include(p => p.User)
                .Include(p => p.Comments.Where(c => c.Created >= cutoffDate))
                    .ThenInclude(c => c.User)
                .Include(p => p.Reactions)
                    .ThenInclude(r => r.User)
                .Where(p => p.Created >= cutoffDate ||
                           p.Comments.Any(c => c.Created >= cutoffDate) ||
                           p.Reactions.Any(r => r.Created >= cutoffDate))
                .ToListAsync();

            return posts
                .OrderByDescending(p =>
                    p.Comments.Count(c => c.Created >= cutoffDate) * 2 +
                    p.Reactions.Count(r => r.Created >= cutoffDate))
                .Take(maxResults)
                .ToList();
        }
    }
}

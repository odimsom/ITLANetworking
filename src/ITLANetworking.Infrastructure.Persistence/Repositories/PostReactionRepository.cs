using ITLANetworking.Core.Domain.Entities;
using ITLANetworking.Core.Domain.Enums;
using ITLANetworking.Core.Domain.Interfaces.Repositories;
using ITLANetworking.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace ITLANetworking.Infrastructure.Persistence.Repositories
{
    public class PostReactionRepository : GenericRepository<PostReaction>, IPostReactionRepository
    {
        private readonly ApplicationContext _context;

        public PostReactionRepository(ApplicationContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<PostReaction>> GetReactionsByPostIdAsync(int postId)
        {
            return await _context.PostReactions
                .Include(pr => pr.User)
                .Where(pr => pr.PostId == postId)
                .ToListAsync();
        }

        public async Task<PostReaction?> GetUserReactionAsync(int postId, string userId)
        {
            return await _context.PostReactions
                .FirstOrDefaultAsync(pr => pr.PostId == postId && pr.UserId == userId);
        }

        public async Task<int> GetReactionCountAsync(int postId, ReactionType reactionType)
        {
            return await _context.PostReactions
                .CountAsync(pr => pr.PostId == postId && pr.Type == reactionType);
        }

        // Additional helper methods not in the interface but used elsewhere in the codebase

        public async Task<PostReaction?> GetReactionAsync(string userId, int postId)
        {
            return await _context.PostReactions
                .FirstOrDefaultAsync(pr => pr.UserId == userId && pr.PostId == postId);
        }

        public async Task<int> GetReactionCountByPostAsync(int postId)
        {
            return await _context.PostReactions.CountAsync(pr => pr.PostId == postId);
        }

        public async Task RemoveReactionAsync(string userId, int postId)
        {
            var reaction = await GetReactionAsync(userId, postId);
            if (reaction != null)
            {
                _context.PostReactions.Remove(reaction);
                await _context.SaveChangesAsync();
            }
        }
    }
}

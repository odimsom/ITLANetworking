using ITLANetworking.Core.Domain.Entities;
using ITLANetworking.Core.Domain.Enums;

namespace ITLANetworking.Core.Domain.Interfaces.Repositories
{
    public interface IPostReactionRepository : IGenericRepository<PostReaction>
    {
        Task<List<PostReaction>> GetReactionsByPostIdAsync(int postId);
        Task<PostReaction?> GetUserReactionAsync(int postId, string userId);
        Task<int> GetReactionCountAsync(int postId, Core.Domain.Enums.ReactionType reactionType);
    }
}

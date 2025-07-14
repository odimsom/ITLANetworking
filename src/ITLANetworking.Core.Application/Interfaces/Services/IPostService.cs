using ITLANetworking.Core.Application.Dtos.Post;
using ITLANetworking.Core.Application.Interfaces.Services.Common;
using ITLANetworking.Core.Domain.Entities;
using ITLANetworking.Core.Domain.Enums;

namespace ITLANetworking.Core.Application.Interfaces.Services
{
    public interface IPostService : IGenericService<SavePostDto, PostDto, Post>
    {
        Task<List<PostDto>> GetAllWithUserAsync();
        Task<List<PostDto>> GetAllPostsWithDetailsAsync();
        Task<List<PostDto>> GetByUserIdAsync(string userId);
        Task<List<PostDto>> GetFriendsPostsAsync(string userId);
        Task<List<PostDto>> GetFeedPostsAsync(List<string> friendIds, int page = 1, int pageSize = 10);
        Task<PostDto?> GetByIdWithDetailsAsync(int id);
        Task<int> GetPostCountByUserAsync(string userId);
        Task ReactToPostAsync(int postId, string userId, ReactionType reactionType);
        Task RemoveReactionAsync(int postId, string userId);
        Task<List<PostDto>> GetRecentPostsAsync(int count = 20);
        Task<List<PostDto>> SearchPostsAsync(string searchText, int maxResults = 50);
        Task<List<PostDto>> GetTrendingPostsAsync(int hoursWindow = 24, int maxResults = 20);
        Task<Dictionary<ReactionType, int>> GetPostReactionStatsAsync(int postId);
    }
}

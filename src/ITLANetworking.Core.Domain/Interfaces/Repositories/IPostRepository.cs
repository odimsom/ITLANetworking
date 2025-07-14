using ITLANetworking.Core.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ITLANetworking.Core.Domain.Interfaces.Repositories
{
    public interface IPostRepository : IGenericRepository<Post>
    {
        Task<List<Post>> GetUserPostsAsync(string userId);
        Task<List<Post>> GetFriendsPostsAsync(string userId);
        Task<List<Post>> GetPostsWithCommentsAsync(string userId);
        Task<List<Post>> GetFeedPostsAsync(List<string> friendIds, int page = 1, int pageSize = 10);
        Task<Post?> GetPostWithDetailsAsync(int postId);
        Task<int> GetPostCountByUserAsync(string userId);
        Task<List<Post>> GetRecentPostsAsync(int count = 20);
        Task<List<Post>> SearchPostsAsync(string searchText, int maxResults = 50);
        Task<List<Post>> GetTrendingPostsAsync(int hoursWindow = 24, int maxResults = 20);
    }
}

using ITLANetworking.Core.Domain.Entities;

namespace ITLANetworking.Core.Domain.Interfaces.Repositories
{
    public interface ICommentRepository : IGenericRepository<Comment>
    {
        Task<List<Comment>> GetCommentsByPostIdAsync(int postId);
        Task<List<Comment>> GetRepliesByCommentIdAsync(int commentId);
    }
}

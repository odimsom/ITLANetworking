using ITLANetworking.Core.Domain.Entities;
using ITLANetworking.Core.Domain.Interfaces.Repositories;
using ITLANetworking.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace ITLANetworking.Infrastructure.Persistence.Repositories
{
    public class CommentRepository : GenericRepository<Comment>, ICommentRepository
    {
        private readonly ApplicationContext _context;

        public CommentRepository(ApplicationContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<Comment>> GetCommentsByPostIdAsync(int postId)
        {
            return await _context.Comments
                .Include(c => c.User)
                .Include(c => c.Replies)
                    .ThenInclude(r => r.User)
                .Where(c => c.PostId == postId && c.ParentCommentId == null)
                .OrderBy(c => c.Created)
                .ToListAsync();
        }

        public async Task<List<Comment>> GetRepliesByCommentIdAsync(int commentId)
        {
            return await _context.Comments
                .Include(c => c.User)
                .Where(c => c.ParentCommentId == commentId)
                .OrderBy(c => c.Created)
                .ToListAsync();
        }

        public async Task<Comment?> GetCommentWithDetailsAsync(int commentId)
        {
            return await _context.Comments
                .Include(c => c.User)
                .Include(c => c.Post)
                .Include(c => c.ParentComment)
                .Include(c => c.Replies)
                    .ThenInclude(r => r.User)
                .FirstOrDefaultAsync(c => c.Id == commentId);
        }

        public async Task<int> GetCommentCountByPostAsync(int postId)
        {
            return await _context.Comments.CountAsync(c => c.PostId == postId);
        }
    }
}

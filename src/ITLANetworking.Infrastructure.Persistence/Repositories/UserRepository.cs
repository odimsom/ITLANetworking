using ITLANetworking.Core.Domain.Entities;
using ITLANetworking.Core.Domain.Interfaces.Repositories;
using ITLANetworking.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace ITLANetworking.Infrastructure.Persistence.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        private readonly ApplicationContext _dbContext;

        public UserRepository(ApplicationContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public override async Task<User?> GetByIdAsync(int id)
        {
            return await _dbContext.Users
                .FirstOrDefaultAsync(u => u.Id == id.ToString());
        }

        public async Task<User?> GetByIdAsync(string id)
        {
            return await _dbContext.Users
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task UpdateAsync(User entity, string id)
        {
            var existingUser = await GetByIdAsync(id);
            if (existingUser != null)
            {
                _dbContext.Entry(existingUser).CurrentValues.SetValues(entity);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<User?> GetByUsernameAsync(string username)
        {
            return await _dbContext.Users
                .FirstOrDefaultAsync(u => u.FirstName.ToLower() + "." + u.LastName.ToLower() == username.ToLower());
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _dbContext.Users
                .FirstOrDefaultAsync(u => u.Id == email);
        }

        public async Task<List<User>> SearchUsersByUsernameAsync(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return new List<User>();

            searchTerm = searchTerm.ToLower();

            return await _dbContext.Users
                .Where(u => (u.FirstName + " " + u.LastName).ToLower().Contains(searchTerm) ||
                           (u.FirstName + "." + u.LastName).ToLower().Contains(searchTerm))
                .Where(u => u.IsActive)
                .ToListAsync();
        }

        public async Task<List<User>> GetActiveUsersAsync()
        {
            return await _dbContext.Users
                .Where(u => u.IsActive)
                .ToListAsync();
        }

        public async Task<User?> GetUserWithPostsAsync(string userId)
        {
            return await _dbContext.Users
                .Include(u => u.Posts)
                .FirstOrDefaultAsync(u => u.Id == userId);
        }

        public async Task<User?> GetUserWithCommentsAsync(string userId)
        {
            return await _dbContext.Users
                .Include(u => u.Comments)
                .FirstOrDefaultAsync(u => u.Id == userId);
        }

        public async Task<User?> GetUserWithFriendshipsAsync(string userId)
        {
            return await _dbContext.Users
                .Include(u => u.SentFriendRequests)
                    .ThenInclude(f => f.Receiver)
                .Include(u => u.ReceivedFriendRequests)
                    .ThenInclude(f => f.Requester)
                .FirstOrDefaultAsync(u => u.Id == userId);
        }

        public async Task<User?> GetUserWithAllDetailsAsync(string userId)
        {
            return await _dbContext.Users
                .Include(u => u.Posts)
                .Include(u => u.Comments)
                .Include(u => u.PostReactions)
                .Include(u => u.SentFriendRequests)
                    .ThenInclude(f => f.Receiver)
                .Include(u => u.ReceivedFriendRequests)
                    .ThenInclude(f => f.Requester)
                .FirstOrDefaultAsync(u => u.Id == userId);
        }

        public async Task<bool> IsUsernameTakenAsync(string username)
        {
            return await _dbContext.Users
                .AnyAsync(u => (u.FirstName + "." + u.LastName).ToLower() == username.ToLower());
        }

        public async Task<bool> IsEmailRegisteredAsync(string email)
        {
            return await _dbContext.Users
                .AnyAsync(u => u.Id == email);
        }

        public async Task<List<User>> GetMostActiveUsersAsync(int count = 10)
        {
            return await _dbContext.Users
                .Where(u => u.IsActive)
                .OrderByDescending(u => u.Posts.Count + u.Comments.Count)
                .Take(count)
                .ToListAsync();
        }

        public async Task<List<User>> GetRecentUsersAsync(int count = 10)
        {
            return await _dbContext.Users
                .Where(u => u.IsActive)
                .OrderByDescending(u => u.Created)
                .Take(count)
                .ToListAsync();
        }
    }
}

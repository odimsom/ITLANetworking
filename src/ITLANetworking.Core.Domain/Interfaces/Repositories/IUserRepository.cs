using ITLANetworking.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ITLANetworking.Core.Domain.Interfaces.Repositories
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<User?> GetByIdAsync(string id);
        Task UpdateAsync(User entity, string id);
        Task<List<User>> SearchUsersByUsernameAsync(string searchTerm);
        Task<User?> GetByUsernameAsync(string username);
        Task<User?> GetByEmailAsync(string email);
        Task<List<User>> GetActiveUsersAsync();
        Task<User?> GetUserWithPostsAsync(string userId);
        Task<User?> GetUserWithCommentsAsync(string userId);
        Task<User?> GetUserWithFriendshipsAsync(string userId);
        Task<User?> GetUserWithAllDetailsAsync(string userId);
        Task<bool> IsUsernameTakenAsync(string username);
        Task<bool> IsEmailRegisteredAsync(string email);
        Task<List<User>> GetMostActiveUsersAsync(int count = 10);
        Task<List<User>> GetRecentUsersAsync(int count = 10);
    }
}

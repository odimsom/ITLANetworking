using ITLANetworking.Core.Application.Dtos.User;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ITLANetworking.Core.Application.Interfaces.Services
{
    public interface IUserSyncService
    {
        Task DeactivateUserAsync(string userId);
        Task<UserDto?> GetUserByIdAsync(string userId);
        Task<List<UserDto>> SearchUsersAsync(string searchTerm);
        Task SyncUserAsync(UserDto userDto);
        Task UpdateUserAsync(UserDto userDto);
        Task UpdateUserProfileAsync(string userId, string firstName, string lastName, string? phone, string? profilePicture);
        Task CreateDomainUserAsync(string userId, string firstName, string lastName, string? phone = null, string? profilePicture = null);
        Task ActivateUserAsync(string userId);
    }
}
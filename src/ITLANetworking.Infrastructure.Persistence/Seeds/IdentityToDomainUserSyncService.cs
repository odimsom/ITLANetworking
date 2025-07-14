using ITLANetworking.Core.Domain.Entities;
using ITLANetworking.Infrastructure.Identity.Entities;
using ITLANetworking.Infrastructure.Persistence.Contexts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ITLANetworking.Infrastructure.Persistence.Seeds
{
    public class IdentityToDomainUserSyncService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationContext _context;

        public IdentityToDomainUserSyncService(UserManager<ApplicationUser> userManager, ApplicationContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task SyncAsync()
        {
            var identityUsers = await _userManager.Users.ToListAsync();

            foreach (var identityUser in identityUsers)
            {
                var exists = await _context.Users.AnyAsync(u => u.Id == identityUser.Id);
                if (!exists)
                {
                    var domainUser = new User
                    {
                        Id = identityUser.Id,
                        FirstName = identityUser.FirstName,
                        LastName = identityUser.LastName,
                        Phone = identityUser.PhoneNumber,
                        ProfilePicture = null,
                        IsActive = identityUser.IsActive,
                        CreatedBy = "Seed"
                    };

                    _context.Users.Add(domainUser);
                }
            }

            await _context.SaveChangesAsync();
        }
    }
}

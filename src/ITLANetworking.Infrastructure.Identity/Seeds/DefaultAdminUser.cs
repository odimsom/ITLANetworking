using ITLANetworking.Infrastructure.Identity.Entities;
using Microsoft.AspNetCore.Identity;

namespace ITLANetworking.Infrastructure.Identity.Seeds
{
    public class DefaultAdminUser
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public DefaultAdminUser(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task SeedAsync()
        {
            // Admin details
            var adminUser = new ApplicationUser
            {
                UserName = "admin@itlanetworking.com",
                Email = "admin@itlanetworking.com",
                FirstName = "System",
                LastName = "Administrator",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                IsActive = true
            };

            var user = await _userManager.FindByEmailAsync(adminUser.Email);
            if (user == null)
            {
                await _userManager.CreateAsync(adminUser, "Admin@123");
                await _userManager.AddToRoleAsync(adminUser, "Admin");
            }
        }
    }
}
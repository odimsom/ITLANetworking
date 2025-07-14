using ITLANetworking.Infrastructure.Identity.Entities;
using Microsoft.AspNetCore.Identity;

namespace ITLANetworking.Infrastructure.Identity.Seeds
{
    public class DefaultBasicUser
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public DefaultBasicUser(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task SeedAsync()
        {
            var basicUser = new ApplicationUser
            {
                UserName = "user@itlanetworking.com",
                Email = "user@itlanetworking.com",
                FirstName = "Basic",
                LastName = "User",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                IsActive = true
            };

            var user = await _userManager.FindByEmailAsync(basicUser.Email);
            if (user == null)
            {
                await _userManager.CreateAsync(basicUser, "User@123");
                await _userManager.AddToRoleAsync(basicUser, "Basic");
            }
        }
    }
}

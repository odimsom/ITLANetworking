using Microsoft.AspNetCore.Identity;

namespace ITLANetworking.Infrastructure.Identity.Seeds
{
    public class DefaultRoles
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public DefaultRoles(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task SeedAsync()
        {
            string[] roleNames = { "Admin", "Moderator", "Basic" };

            foreach (var roleName in roleNames)
            {
                var roleExists = await _roleManager.RoleExistsAsync(roleName);
                if (!roleExists)
                {
                    await _roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }
        }
    }
}

using ITLANetworking.Infrastructure.Identity.Seeds;
using ITLANetworking.Infrastructure.Identity.Service.Interfaces;

namespace ITLANetworking.Infrastructure.Identity.Service
{
    public class IdentitySeedService : IIdentitySeedService
    {
        private readonly DefaultRoles _roleSeeder;
        private readonly DefaultAdminUser _adminSeeder;
        private readonly DefaultBasicUser _basicUserSeeder;

        public IdentitySeedService(
            DefaultRoles roleSeeder,
            DefaultAdminUser adminSeeder,
            DefaultBasicUser basicUserSeeder)
        {
            _roleSeeder = roleSeeder;
            _adminSeeder = adminSeeder;
            _basicUserSeeder = basicUserSeeder;
        }

        public async Task SeedAllAsync()
        {
            await _roleSeeder.SeedAsync();
            await _adminSeeder.SeedAsync();
            await _basicUserSeeder.SeedAsync();
        }
    }
}

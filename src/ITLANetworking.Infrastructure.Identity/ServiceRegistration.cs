using ITLANetworking.Infrastructure.Identity.Contexts;
using ITLANetworking.Infrastructure.Identity.Entities;
using ITLANetworking.Infrastructure.Identity.Seeds;
using ITLANetworking.Infrastructure.Identity.Service;
using ITLANetworking.Infrastructure.Identity.Service.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace ITLANetworking.Infrastructure.Identity
{
    public static class ServiceRegistration
    {
        public static void AddIdentityInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            #region Contexts
            services.AddDbContext<IdentityContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("IdentityConnection"),
                    m => m.MigrationsAssembly(typeof(IdentityContext).Assembly.FullName)
                ));
            #endregion

            #region Identity
            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                // Configuraci�n de confirmaci�n de email
                options.SignIn.RequireConfirmedEmail = true;

                // Configuraci�n de contrase�as
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;

                // Configuraci�n de usuario
                options.User.RequireUniqueEmail = true;

                // Configuraci�n de bloqueo (opcional)
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = false;
            })
            .AddEntityFrameworkStores<IdentityContext>()
            .AddDefaultTokenProviders();
            #endregion

            #region Cookie Configuration
            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Account/Login";
                options.AccessDeniedPath = "/Account/AccessDenied";
                options.LogoutPath = "/Account/Logout";

                // Configuraci�n de expiraci�n
                options.ExpireTimeSpan = TimeSpan.FromHours(8);
                options.SlidingExpiration = true;

                // Configuraci�n de seguridad
                options.Cookie.HttpOnly = true;
                options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
                options.Cookie.SameSite = SameSiteMode.Lax;

                // Nombre de la cookie (opcional)
                options.Cookie.Name = "ITLANetworking.Auth";
            });
            #endregion
            #region Seed Configuration
            services.AddTransient<DefaultRoles>();
            services.AddTransient<DefaultAdminUser>();
            services.AddTransient<DefaultBasicUser>();
            services.AddTransient<IIdentitySeedService, IdentitySeedService>();
            #endregion
        }

        public static async Task SeedIdentityDataAsync(this IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var seedService = scope.ServiceProvider.GetRequiredService<IIdentitySeedService>();
            await seedService.SeedAllAsync();
        }
    }
}
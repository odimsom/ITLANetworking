using ITLANetworking.Core.Domain.Interfaces.Repositories;
using ITLANetworking.Infrastructure.Persistence.Contexts;
using ITLANetworking.Infrastructure.Persistence.Repositories;
using ITLANetworking.Infrastructure.Persistence.Seeds;
using ITLANetworking.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace ITLANetworking.Infrastructure.Persistence
{
    public static class ServiceRegistration
    {
        public static void AddPersistenceInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            #region Contexts
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<ApplicationContext>(opt =>
            {
                opt.EnableSensitiveDataLogging();
                opt.UseSqlServer(connectionString,
                    m => m.MigrationsAssembly(typeof(ApplicationContext).Assembly.FullName));
            }, ServiceLifetime.Scoped, ServiceLifetime.Scoped);
            #endregion

            services.AddTransient<IdentityToDomainUserSyncService>();

            #region Repositories
            //services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            //services.AddScoped<IUserRepository, UserRepository>();
            //services.AddScoped<IPostRepository, PostRepository>();
            //services.AddScoped<ICommentRepository, CommentRepository>();
            //services.AddScoped<IPostReactionRepository, PostReactionRepository>();
            //services.AddScoped<IFriendshipRepository, FriendshipRepository>();
            //services.AddScoped<IBattleshipGameRepository, BattleshipGameRepository>();
            //services.AddScoped<IBattleshipShipRepository, BattleshipShipRepository>();
            //services.AddScoped<IBattleshipAttackRepository, BattleshipAttackRepository>();
            services.AddStereotype(typeof(IGenericRepository<>), Assembly.GetExecutingAssembly());
            #endregion
        }
    }
}

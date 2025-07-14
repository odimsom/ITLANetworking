using FluentValidation;
using ITLANetworking.Core.Application.Behaviors;
using ITLANetworking.Core.Application.Interfaces.Services;
using ITLANetworking.Core.Application.Services;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace ITLANetworking.Core.Application
{
    public static class ServiceRegistration
    {
        public static void AddApplicationLayer(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
            
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            
            #region Services
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IPostService, PostService>();
            services.AddScoped<ICommentService, CommentService>();
            services.AddScoped<IFriendshipService, FriendshipService>();
            services.AddScoped<IPostService, PostService>();
            services.AddScoped<IUserSyncService, UserSyncService>();
            services.AddScoped<IBattleshipService, BattleshipService>();
            services.AddScoped<IUserSyncService, UserSyncService>();
            #endregion
        }
    }
}

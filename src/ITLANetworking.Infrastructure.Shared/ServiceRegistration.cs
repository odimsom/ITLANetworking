using ITLANetworking.Core.Application.Interfaces.Services.Shared;
using ITLANetworking.Core.Domain.Settings;
using ITLANetworking.Infrastructure.Shared.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ITLANetworking.Infrastructure.Shared
{
    public static class ServiceRegistration
    {
        public static void AddSharedInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            #region Settings
            services.Configure<MailSettings>(configuration.GetSection("MailSettings"));
            #endregion

            #region Services
            services.AddTransient<IEmailService, EmailService>();
            services.AddTransient<IFileUploadService, FileUploadService>();
            #endregion
        }
    }
}

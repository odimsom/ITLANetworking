using ITLANetworking.Core.Application.Dtos.Email;

namespace ITLANetworking.Core.Application.Interfaces.Services.Shared
{
    public interface IEmailService
    {
        Task SendAsync(EmailRequest request);
    }
}

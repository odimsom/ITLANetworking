using ITLANetworking.Core.Application.Interfaces.Services;
using MediatR;

namespace ITLANetworking.Core.Application.Features.Friends.Commands.SendFriendRequest
{
    public class SendFriendRequestHandler : IRequestHandler<SendFriendRequestCommand, SendFriendRequestResponse>
    {
        private readonly IFriendshipService _friendshipService;

        public SendFriendRequestHandler(IFriendshipService friendshipService)
        {
            _friendshipService = friendshipService;
        }

        public async Task<SendFriendRequestResponse> Handle(SendFriendRequestCommand request, CancellationToken cancellationToken)
        {
            try
            {
                await _friendshipService.SendFriendRequestAsync(request.RequesterId, request.ReceiverId);
                
                return new SendFriendRequestResponse
                {
                    Success = true,
                    Message = "Solicitud de amistad enviada exitosamente"
                };
            }
            catch (Exception ex)
            {
                return new SendFriendRequestResponse
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }
    }
}

using ITLANetworking.Core.Application.Interfaces.Services;
using MediatR;

namespace ITLANetworking.Core.Application.Features.Friends.Commands.AcceptFriendRequest
{
    public class AcceptFriendRequestHandler : IRequestHandler<AcceptFriendRequestCommand, AcceptFriendRequestResponse>
    {
        private readonly IFriendshipService _friendshipService;

        public AcceptFriendRequestHandler(IFriendshipService friendshipService)
        {
            _friendshipService = friendshipService;
        }

        public async Task<AcceptFriendRequestResponse> Handle(AcceptFriendRequestCommand request, CancellationToken cancellationToken)
        {
            try
            {
                await _friendshipService.AcceptFriendRequestAsync(request.FriendshipId);
                
                return new AcceptFriendRequestResponse
                {
                    Success = true,
                    Message = "Solicitud de amistad aceptada"
                };
            }
            catch (Exception ex)
            {
                return new AcceptFriendRequestResponse
                {
                    Success = false,
                    Message = ex.Message
                };
            }
        }
    }
}

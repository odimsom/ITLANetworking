using MediatR;

namespace ITLANetworking.Core.Application.Features.Friends.Commands.SendFriendRequest
{
    public class SendFriendRequestCommand : IRequest<SendFriendRequestResponse>
    {
        public string RequesterId { get; set; } = string.Empty;
        public string ReceiverId { get; set; } = string.Empty;
    }
}

using MediatR;

namespace ITLANetworking.Core.Application.Features.Posts.Commands.DeletePost
{
    public class DeletePostCommand : IRequest<bool>
    {
        public int Id { get; set; }
        public string UserId { get; set; } = string.Empty;
    }

}

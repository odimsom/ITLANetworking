using MediatR;

namespace ITLANetworking.Core.Application.Features.Posts.Commands.ReactToPost
{
    public class ReactToPostCommand : IRequest<ReactToPostResponse>
    {
        public int PostId { get; set; }
        public string UserId { get; set; } = string.Empty;
        public int ReactionType { get; set; } // 1 = Like, 2 = Dislike
    }
}

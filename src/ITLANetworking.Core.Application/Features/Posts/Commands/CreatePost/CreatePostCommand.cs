using MediatR;

namespace ITLANetworking.Core.Application.Features.Posts.Commands.CreatePost
{
    public class CreatePostCommand : IRequest<CreatePostResponse>
    {
        public string Content { get; set; } = string.Empty;
        public string? ImageUrl { get; set; }
        public string? VideoUrl { get; set; }
        public string UserId { get; set; } = string.Empty;
    }
}

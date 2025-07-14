using MediatR;

namespace ITLANetworking.Core.Application.Features.Posts.Commands.UpdatePost
{
    public class UpdatePostCommand : IRequest<bool>
    {
        public int Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public string? ImageUrl { get; set; }
        public string? VideoUrl { get; set; }
        public string UserId { get; set; } = string.Empty;
    }

}

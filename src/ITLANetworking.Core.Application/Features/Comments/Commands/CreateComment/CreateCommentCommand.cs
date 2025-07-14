using MediatR;

namespace ITLANetworking.Core.Application.Features.Comments.Commands.CreateComment
{
    public class CreateCommentCommand : IRequest<CreateCommentResponse>
    {
        public string Content { get; set; } = string.Empty;
        public int PostId { get; set; }
        public string UserId { get; set; } = string.Empty;
        public int? ParentCommentId { get; set; }
    }
}

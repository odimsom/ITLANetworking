namespace ITLANetworking.Core.Application.Features.Comments.Commands.CreateComment
{
    public class CreateCommentResponse
    {
        public int Id { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}

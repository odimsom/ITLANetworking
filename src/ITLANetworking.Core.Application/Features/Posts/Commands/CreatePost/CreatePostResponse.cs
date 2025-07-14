namespace ITLANetworking.Core.Application.Features.Posts.Commands.CreatePost
{
    public class CreatePostResponse
    {
        public int Id { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}

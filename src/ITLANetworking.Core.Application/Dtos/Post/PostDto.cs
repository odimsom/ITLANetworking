using ITLANetworking.Core.Application.Dtos.Comment;
using ITLANetworking.Core.Application.Dtos.Reaction;
using ITLANetworking.Core.Application.Dtos.User;

namespace ITLANetworking.Core.Application.Dtos.Post
{
    public class PostDto
    {
        public int Id { get; set; }
        public string? ReactionType { get; set; }
        public string Content { get; set; } = string.Empty;
        public string? ImageUrl { get; set; }
        public string? VideoUrl { get; set; }
        public DateTime Created { get; set; }
        public DateTime? LastModified { get; set; }
        public string UserId { get; set; } = string.Empty;
        public UserDto User { get; set; } = null!;
        public List<CommentDto> Comments { get; set; } = new();
        public List<ReactionDto> ReactionStats { get; set; } = new();

        public int ReactionsCount => ReactionStats?.Count ?? 0;
        public string? UserReaction { get; set; }
        public bool HasUserReacted => !string.IsNullOrEmpty(UserReaction);
    }
}

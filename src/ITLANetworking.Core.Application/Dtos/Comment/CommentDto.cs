using ITLANetworking.Core.Application.Dtos.User;

namespace ITLANetworking.Core.Application.Dtos.Comment
{
    public class CommentDto
    {
        public int Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public int PostId { get; set; }
        public string UserId { get; set; } = string.Empty;
        public DateTime Created { get; set; }
        public DateTime? LastModified { get; set; }
        public UserDto User { get; set; } = null!;
        
        // Para comentarios anidados (futuro)
        public int? ParentCommentId { get; set; }
        public List<CommentDto> Replies { get; set; } = new();
        public bool HasReplies => Replies?.Any() == true;
        public int RepliesCount => Replies?.Count ?? 0;
    }
}

using ITLANetworking.Core.Domain.Common;

namespace ITLANetworking.Core.Domain.Entities
{
    public class Comment : AuditableBaseEntity
    {
        public string Content { get; set; } = string.Empty;
        public int PostId { get; set; }
        public string UserId { get; set; } = string.Empty;
        public int? ParentCommentId { get; set; }

        // Navigation Properties
        public Post Post { get; set; } = null!;
        public User User { get; set; } = null!;
        public Comment? ParentComment { get; set; }
        public ICollection<Comment> Replies { get; set; } = new List<Comment>();
    }
}

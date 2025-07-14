using ITLANetworking.Core.Domain.Common;

namespace ITLANetworking.Core.Domain.Entities
{
    public class Post : AuditableBaseEntity
    {
        public string Content { get; set; } = string.Empty;
        public string? ImageUrl { get; set; }
        public string? VideoUrl { get; set; }
        public string UserId { get; set; } = string.Empty;

        // Navigation Properties
        public User User { get; set; } = null!;
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public ICollection<PostReaction> Reactions { get; set; } = new List<PostReaction>();
    }
}

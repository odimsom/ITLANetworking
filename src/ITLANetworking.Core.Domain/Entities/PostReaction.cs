using ITLANetworking.Core.Domain.Common;
using ITLANetworking.Core.Domain.Enums;

namespace ITLANetworking.Core.Domain.Entities
{
    public class PostReaction : AuditableBaseEntity
    {
        public int PostId { get; set; }
        public string UserId { get; set; } = string.Empty;
        public ReactionType Type { get; set; }

        // Navigation Properties
        public Post Post { get; set; } = null!;
        public User User { get; set; } = null!;
    }
}

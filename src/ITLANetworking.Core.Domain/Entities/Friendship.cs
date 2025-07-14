using ITLANetworking.Core.Domain.Common;
using ITLANetworking.Core.Domain.Enums;

namespace ITLANetworking.Core.Domain.Entities
{
    public class Friendship : AuditableBaseEntity
    {
        public string RequesterId { get; set; } = string.Empty;
        public string ReceiverId { get; set; } = string.Empty;
        public FriendshipStatus? Status { get; set; }
        public DateTime RequestDate { get; set; }
        public DateTime? ResponseDate { get; set; }

        // Navigation Properties
        public User Requester { get; set; } = null!;
        public User Receiver { get; set; } = null!;
    }
}

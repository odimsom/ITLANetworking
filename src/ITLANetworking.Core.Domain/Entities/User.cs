using ITLANetworking.Core.Domain.Common;

namespace ITLANetworking.Core.Domain.Entities
{
    public class User : AuditableBaseEntity
    {
        public new string Id { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string? ProfilePicture { get; set; }
    public bool IsActive { get; set; } = true;

    // Navigation Properties
    public ICollection<Post> Posts { get; set; } = new List<Post>();
    public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    public ICollection<PostReaction> PostReactions { get; set; } = new List<PostReaction>();
    public ICollection<Friendship> SentFriendRequests { get; set; } = new List<Friendship>();
    public ICollection<Friendship> ReceivedFriendRequests { get; set; } = new List<Friendship>();
    public ICollection<BattleshipGame> Player1Games { get; set; } = new List<BattleshipGame>();
    public ICollection<BattleshipGame> Player2Games { get; set; } = new List<BattleshipGame>();
    public ICollection<BattleshipGame> WonGames { get; set; } = new List<BattleshipGame>();
    public ICollection<BattleshipAttack> BattleshipAttacks { get; set; } = new List<BattleshipAttack>();
    }
}

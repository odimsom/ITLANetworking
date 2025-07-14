using ITLANetworking.Core.Domain.Common;
using ITLANetworking.Core.Domain.Enums;

namespace ITLANetworking.Core.Domain.Entities
{
    public class BattleshipGame : AuditableBaseEntity
    {
        public string Player1Id { get; set; } = string.Empty;
        public string? Player2Id { get; set; }
        public string? CurrentPlayerId { get; set; }
        public string? WinnerId { get; set; }
        public GameStatus? Status { get; set; }
        public bool Player1ShipsConfigured { get; set; } = false;
        public bool Player2ShipsConfigured { get; set; } = false;
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public User Player1 { get; set; } = null!;
        public User? Player2 { get; set; }
        public User? CurrentPlayer { get; set; }
        public User? Winner { get; set; }
        public ICollection<BattleshipShip> Ships { get; set; } = new List<BattleshipShip>();
        public ICollection<BattleshipAttack> Attacks { get; set; } = new List<BattleshipAttack>();
    }
}

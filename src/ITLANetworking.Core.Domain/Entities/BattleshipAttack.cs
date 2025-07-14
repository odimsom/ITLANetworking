using ITLANetworking.Core.Domain.Common;

namespace ITLANetworking.Core.Domain.Entities
{
    public class BattleshipAttack : AuditableBaseEntity
    {
        public int GameId { get; set; }
        public string AttackerId { get; set; } = string.Empty;
        public int Row { get; set; }
        public int Column { get; set; }
        public bool IsHit { get; set; }
        public DateTime AttackDate { get; set; }
        public BattleshipGame Game { get; set; } = null!;
        public User Attacker { get; set; } = null!;
    }
}

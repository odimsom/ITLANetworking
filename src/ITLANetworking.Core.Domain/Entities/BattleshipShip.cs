using ITLANetworking.Core.Domain.Common;
using ITLANetworking.Core.Domain.Enums;

namespace ITLANetworking.Core.Domain.Entities
{
    public class BattleshipShip : AuditableBaseEntity
    {
        public int GameId { get; set; }
        public string PlayerId { get; set; } = string.Empty;
        public int StartRow { get; set; }
        public int StartColumn { get; set; }
        public int Size { get; set; }
        public ShipDirection Direction { get; set; }
        public bool IsSunk { get; set; } = false;
        public BattleshipGame Game { get; set; } = null!;
        public User Player { get; set; } = null!;
    }
}

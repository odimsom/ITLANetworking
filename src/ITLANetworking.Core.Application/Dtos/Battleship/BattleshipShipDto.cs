using ITLANetworking.Core.Domain.Enums;

namespace ITLANetworking.Core.Application.Dtos.Battleship
{
    public class BattleshipShipDto
    {
        public int Id { get; set; }
        public int GameId { get; set; }
        public string PlayerId { get; set; } = string.Empty;
        public int StartX { get; set; }
        public int StartY { get; set; }
        public int EndX { get; set; }
        public int EndY { get; set; }
        public int Size { get; set; }
        public ShipDirection Direction { get; set; }
        public bool IsSunk { get; set; }
    }
}

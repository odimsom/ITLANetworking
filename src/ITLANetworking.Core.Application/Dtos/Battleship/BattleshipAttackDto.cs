namespace ITLANetworking.Core.Application.Dtos.Battleship
{
    public class BattleshipAttackDto
    {
        public int Id { get; set; }
        public int GameId { get; set; }
        public string AttackerId { get; set; } = string.Empty;
        public string TargetId { get; set; } = string.Empty;
        public int X { get; set; }
        public int Y { get; set; }
        public bool IsHit { get; set; }
        public DateTime Created { get; set; }
    }
}

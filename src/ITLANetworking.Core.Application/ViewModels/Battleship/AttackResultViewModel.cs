namespace ITLANetworking.Core.Application.ViewModels.Battleship
{
    public class AttackResultViewModel
    {
        public int X { get; set; }
        public int Y { get; set; }
        public bool IsHit { get; set; }
        public bool IsGameOver { get; set; }
        public int? WinnerId { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}

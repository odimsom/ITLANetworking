namespace ITLANetworking.Core.Application.Features.Battleship.Commands.CreateGame
{
    public class CreateGameResponse
    {
        public int GameId { get; set; }
        public string Player1Id { get; set; }
        public string Player2Id { get; set; }
        public string Status { get; set; }
        public bool HasError { get; set; }
        public string? Error { get; set; }
    }
}

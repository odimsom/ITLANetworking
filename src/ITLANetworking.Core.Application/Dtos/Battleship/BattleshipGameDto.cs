using ITLANetworking.Core.Application.Dtos.User;
using ITLANetworking.Core.Domain.Enums;

namespace ITLANetworking.Core.Application.Dtos.Battleship
{
    public class BattleshipGameDto
    {
        public int Id { get; set; }
        public string Player1Id { get; set; } = string.Empty;
        public string? Player2Id { get; set; }

        public string? CurrentPlayerId { get; set; }

        public string? WinnerId { get; set; }
        public GameStatus Status { get; set; }

        public bool Player1ShipsConfigured { get; set; }
        public bool Player2ShipsConfigured { get; set; }

        public DateTime Created { get; set; }
        public DateTime? EndDate { get; set; } 

        public UserDto Player1 { get; set; } = null!;
        public UserDto? Player2 { get; set; }

        public UserDto? CurrentPlayer { get; set; }

        public UserDto? Winner { get; set; }
        public List<BattleshipShipDto> Ships { get; set; } = new();
        public List<BattleshipAttackDto> Attacks { get; set; } = new();
    }
}
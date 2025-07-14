using ITLANetworking.Core.Application.ViewModels.User;
using ITLANetworking.Core.Domain.Enums;

namespace ITLANetworking.Core.Application.ViewModels.Battleship
{
    public class BattleshipGameViewModel
    {
        public int Id { get; set; }
        public UserViewModel Player1 { get; set; } = new();
        public UserViewModel Player2 { get; set; } = new();
        public GameStatus Status { get; set; }
        public UserViewModel? Winner { get; set; }
        public UserViewModel CurrentTurnPlayer { get; set; } = new();
        public DateTime Created { get; set; }
        public bool IsCurrentUserTurn { get; set; }
        public bool IsCurrentUserPlayer { get; set; }
        public string StatusText => Status switch
        {
            GameStatus.WaitingForPlayers => "Esperando jugadores",
            GameStatus.InProgress => "En progreso",
            GameStatus.Finished => "Finalizado",
            GameStatus.Cancelled => "Cancelado",
            GameStatus.Abandoned => "Abandonado",
            _ => "Desconocido"
        };
    }
}

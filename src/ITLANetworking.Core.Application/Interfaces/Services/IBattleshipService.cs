using ITLANetworking.Core.Application.Dtos.Battleship;
using ITLANetworking.Core.Application.ViewModels.Battleship;
using ITLANetworking.Core.Domain.Entities;

namespace ITLANetworking.Core.Application.Interfaces.Services
{
    public interface IBattleshipService
    {
        Task<BattleshipGameDto> CreateGameAsync(CreateGameDto dto);
        Task<BattleshipGameDto?> GetGameAsync(int gameId);
        Task<List<BattleshipGameDto>> GetActiveGamesAsync(string userId);
        Task<List<BattleshipGameDto>> GetGameHistoryAsync(string userId);
        Task<List<BattleshipGameDto>> GetCompletedGameHistoryAsync(string userId);
        Task<List<BattleshipGameDto>> GetPendingConfigurationGamesAsync(string userId);
        Task CleanupOldConfigurationGamesAsync();
        Task<bool> PlaceShipAsync(PlaceShipDto dto);
        Task<bool> AttackAsync(AttackDto dto);
        Task<bool> SurrenderGameAsync(int gameId, string playerId);
        Task<bool> IsValidShipPlacementAsync(PlaceShipDto dto);
        Task<bool> AreAllShipsPlacedAsync(int gameId, string playerId);
        Task<bool> IsGameOverAsync(int gameId);
        Task<string?> GetWinnerAsync(int gameId);
        Task<List<List<string>>> GetPlayerBoardAsync(int gameId, string playerId, bool showShips = true);
        Task<List<List<string>>> GetOpponentBoardAsync(int gameId, string playerId);
    }
}

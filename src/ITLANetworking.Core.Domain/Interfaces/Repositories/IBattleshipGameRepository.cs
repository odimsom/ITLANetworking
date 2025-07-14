using ITLANetworking.Core.Domain.Entities;
using ITLANetworking.Core.Domain.Enums;

namespace ITLANetworking.Core.Domain.Interfaces.Repositories
{
    public interface IBattleshipGameRepository : IGenericRepository<BattleshipGame>
    {
        Task<List<BattleshipGame>> GetActiveGamesByPlayerIdAsync(string playerId);
        Task<List<BattleshipGame>> GetGamesByPlayerIdAsync(string playerId);
        Task<List<BattleshipGame>> GetActiveGamesAsync(string userId);
        Task<List<BattleshipGame>> GetGameHistoryAsync(string userId);
        Task<List<BattleshipGame>> GetPendingConfigurationGamesAsync(string userId);
        Task<BattleshipGame?> GetGameWithDetailsAsync(int gameId);
        Task<bool> HasActiveGameWithPlayerAsync(string player1Id, string player2Id);
    }
}

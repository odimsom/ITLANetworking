using ITLANetworking.Core.Domain.Entities;
using ITLANetworking.Core.Domain.Enums;

namespace ITLANetworking.Core.Domain.Interfaces.Repositories
{
    public interface IBattleshipShipRepository : IGenericRepository<BattleshipShip>
    {
        Task<List<BattleshipShip>> GetPlayerShipsAsync(int gameId, string playerId);
        Task<BattleshipShip?> GetShipAtPositionAsync(int gameId, string playerId, int row, int column);
        Task<bool> IsPositionOccupiedAsync(int gameId, string playerId, int row, int column);
        Task<List<BattleshipShip>> GetSunkShipsAsync(int gameId, string playerId);
        Task<bool> IsValidShipPlacementAsync(int gameId, string playerId, int startRow, int startColumn, int size, ShipDirection direction);
        Task<bool> CanPlaceShipAsync(int gameId, string playerId, int startRow, int startColumn, int size, ShipDirection direction);
        Task<int> GetShipCountByTypeAsync(int gameId, string playerId, int size);
    }
}

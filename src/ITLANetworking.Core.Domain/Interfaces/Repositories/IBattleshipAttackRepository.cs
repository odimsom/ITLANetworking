using ITLANetworking.Core.Domain.Entities;
using ITLANetworking.Core.Domain.Interfaces;

namespace ITLANetworking.Core.Domain.Interfaces.Repositories
{
    public interface IBattleshipAttackRepository : IGenericRepository<BattleshipAttack>
    {
        Task<List<BattleshipAttack>> GetGameAttacksAsync(int gameId);
        Task<List<BattleshipAttack>> GetPlayerAttacksAsync(int gameId, string playerId);
        Task<bool> HasAttackedPositionAsync(int gameId, string attackerId, int row, int column);
        Task<BattleshipAttack?> GetAttackAtPositionAsync(int gameId, int row, int column);
    }
}

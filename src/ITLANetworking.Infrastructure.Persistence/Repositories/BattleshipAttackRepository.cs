using ITLANetworking.Core.Domain.Entities;
using ITLANetworking.Core.Domain.Interfaces.Repositories;
using ITLANetworking.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace ITLANetworking.Infrastructure.Persistence.Repositories
{
    public class BattleshipAttackRepository : GenericRepository<BattleshipAttack>, IBattleshipAttackRepository
    {
        private readonly ApplicationContext _context;

        public BattleshipAttackRepository(ApplicationContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<BattleshipAttack>> GetGameAttacksAsync(int gameId)
        {
            return await _context.BattleshipAttacks
                .Include(a => a.Attacker)
                .Where(a => a.GameId == gameId)
                .OrderBy(a => a.AttackDate)
                .ToListAsync();
        }

        public async Task<List<BattleshipAttack>> GetAttacksByPlayerAsync(int gameId, string attackerId)
        {
            return await _context.BattleshipAttacks
                .Where(a => a.GameId == gameId && a.AttackerId == attackerId)
                .OrderBy(a => a.AttackDate)
                .ToListAsync();
        }

        public async Task<BattleshipAttack?> GetAttackAtPositionAsync(int gameId, int row, int column)
        {
            try
            {
                return await _context.BattleshipAttacks
                    .FirstOrDefaultAsync(a =>
                        a.GameId == gameId &&
                        a.Row == row &&
                        a.Column == column) ?? throw new KeyNotFoundException("Attack not found at the specified position.");
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving the attack at the specified position.", ex);
            }
        }

        public async Task<bool> HasAttackAtPositionAsync(int gameId, int row, int column)
        {
            return await _context.BattleshipAttacks
                .AnyAsync(a =>
                    a.GameId == gameId &&
                    a.Row == row &&
                    a.Column == column);
        }

        public Task<List<BattleshipAttack>> GetPlayerAttacksAsync(int gameId, string playerId)
        {
            return _context.BattleshipAttacks
                .Where(a => a.GameId == gameId && a.AttackerId == playerId)
                .OrderBy(a => a.AttackDate)
                .ToListAsync();
        }

        public Task<bool> HasAttackedPositionAsync(int gameId, string attackerId, int row, int column)
        {
            return _context.BattleshipAttacks
                .AnyAsync(a =>
                    a.GameId == gameId &&
                    a.AttackerId == attackerId &&
                    a.Row == row &&
                    a.Column == column);
        }
    }
}

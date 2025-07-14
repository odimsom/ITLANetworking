using ITLANetworking.Core.Domain.Entities;
using ITLANetworking.Core.Domain.Enums;
using ITLANetworking.Core.Domain.Interfaces.Repositories;
using ITLANetworking.Infrastructure.Persistence.Contexts;
using ITLANetworking.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ITLANetworking.Infrastructure.Repositories
{
    public class BattleshipGameRepository : GenericRepository<BattleshipGame>, IBattleshipGameRepository
    {
        private readonly ApplicationContext _context;

        public BattleshipGameRepository(ApplicationContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<BattleshipGame>> GetActiveGamesAsync(string userId)
        {
            // ✅ CORREGIDO: Incluir TODOS los juegos donde el usuario participa y no están terminados
            return await _context.BattleshipGames
                .Include(g => g.Player1)
                .Include(g => g.Player2)
                .Include(g => g.CurrentPlayer)
                .Include(g => g.Winner)
                .Where(g => (g.Player1Id == userId || g.Player2Id == userId) &&
                           g.Status != GameStatus.Finished)
                .OrderByDescending(g => g.StartDate)
                .ToListAsync();
        }

        public async Task<List<BattleshipGame>> GetGameHistoryAsync(string userId)
        {
            // ✅ CORREGIDO: Solo juegos terminados para el historial
            return await _context.BattleshipGames
                .Include(g => g.Player1)
                .Include(g => g.Player2)
                .Include(g => g.CurrentPlayer)
                .Include(g => g.Winner)
                .Where(g => (g.Player1Id == userId || g.Player2Id == userId) &&
                           g.Status == GameStatus.Finished)
                .OrderByDescending(g => g.EndDate ?? g.StartDate)
                .ToListAsync();
        }

        public async Task<BattleshipGame?> GetGameWithDetailsAsync(int gameId)
        {
            return await _context.BattleshipGames
                .Include(g => g.Player1)
                .Include(g => g.Player2)
                .Include(g => g.CurrentPlayer)
                .Include(g => g.Winner)
                .Include(g => g.Ships)
                    .ThenInclude(s => s.Player)
                .Include(g => g.Attacks)
                    .ThenInclude(a => a.Attacker)
                .FirstOrDefaultAsync(g => g.Id == gameId);
        }

        public async Task<bool> HasActiveGameWithPlayerAsync(string player1Id, string player2Id)
        {
            return await _context.BattleshipGames
                .AnyAsync(g => ((g.Player1Id == player1Id && g.Player2Id == player2Id) ||
                               (g.Player1Id == player2Id && g.Player2Id == player1Id)) &&
                              g.Status != GameStatus.Finished);
        }

        // ✅ NUEVO: Método para obtener juegos pendientes de configuración
        public async Task<List<BattleshipGame>> GetPendingConfigurationGamesAsync(string userId)
        {
            return await _context.BattleshipGames
                .Include(g => g.Player1)
                .Include(g => g.Player2)
                .Include(g => g.CurrentPlayer)
                .Where(g => (g.Player1Id == userId || g.Player2Id == userId) &&
                           g.Status == GameStatus.ConfigurationShip)
                .OrderByDescending(g => g.StartDate)
                .ToListAsync();
        }

        // ✅ NUEVO: Método para obtener juegos en progreso
        public async Task<List<BattleshipGame>> GetGamesInProgressAsync(string userId)
        {
            return await _context.BattleshipGames
                .Include(g => g.Player1)
                .Include(g => g.Player2)
                .Include(g => g.CurrentPlayer)
                .Where(g => (g.Player1Id == userId || g.Player2Id == userId) &&
                           g.Status == GameStatus.InProgress)
                .OrderByDescending(g => g.StartDate)
                .ToListAsync();
        }

        public async Task<List<BattleshipGame>> GetGamesByPlayerIdAsync(string playerId)
        {
            return await _context.BattleshipGames
                .Include(g => g.Player1)
                .Include(g => g.Player2)
                .Include(g => g.CurrentPlayer)
                .Include(g => g.Winner)
                .Where(g => g.Player1Id == playerId || g.Player2Id == playerId)
                .OrderByDescending(g => g.StartDate)
                .ToListAsync();
        }

        public Task<List<BattleshipGame>> GetActiveGamesByPlayerIdAsync(string playerId)
        {
            return _context.BattleshipGames
                .Include(g => g.Player1)
                .Include(g => g.Player2)
                .Include(g => g.CurrentPlayer)
                .Include(g => g.Winner)
                .Where(g => (g.Player1Id == playerId || g.Player2Id == playerId) &&
                           g.Status != GameStatus.Finished)
                .OrderByDescending(g => g.StartDate)
                .ToListAsync();
        }
    }
}

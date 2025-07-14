using ITLANetworking.Core.Domain.Entities;
using ITLANetworking.Core.Domain.Enums;
using ITLANetworking.Core.Domain.Interfaces.Repositories;
using ITLANetworking.Infrastructure.Persistence.Contexts;
using ITLANetworking.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ITLANetworking.Infrastructure.Repositories
{
    public class BattleshipShipRepository : GenericRepository<BattleshipShip>, IBattleshipShipRepository
    {
        private readonly ApplicationContext _context;

        public BattleshipShipRepository(ApplicationContext context) : base(context)
        {
            _context = context;
        }

        public async Task<bool> IsPositionOccupiedAsync(int gameId, string playerId, int row, int column)
        {
            // Fetch all ships for the player and evaluate position occupation on the client side
            var ships = await _context.BattleshipShips
                .Where(s => s.GameId == gameId && s.PlayerId == playerId)
                .Select(s => new
                {
                    s.StartRow,
                    s.StartColumn,
                    s.Size,
                    s.Direction
                })
                .ToListAsync();

            // Client-side evaluation to check if any ship occupies the target position
            return ships.Any(ship => IsPositionOccupiedByShip(
                ship.StartRow, ship.StartColumn, ship.Size, ship.Direction, row, column));
        }

        public async Task<bool> CanPlaceShipAsync(int gameId, string playerId, int startRow, int startColumn, int size, ShipDirection direction)
        {
            // Check board boundaries first
            if (!IsWithinBounds(startRow, startColumn, size, direction))
            {
                return false;
            }

            // Get positions that the new ship would occupy
            var newShipPositions = GetShipPositions(startRow, startColumn, size, direction);

            // Get all existing ships for the player
            var existingShips = await _context.BattleshipShips
                .Where(s => s.GameId == gameId && s.PlayerId == playerId)
                .Select(s => new
                {
                    s.StartRow,
                    s.StartColumn,
                    s.Size,
                    s.Direction
                })
                .ToListAsync();

            // Check for collisions with existing ships
            foreach (var existingShip in existingShips)
            {
                var existingPositions = GetShipPositions(
                    existingShip.StartRow, existingShip.StartColumn,
                    existingShip.Size, existingShip.Direction);

                // If any position overlaps, return false
                if (newShipPositions.Any(newPos => existingPositions.Any(existingPos =>
                    existingPos.Row == newPos.Row && existingPos.Column == newPos.Column)))
                {
                    return false;
                }
            }

            return true;
        }

        public async Task<int> GetShipCountByTypeAsync(int gameId, string playerId, int size)
        {
            return await _context.BattleshipShips
                .CountAsync(s => s.GameId == gameId && s.PlayerId == playerId && s.Size == size);
        }

        public async Task<List<BattleshipShip>> GetPlayerShipsAsync(int gameId, string playerId)
        {
            return await _context.BattleshipShips
                .Where(s => s.GameId == gameId && s.PlayerId == playerId)
                .ToListAsync();
        }

        public async Task<BattleshipShip?> GetShipAtPositionAsync(int gameId, string playerId, int row, int column)
        {
            // Fetch all ships and evaluate position on client side
            var ships = await _context.BattleshipShips
                .Where(s => s.GameId == gameId && s.PlayerId == playerId)
                .ToListAsync();

            return ships.FirstOrDefault(ship =>
                IsPositionOccupiedByShip(ship.StartRow, ship.StartColumn, ship.Size, ship.Direction, row, column));
        }

        public async Task<List<BattleshipShip>> GetSunkShipsAsync(int gameId, string playerId)
        {
            return await _context.BattleshipShips
                .Where(s => s.GameId == gameId && s.PlayerId == playerId && s.IsSunk)
                .ToListAsync();
        }

        public async Task<bool> IsValidShipPlacementAsync(int gameId, string playerId, int startRow, int startColumn, int size, ShipDirection direction)
        {
            // Check boundaries first
            if (!IsWithinBounds(startRow, startColumn, size, direction))
            {
                return false;
            }

            // Get positions for the new ship
            var newShipPositions = GetShipPositions(startRow, startColumn, size, direction);

            // Get existing ships and check for overlaps
            var existingShips = await _context.BattleshipShips
                .Where(s => s.GameId == gameId && s.PlayerId == playerId)
                .Select(s => new
                {
                    s.StartRow,
                    s.StartColumn,
                    s.Size,
                    s.Direction
                })
                .ToListAsync();

            // Check for any overlapping positions
            foreach (var existingShip in existingShips)
            {
                var existingPositions = GetShipPositions(
                    existingShip.StartRow, existingShip.StartColumn,
                    existingShip.Size, existingShip.Direction);

                if (newShipPositions.Any(newPos => existingPositions.Any(existingPos =>
                    existingPos.Row == newPos.Row && existingPos.Column == newPos.Column)))
                {
                    return false;
                }
            }

            return true;
        }

        // Additional method to get all ships that occupy a specific position (useful for attacks)
        public async Task<List<BattleshipShip>> GetShipsAtPositionAsync(int gameId, string playerId, int row, int column)
        {
            var ships = await _context.BattleshipShips
                .Where(s => s.GameId == gameId && s.PlayerId == playerId)
                .ToListAsync();

            return ships.Where(ship =>
                IsPositionOccupiedByShip(ship.StartRow, ship.StartColumn, ship.Size, ship.Direction, row, column))
                .ToList();
        }

        // Helper method to check if all ships are placed for a player
        public async Task<bool> AreAllShipsPlacedAsync(int gameId, string playerId, Dictionary<int, int> requiredShips)
        {
            var shipCounts = await _context.BattleshipShips
                .Where(s => s.GameId == gameId && s.PlayerId == playerId)
                .GroupBy(s => s.Size)
                .Select(g => new { Size = g.Key, Count = g.Count() })
                .ToListAsync();

            foreach (var requirement in requiredShips)
            {
                var actualCount = shipCounts.FirstOrDefault(sc => sc.Size == requirement.Key)?.Count ?? 0;
                if (actualCount != requirement.Value)
                {
                    return false;
                }
            }

            return true;
        }

        #region Private Helper Methods

        private bool IsWithinBounds(int startRow, int startColumn, int size, ShipDirection direction)
        {
            const int BoardSize = 10;

            return direction switch
            {
                ShipDirection.Up => startRow - size + 1 >= 0,
                ShipDirection.Down => startRow + size - 1 < BoardSize,
                ShipDirection.Left => startColumn - size + 1 >= 0,
                ShipDirection.Right => startColumn + size - 1 < BoardSize,
                _ => false
            };
        }

        private List<(int Row, int Column)> GetShipPositions(int startRow, int startColumn, int size, ShipDirection direction)
        {
            var positions = new List<(int Row, int Column)>();

            for (int i = 0; i < size; i++)
            {
                var (row, column) = direction switch
                {
                    ShipDirection.Up => (startRow - i, startColumn),
                    ShipDirection.Down => (startRow + i, startColumn),
                    ShipDirection.Left => (startRow, startColumn - i),
                    ShipDirection.Right => (startRow, startColumn + i),
                    _ => (startRow, startColumn)
                };

                positions.Add((row, column));
            }

            return positions;
        }

        private bool IsPositionOccupiedByShip(int shipStartRow, int shipStartColumn, int shipSize, ShipDirection shipDirection, int targetRow, int targetColumn)
        {
            for (int i = 0; i < shipSize; i++)
            {
                var (row, column) = shipDirection switch
                {
                    ShipDirection.Up => (shipStartRow - i, shipStartColumn),
                    ShipDirection.Down => (shipStartRow + i, shipStartColumn),
                    ShipDirection.Left => (shipStartRow, shipStartColumn - i),
                    ShipDirection.Right => (shipStartRow, shipStartColumn + i),
                    _ => (shipStartRow, shipStartColumn)
                };

                if (row == targetRow && column == targetColumn)
                {
                    return true;
                }
            }

            return false;
        }

        #endregion
    }
}
using AutoMapper;
using ITLANetworking.Core.Application.Dtos.Battleship;
using ITLANetworking.Core.Application.Interfaces.Services;
using ITLANetworking.Core.Domain.Entities;
using ITLANetworking.Core.Domain.Enums;
using ITLANetworking.Core.Domain.Interfaces.Repositories;

namespace ITLANetworking.Core.Application.Services
{
    public class BattleshipService : IBattleshipService
    {
        private readonly IBattleshipGameRepository _gameRepository;
        private readonly IBattleshipShipRepository _shipRepository;
        private readonly IBattleshipAttackRepository _attackRepository;
        private readonly IFriendshipRepository _friendshipRepository;
        private readonly IMapper _mapper;

        private readonly int[] _shipSizes = { 5, 4, 3, 3, 2 }; // Correcto según spec
        private const int BOARD_SIZE = 10; // Tablero 10x10

        public BattleshipService(
            IBattleshipGameRepository gameRepository,
            IBattleshipShipRepository shipRepository,
            IBattleshipAttackRepository attackRepository,
            IFriendshipRepository friendshipRepository,
            IMapper mapper)
        {
            _gameRepository = gameRepository;
            _shipRepository = shipRepository;
            _attackRepository = attackRepository;
            _friendshipRepository = friendshipRepository;
            _mapper = mapper;
        }

        public async Task<BattleshipGameDto> CreateGameAsync(CreateGameDto dto)
        {
            if (dto.Player2Id != null && !await _friendshipRepository.AreFriendsAsync(dto.Player1Id, dto.Player2Id))
                throw new InvalidOperationException("Solo puedes jugar con tus amigos");

            if (dto.Player2Id != null && await _gameRepository.HasActiveGameWithPlayerAsync(dto.Player1Id, dto.Player2Id))
                throw new InvalidOperationException("Ya tienes un juego activo con este jugador");

            var game = _mapper.Map<BattleshipGame>(dto);
            game.Status = dto.Player2Id != null ? GameStatus.ConfigurationShip : GameStatus.WaitingForPlayers;
            game.CurrentPlayerId = game.Player1Id; // El jugador 1 comienza configurando

            var createdGame = await _gameRepository.AddAsync(game);
            var gameWithDetails = await _gameRepository.GetGameWithDetailsAsync(createdGame.Id);

            return gameWithDetails != null
                ? _mapper.Map<BattleshipGameDto>(gameWithDetails)
                : throw new InvalidOperationException("Error al crear el juego");
        }

        public async Task<BattleshipGameDto?> GetGameAsync(int gameId)
        {
            var game = await _gameRepository.GetGameWithDetailsAsync(gameId);
            return game != null ? _mapper.Map<BattleshipGameDto>(game) : null;
        }

        public async Task<List<BattleshipGameDto>> GetActiveGamesAsync(string userId)
        {
            var games = await _gameRepository.GetActiveGamesAsync(userId);
            return _mapper.Map<List<BattleshipGameDto>>(games);
        }

        public async Task<List<BattleshipGameDto>> GetGameHistoryAsync(string userId)
        {
            var games = await _gameRepository.GetGameHistoryAsync(userId);
            return _mapper.Map<List<BattleshipGameDto>>(games);
        }

        public async Task<List<BattleshipGameDto>> GetCompletedGameHistoryAsync(string userId)
        {
            var games = await _gameRepository.GetGameHistoryAsync(userId);
            // Filtrar solo juegos completados
            var completedGames = games.Where(g => g.Status == GameStatus.Finished).ToList();
            return _mapper.Map<List<BattleshipGameDto>>(completedGames);
        }

        public async Task<List<BattleshipGameDto>> GetPendingConfigurationGamesAsync(string userId)
        {
            var games = await _gameRepository.GetActiveGamesAsync(userId);
            // Filtrar solo juegos en configuración
            var pendingGames = games.Where(g => g.Status == GameStatus.ConfigurationShip).ToList();
            return _mapper.Map<List<BattleshipGameDto>>(pendingGames);
        }

        public async Task CleanupOldConfigurationGamesAsync()
        {
            var cutoffTime = DateTime.UtcNow.AddMinutes(-30);
            var games = await _gameRepository.GetAllAsync();
            var oldGames = games
                .Where(g => g.Status == GameStatus.ConfigurationShip && g.StartDate < cutoffTime)
                .ToList();

            foreach (var game in oldGames)
            {
                game.Status = GameStatus.Finished;
                game.EndDate = DateTime.UtcNow;
                game.WinnerId = null; // Sin ganador
                await _gameRepository.UpdateAsync(game, game.Id);
            }
        }

        public async Task<bool> PlaceShipAsync(PlaceShipDto dto)
        {
            var game = await _gameRepository.GetByIdAsync(dto.GameId);
            if (game == null || game.Status != GameStatus.ConfigurationShip)
                return false;

            // Validar que sea el turno del jugador para configurar
            if (game.CurrentPlayerId != dto.PlayerId)
                return false;

            // Validar que no exceda la cantidad permitida de barcos de ese tamaño
            var currentShipsOfType = await _shipRepository.GetShipCountByTypeAsync(dto.GameId, dto.PlayerId, dto.Size);
            var maxAllowed = GetMaxShipsOfType(dto.Size);

            if (currentShipsOfType >= maxAllowed)
                return false;

            // ✅ AutoMapper maneja la conversión X/Y a Row/Column automáticamente
            if (!await _shipRepository.CanPlaceShipAsync(dto.GameId, dto.PlayerId, dto.StartY, dto.StartX, dto.Size, dto.Direction))
                return false;

            var ship = _mapper.Map<BattleshipShip>(dto);
            await _shipRepository.AddAsync(ship);

            // Verificar si el jugador ha terminado de colocar todos sus barcos
            if (await AreAllShipsPlacedAsync(dto.GameId, dto.PlayerId))
            {
                await UpdateGameConfigurationStatus(dto.GameId, dto.PlayerId);
            }

            return true;
        }

        public async Task<bool> AttackAsync(AttackDto dto)
        {
            var game = await _gameRepository.GetByIdAsync(dto.GameId);
            if (game == null || game.Status != GameStatus.InProgress)
                return false;

            if (game.CurrentPlayerId != dto.AttackerId)
                return false;

            // ✅ AutoMapper maneja la conversión X/Y a Row/Column automáticamente
            if (await _attackRepository.HasAttackedPositionAsync(dto.GameId, dto.AttackerId, dto.Y, dto.X))
                return false;

            var opponentId = dto.AttackerId == game.Player1Id ? game.Player2Id : game.Player1Id;
            if (opponentId == null)
                return false;

            var attackEntity = _mapper.Map<BattleshipAttack>(dto);
            var ship = await _shipRepository.GetShipAtPositionAsync(dto.GameId, opponentId, dto.Y, dto.X);

            attackEntity.IsHit = ship != null;
            await _attackRepository.AddAsync(attackEntity);

            if (attackEntity.IsHit && ship != null)
            {
                await CheckIfShipIsSunk(ship);
            }

            // ✅ CORREGIDO: Verificar si el juego ha terminado
            if (await IsGameOverAsync(dto.GameId))
            {
                game.Status = GameStatus.Finished;
                game.WinnerId = dto.AttackerId;
                game.EndDate = DateTime.UtcNow;
            }
            else
            {
                // ✅ NUEVA LÓGICA: Solo cambiar turno si FALLÓ el ataque
                // Si acertó, el jugador sigue jugando
                if (!attackEntity.IsHit)
                {
                    game.CurrentPlayerId = opponentId;
                }
                // Si acertó (IsHit = true), el turno NO cambia, sigue siendo del mismo jugador
            }

            await _gameRepository.UpdateAsync(game, dto.GameId);
            return true;
        }

        public async Task<bool> SurrenderGameAsync(int gameId, string playerId)
        {
            var game = await _gameRepository.GetByIdAsync(gameId);
            if (game == null || (game.Status != GameStatus.InProgress && game.Status != GameStatus.ConfigurationShip))
                return false;

            if (game.Player1Id != playerId && game.Player2Id != playerId)
                return false;

            game.Status = GameStatus.Finished;
            game.WinnerId = game.Player1Id == playerId ? game.Player2Id : game.Player1Id;
            game.EndDate = DateTime.UtcNow;

            await _gameRepository.UpdateAsync(game, gameId);
            return true;
        }

        public async Task<bool> IsValidShipPlacementAsync(PlaceShipDto dto)
        {
            return await _shipRepository.CanPlaceShipAsync(dto.GameId, dto.PlayerId, dto.StartY, dto.StartX, dto.Size, dto.Direction);
        }

        public async Task<bool> AreAllShipsPlacedAsync(int gameId, string playerId)
        {
            var ships = await _shipRepository.GetPlayerShipsAsync(gameId, playerId);
            var totalCells = ships.Sum(s => s.Size);
            return totalCells >= _shipSizes.Sum();
        }

        public async Task<bool> IsGameOverAsync(int gameId)
        {
            var game = await _gameRepository.GetByIdAsync(gameId);
            if (game == null || game.Player2Id == null) return false;

            var player1Ships = await _shipRepository.GetPlayerShipsAsync(gameId, game.Player1Id);
            var player2Ships = await _shipRepository.GetPlayerShipsAsync(gameId, game.Player2Id);

            if (!player1Ships.Any() || !player2Ships.Any())
                return false;

            var player1AllSunk = player1Ships.All(s => s.IsSunk);
            var player2AllSunk = player2Ships.All(s => s.IsSunk);

            return player1AllSunk || player2AllSunk;
        }

        public async Task<string?> GetWinnerAsync(int gameId)
        {
            var game = await _gameRepository.GetByIdAsync(gameId);
            if (game == null || game.Status != GameStatus.Finished) return null;
            return game.WinnerId;
        }

        public async Task<List<List<string>>> GetPlayerBoardAsync(int gameId, string playerId, bool showShips = true)
        {
            var board = InitializeBoard();

            if (showShips)
            {
                var ships = await _shipRepository.GetPlayerShipsAsync(gameId, playerId);
                foreach (var ship in ships)
                {
                    MarkShipOnBoard(board, ship);
                }
            }

            var attacks = await _attackRepository.GetGameAttacksAsync(gameId);
            var attacksOnPlayer = attacks.Where(a => a.AttackerId != playerId);

            foreach (var attack in attacksOnPlayer)
            {
                if (attack.Row >= 0 && attack.Row < BOARD_SIZE && attack.Column >= 0 && attack.Column < BOARD_SIZE)
                {
                    if (attack.IsHit)
                        board[attack.Row][attack.Column] = "X";
                    else
                        board[attack.Row][attack.Column] = "O";
                }
            }

            return board;
        }

        public async Task<List<List<string>>> GetOpponentBoardAsync(int gameId, string playerId)
        {
            var board = InitializeBoard();
            var attacks = await _attackRepository.GetPlayerAttacksAsync(gameId, playerId);

            foreach (var attack in attacks)
            {
                if (attack.Row >= 0 && attack.Row < BOARD_SIZE && attack.Column >= 0 && attack.Column < BOARD_SIZE)
                {
                    if (attack.IsHit)
                        board[attack.Row][attack.Column] = "X";
                    else
                        board[attack.Row][attack.Column] = "O";
                }
            }

            return board;
        }

        #region Private Methods

        private async Task UpdateGameConfigurationStatus(int gameId, string playerId)
        {
            var game = await _gameRepository.GetByIdAsync(gameId);
            if (game == null) return;

            if (playerId == game.Player1Id)
            {
                game.Player1ShipsConfigured = true;
                if (!game.Player2ShipsConfigured && game.Player2Id != null)
                {
                    game.CurrentPlayerId = game.Player2Id;
                }
            }
            else
            {
                game.Player2ShipsConfigured = true;
            }

            if (game.Player1ShipsConfigured && game.Player2ShipsConfigured)
            {
                game.Status = GameStatus.InProgress;
                game.CurrentPlayerId = game.Player1Id; // Player1 comienza atacando
            }

            await _gameRepository.UpdateAsync(game, gameId);
        }

        private List<List<string>> InitializeBoard()
        {
            var board = new List<List<string>>();
            for (int i = 0; i < BOARD_SIZE; i++)
            {
                var row = new List<string>();
                for (int j = 0; j < BOARD_SIZE; j++)
                {
                    row.Add("~");
                }
                board.Add(row);
            }
            return board;
        }

        private void MarkShipOnBoard(List<List<string>> board, BattleshipShip ship)
        {
            for (int i = 0; i < ship.Size; i++)
            {
                int row = ship.StartRow;
                int column = ship.StartColumn;

                switch (ship.Direction)
                {
                    case ShipDirection.Right:
                        column += i;
                        break;
                    case ShipDirection.Down:
                        row += i;
                        break;
                    case ShipDirection.Left:
                        column -= i;
                        break;
                    case ShipDirection.Up:
                        row -= i;
                        break;
                }

                if (row >= 0 && row < BOARD_SIZE && column >= 0 && column < BOARD_SIZE)
                {
                    board[row][column] = ship.IsSunk ? "S" : "B";
                }
            }
        }

        private async Task CheckIfShipIsSunk(BattleshipShip ship)
        {
            var attacks = await _attackRepository.GetGameAttacksAsync(ship.GameId);
            var shipPositions = GetShipPositions(ship);

            var hitPositions = attacks
                .Where(a => a.IsHit && shipPositions.Any(p => p.Row == a.Row && p.Column == a.Column))
                .Count();

            if (hitPositions >= ship.Size)
            {
                ship.IsSunk = true;
                await _shipRepository.UpdateAsync(ship, ship.Id);
            }
        }

        private List<(int Row, int Column)> GetShipPositions(BattleshipShip ship)
        {
            var positions = new List<(int Row, int Column)>();

            for (int i = 0; i < ship.Size; i++)
            {
                int row = ship.StartRow;
                int column = ship.StartColumn;

                switch (ship.Direction)
                {
                    case ShipDirection.Right:
                        column += i;
                        break;
                    case ShipDirection.Down:
                        row += i;
                        break;
                    case ShipDirection.Left:
                        column -= i;
                        break;
                    case ShipDirection.Up:
                        row -= i;
                        break;
                }

                positions.Add((row, column));
            }

            return positions;
        }

        private int GetMaxShipsOfType(int size)
        {
            return size switch
            {
                5 => 1, // Portaaviones
                4 => 1, // Acorazado
                3 => 2, // Crucero/Submarino
                2 => 1, // Destructor
                _ => 0
            };
        }

        #endregion
    }
}

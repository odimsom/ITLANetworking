using ITLANetworking.Core.Application.Dtos.Battleship;
using ITLANetworking.Core.Application.Features.Friends.Queries.GetFriends;
using ITLANetworking.Core.Application.Features.Battleship.Commands.CreateGame;
using ITLANetworking.Core.Application.Interfaces.Services;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ITLANetworking.Presentation.WebApp.Controllers
{
    [Authorize]
    public class BattleshipController : Controller
    {
        private readonly IBattleshipService _battleshipService;
        private readonly IFriendshipService _friendshipService;
        private readonly IMediator _mediator;

        public BattleshipController(IBattleshipService battleshipService, IFriendshipService friendshipService, IMediator mediator)
        {
            _battleshipService = battleshipService;
            _friendshipService = friendshipService;
            _mediator = mediator;
        }

        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

            // ✅ CORREGIDO: Obtener todos los juegos donde el usuario participa
            var activeGames = await _battleshipService.GetActiveGamesAsync(userId);
            var gameHistory = await _battleshipService.GetCompletedGameHistoryAsync(userId);
            var friends = await _mediator.Send(new GetFriendsQuery { UserId = userId });

            // ✅ NUEVO: Separar juegos por estado para mejor visualización
            var gamesInProgress = activeGames.Where(g => g.Status.ToString() == "InProgress").ToList();
            var gamesInConfiguration = activeGames.Where(g => g.Status.ToString() == "ConfigurationShip").ToList();
            var gamesWaitingForPlayers = activeGames.Where(g => g.Status.ToString() == "WaitingForPlayers").ToList();

            ViewBag.ActiveGames = activeGames;
            ViewBag.GamesInProgress = gamesInProgress;
            ViewBag.GamesInConfiguration = gamesInConfiguration;
            ViewBag.GamesWaitingForPlayers = gamesWaitingForPlayers;
            ViewBag.GameHistory = gameHistory;
            ViewBag.Friends = friends;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateGame(string opponentId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

            try
            {
                var command = new CreateGameCommand
                {
                    Player1Id = userId,
                    Player2Id = opponentId
                };

                var response = await _mediator.Send(command);

                if (response.HasError)
                {
                    TempData["ErrorMessage"] = response.Error;
                    return RedirectToAction("Index");
                }

                TempData["SuccessMessage"] = "¡Juego creado exitosamente! Tu oponente ha sido notificado.";
                return RedirectToAction("Game", new { id = response.GameId });
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("Index");
            }
        }

        public async Task<IActionResult> Game(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var game = await _battleshipService.GetGameAsync(id);

            if (game == null || (game.Player1Id != userId && game.Player2Id != userId))
            {
                TempData["ErrorMessage"] = "No tienes acceso a este juego.";
                return RedirectToAction("Index");
            }

            // ✅ NUEVO: Verificar si el juego está abandonado o terminado
            if (game.Status.ToString() == "Finished")
            {
                return RedirectToAction("GameResult", new { id });
            }

            var playerBoard = await _battleshipService.GetPlayerBoardAsync(id, userId);
            var opponentBoard = await _battleshipService.GetOpponentBoardAsync(id, userId);

            ViewBag.Game = game;
            ViewBag.PlayerBoard = playerBoard;
            ViewBag.OpponentBoard = opponentBoard;
            ViewBag.CurrentUserId = userId;
            ViewBag.IsPlayerTurn = game.CurrentPlayerId == userId;

            // ✅ NUEVO: Información adicional para la vista
            ViewBag.PlayerShipsConfigured = userId == game.Player1Id ? game.Player1ShipsConfigured : game.Player2ShipsConfigured;
            ViewBag.OpponentShipsConfigured = userId == game.Player1Id ? game.Player2ShipsConfigured : game.Player1ShipsConfigured;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> PlaceShip(PlaceShipDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            dto.PlayerId = userId;

            try
            {
                var game = await _battleshipService.GetGameAsync(dto.GameId);
                if (game == null || game.Status.ToString() == "Finished")
                {
                    TempData["ErrorMessage"] = "El juego ya no está activo";
                    return RedirectToAction("Index");
                }

                // ✅ NUEVO: Validar que sea el turno del jugador para configurar
                if (game.CurrentPlayerId != userId)
                {
                    TempData["ErrorMessage"] = "No es tu turno para configurar barcos";
                    return RedirectToAction("Game", new { id = dto.GameId });
                }

                var success = await _battleshipService.PlaceShipAsync(dto);
                if (success)
                {
                    TempData["SuccessMessage"] = "Barco colocado exitosamente";

                    // ✅ NUEVO: Verificar si todos los barcos están colocados
                    if (await _battleshipService.AreAllShipsPlacedAsync(dto.GameId, userId))
                    {
                        TempData["SuccessMessage"] = "¡Todos tus barcos han sido colocados! Esperando al oponente...";
                    }
                }
                else
                {
                    TempData["ErrorMessage"] = "No se pudo colocar el barco en esa posición. Verifica que no se superponga con otros barcos y esté dentro del tablero.";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }

            return RedirectToAction("Game", new { id = dto.GameId });
        }

        [HttpPost]
        public async Task<IActionResult> Attack(AttackDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            dto.AttackerId = userId;

            try
            {
                var game = await _battleshipService.GetGameAsync(dto.GameId);
                if (game == null || game.Status.ToString() != "InProgress")
                {
                    TempData["ErrorMessage"] = "El juego no está en progreso";
                    return RedirectToAction("Game", new { id = dto.GameId });
                }

                if (game.CurrentPlayerId != userId)
                {
                    TempData["ErrorMessage"] = "No es tu turno para atacar";
                    return RedirectToAction("Game", new { id = dto.GameId });
                }

                var success = await _battleshipService.AttackAsync(dto);
                if (success)
                {
                    // ✅ NUEVO: Verificar si el juego terminó
                    var updatedGame = await _battleshipService.GetGameAsync(dto.GameId);
                    if (updatedGame?.Status.ToString() == "Finished")
                    {
                        if (updatedGame.WinnerId == userId)
                        {
                            TempData["SuccessMessage"] = "¡Felicidades! ¡Has ganado el juego!";
                        }
                        else
                        {
                            TempData["ErrorMessage"] = "Has perdido el juego.";
                        }
                    }
                    else
                    {
                        // ✅ NUEVO: Verificar si acertó para mostrar mensaje apropiado
                        var position = $"{(char)('A' + dto.X)}{dto.Y + 1}";

                        // Verificar si el ataque fue un acierto
                        var opponentId = updatedGame?.Player1Id == userId ? updatedGame.Player2Id : updatedGame?.Player1Id;
                        if (opponentId != null)
                        {
                            var ship = await _battleshipService.GetPlayerBoardAsync(dto.GameId, opponentId, showShips: true);
                            var isHit = ship[dto.Y][dto.X] == "X";

                            if (isHit)
                            {
                                if (updatedGame?.CurrentPlayerId == userId)
                                {
                                    TempData["SuccessMessage"] = $"¡Acierto en {position}! 🎯 Sigues jugando.";
                                }
                                else
                                {
                                    TempData["SuccessMessage"] = $"¡Acierto en {position}! 🎯";
                                }
                            }
                            else
                            {
                                TempData["SuccessMessage"] = $"Agua en {position} 💧 Turno del oponente.";
                            }
                        }
                        else
                        {
                            TempData["SuccessMessage"] = "Ataque realizado";
                        }
                    }
                }
                else
                {
                    TempData["ErrorMessage"] = "No se pudo realizar el ataque. Verifica que no hayas atacado esa posición antes.";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }

            return RedirectToAction("Game", new { id = dto.GameId });
        }

        [HttpGet]
        public async Task<JsonResult> GetGameState(int gameId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

            try
            {
                var game = await _battleshipService.GetGameAsync(gameId);
                if (game == null || (game.Player1Id != userId && game.Player2Id != userId))
                {
                    return Json(new { success = false, error = "Juego no encontrado" });
                }

                return Json(new
                {
                    success = true,
                    status = game.Status.ToString(),
                    currentPlayerId = game.CurrentPlayerId,
                    isPlayerTurn = game.CurrentPlayerId == userId,
                    winner = game.WinnerId,
                    player1ShipsConfigured = game.Player1ShipsConfigured,
                    player2ShipsConfigured = game.Player2ShipsConfigured
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> SurrenderGame(int gameId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

            try
            {
                var success = await _battleshipService.SurrenderGameAsync(gameId, userId);
                if (success)
                {
                    TempData["SuccessMessage"] = "Has abandonado el juego";
                }
                else
                {
                    TempData["ErrorMessage"] = "No se pudo abandonar el juego";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> GameResult(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            var game = await _battleshipService.GetGameAsync(id);

            if (game == null || (game.Player1Id != userId && game.Player2Id != userId))
            {
                return NotFound();
            }

            if (game.Status.ToString() != "Finished")
            {
                return RedirectToAction("Game", new { id });
            }

            var playerBoard = await _battleshipService.GetPlayerBoardAsync(id, userId, showShips: true);
            var opponentBoard = await _battleshipService.GetOpponentBoardAsync(id, userId);

            ViewBag.Game = game;
            ViewBag.PlayerBoard = playerBoard;
            ViewBag.OpponentBoard = opponentBoard;
            ViewBag.CurrentUserId = userId;

            return View();
        }

        // ✅ NUEVO: Método para rechazar un juego
        [HttpPost]
        public async Task<IActionResult> DeclineGame(int gameId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

            try
            {
                var game = await _battleshipService.GetGameAsync(gameId);
                if (game == null || game.Player2Id != userId)
                {
                    TempData["ErrorMessage"] = "No puedes rechazar este juego";
                    return RedirectToAction("Index");
                }

                if (game.Status.ToString() != "ConfigurationShip")
                {
                    TempData["ErrorMessage"] = "Este juego ya no se puede rechazar";
                    return RedirectToAction("Index");
                }

                var success = await _battleshipService.SurrenderGameAsync(gameId, userId);
                if (success)
                {
                    TempData["SuccessMessage"] = "Has rechazado el desafío";
                }
                else
                {
                    TempData["ErrorMessage"] = "No se pudo rechazar el juego";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }

            return RedirectToAction("Index");
        }
    }
}

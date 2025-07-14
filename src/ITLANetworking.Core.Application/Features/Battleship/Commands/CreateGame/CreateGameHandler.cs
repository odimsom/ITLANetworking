using AutoMapper;
using ITLANetworking.Core.Domain.Entities;
using ITLANetworking.Core.Domain.Enums;
using ITLANetworking.Core.Domain.Interfaces.Repositories;
using MediatR;

namespace ITLANetworking.Core.Application.Features.Battleship.Commands.CreateGame
{
    public class CreateGameHandler : IRequestHandler<CreateGameCommand, CreateGameResponse>
    {
        private readonly IBattleshipGameRepository _gameRepository;
        private readonly IMapper _mapper;

        public CreateGameHandler(IBattleshipGameRepository gameRepository, IMapper mapper)
        {
            _gameRepository = gameRepository;
            _mapper = mapper;
        }

        public async Task<CreateGameResponse> Handle(CreateGameCommand request, CancellationToken cancellationToken)
        {
            try
            {

                if (request.Player2Id != null && await _gameRepository.HasActiveGameWithPlayerAsync(request.Player1Id, request.Player2Id))
                    return new CreateGameResponse { HasError = true, Error = "Ya tienes un juego activo con este jugador" };

                var game = new BattleshipGame
                {
                    Player1Id = request.Player1Id,
                    Player2Id = request.Player2Id,
                    CurrentPlayerId = request.Player1Id,
                    Status = request.Player2Id != null ? GameStatus.ConfigurationShip : GameStatus.WaitingForPlayers,
                    StartDate = DateTime.UtcNow
                };

                await _gameRepository.AddAsync(game);

                return new CreateGameResponse
                {
                    GameId = game.Id,
                    Player1Id = game.Player1Id,
                    Player2Id = game.Player2Id,
                    Status = game.Status.ToString(),
                    HasError = false
                };
            }
            catch (Exception ex)
            {
                return new CreateGameResponse
                {
                    HasError = true,
                    Error = ex.Message
                };
            }
        }
    }
}

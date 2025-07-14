using MediatR;

namespace ITLANetworking.Core.Application.Features.Battleship.Commands.CreateGame
{
    public class CreateGameCommand : IRequest<CreateGameResponse>
    {
        public string Player1Id { get; set; }
        public string Player2Id { get; set; }
    }
}

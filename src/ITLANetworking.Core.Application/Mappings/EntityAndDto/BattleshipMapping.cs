using AutoMapper;
using ITLANetworking.Core.Application.Dtos.Battleship;
using ITLANetworking.Core.Domain.Entities;
using ITLANetworking.Core.Domain.Enums;

namespace ITLANetworking.Core.Application.Mappings.EntityAndDto
{
    public class BattleshipMapping : Profile
    {
        public BattleshipMapping()
        {
            // BattleshipGame mappings
            CreateMap<BattleshipGame, BattleshipGameDto>()
                .ForMember(dest => dest.Player1ShipsConfigured, opt => opt.MapFrom(src => src.Player1ShipsConfigured))
                .ForMember(dest => dest.Player2ShipsConfigured, opt => opt.MapFrom(src => src.Player2ShipsConfigured))
                .ForMember(dest => dest.CurrentPlayerId, opt => opt.MapFrom(src => src.CurrentPlayerId))
                .ForMember(dest => dest.Created, opt => opt.MapFrom(src => src.StartDate))
                .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.EndDate));

            CreateMap<BattleshipGameDto, BattleshipGame>()
                .ForMember(dest => dest.Player1ShipsConfigured, opt => opt.MapFrom(src => src.Player1ShipsConfigured))
                .ForMember(dest => dest.Player2ShipsConfigured, opt => opt.MapFrom(src => src.Player2ShipsConfigured))
                .ForMember(dest => dest.CurrentPlayerId, opt => opt.MapFrom(src => src.CurrentPlayerId))
                .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => src.Created))
                .ForMember(dest => dest.EndDate, opt => opt.MapFrom(src => src.EndDate));

            // CreateGameDto to BattleshipGame mapping
            CreateMap<CreateGameDto, BattleshipGame>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => GameStatus.ConfigurationShip))
                .ForMember(dest => dest.StartDate, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.CurrentPlayerId, opt => opt.MapFrom(src => src.Player1Id))
                .ForMember(dest => dest.Player1ShipsConfigured, opt => opt.MapFrom(src => false))
                .ForMember(dest => dest.Player2ShipsConfigured, opt => opt.MapFrom(src => false))
                .ForMember(dest => dest.Ships, opt => opt.Ignore())
                .ForMember(dest => dest.Attacks, opt => opt.Ignore())
                .ForMember(dest => dest.Player1, opt => opt.Ignore())
                .ForMember(dest => dest.Player2, opt => opt.Ignore())
                .ForMember(dest => dest.CurrentPlayer, opt => opt.Ignore())
                .ForMember(dest => dest.Winner, opt => opt.Ignore());

            // PlaceShipDto to BattleshipShip mapping
            CreateMap<PlaceShipDto, BattleshipShip>()
                .ForMember(dest => dest.StartRow, opt => opt.MapFrom(src => src.StartY))
                .ForMember(dest => dest.StartColumn, opt => opt.MapFrom(src => src.StartX))
                .ForMember(dest => dest.IsSunk, opt => opt.MapFrom(src => false))
                .ForMember(dest => dest.Game, opt => opt.Ignore())
                .ForMember(dest => dest.Player, opt => opt.Ignore());

            // AttackDto to BattleshipAttack mapping
            CreateMap<AttackDto, BattleshipAttack>()
                .ForMember(dest => dest.Row, opt => opt.MapFrom(src => src.Y))
                .ForMember(dest => dest.Column, opt => opt.MapFrom(src => src.X))
                .ForMember(dest => dest.AttackDate, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.IsHit, opt => opt.Ignore())
                .ForMember(dest => dest.Game, opt => opt.Ignore())
                .ForMember(dest => dest.Attacker, opt => opt.Ignore());

            // BattleshipShip to BattleshipShipDto mapping
            CreateMap<BattleshipShip, BattleshipShipDto>()
                .ForMember(dest => dest.StartX, opt => opt.MapFrom(src => src.StartColumn))
                .ForMember(dest => dest.StartY, opt => opt.MapFrom(src => src.StartRow))
                .ForMember(dest => dest.EndX, opt => opt.MapFrom(src => CalculateEndX(src)))
                .ForMember(dest => dest.EndY, opt => opt.MapFrom(src => CalculateEndY(src)));

            // BattleshipShipDto to BattleshipShip mapping
            CreateMap<BattleshipShipDto, BattleshipShip>()
                .ForMember(dest => dest.StartColumn, opt => opt.MapFrom(src => src.StartX))
                .ForMember(dest => dest.StartRow, opt => opt.MapFrom(src => src.StartY));

            // BattleshipAttack to BattleshipAttackDto mapping
            CreateMap<BattleshipAttack, BattleshipAttackDto>()
                .ForMember(dest => dest.X, opt => opt.MapFrom(src => src.Column))
                .ForMember(dest => dest.Y, opt => opt.MapFrom(src => src.Row))
                .ForMember(dest => dest.Created, opt => opt.MapFrom(src => src.AttackDate))
                .ForMember(dest => dest.TargetId, opt => opt.Ignore());

            // BattleshipAttackDto to BattleshipAttack mapping
            CreateMap<BattleshipAttackDto, BattleshipAttack>()
                .ForMember(dest => dest.Column, opt => opt.MapFrom(src => src.X))
                .ForMember(dest => dest.Row, opt => opt.MapFrom(src => src.Y))
                .ForMember(dest => dest.AttackDate, opt => opt.MapFrom(src => src.Created));
        }

        // Helper methods
        private int CalculateEndX(BattleshipShip ship)
        {
            return ship.Direction switch
            {
                ShipDirection.Right => ship.StartColumn + ship.Size - 1,
                ShipDirection.Left => ship.StartColumn - ship.Size + 1,
                _ => ship.StartColumn
            };
        }

        private int CalculateEndY(BattleshipShip ship)
        {
            return ship.Direction switch
            {
                ShipDirection.Down => ship.StartRow + ship.Size - 1,
                ShipDirection.Up => ship.StartRow - ship.Size + 1,
                _ => ship.StartRow
            };
        }
    }
}

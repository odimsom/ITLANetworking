using ITLANetworking.Core.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace ITLANetworking.Core.Application.Dtos.Battleship
{
    public class PlaceShipDto
    {
        [Required]
        public int GameId { get; set; }

        [Required]
        public string PlayerId { get; set; } = string.Empty;

        [Required]
        [Range(0, 9)]
        public int StartX { get; set; }

        [Required]
        [Range(0, 9)]
        public int StartY { get; set; }

        [Required]
        [Range(2, 5)]
        public int Size { get; set; }

        [Required]
        public ShipDirection Direction { get; set; }
    }
}

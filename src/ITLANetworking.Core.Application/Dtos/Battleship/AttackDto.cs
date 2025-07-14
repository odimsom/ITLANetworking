using System.ComponentModel.DataAnnotations;

namespace ITLANetworking.Core.Application.Dtos.Battleship
{
    public class AttackDto
    {
        [Required]
        public int GameId { get; set; }

        [Required]
        public string AttackerId { get; set; } = string.Empty;

        [Required]
        [Range(0, 9)]
        public int X { get; set; }

        [Required]
        [Range(0, 9)]
        public int Y { get; set; }
    }
}

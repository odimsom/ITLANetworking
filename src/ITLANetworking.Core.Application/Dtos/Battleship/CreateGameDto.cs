using System.ComponentModel.DataAnnotations;

namespace ITLANetworking.Core.Application.Dtos.Battleship
{
    public class CreateGameDto
    {
        [Required]
        public string Player1Id { get; set; } = string.Empty;

        public string? Player2Id { get; set; }
    }
}

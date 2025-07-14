using System.ComponentModel.DataAnnotations;

namespace ITLANetworking.Core.Application.Dtos.Post
{
    public class UpdatePostDto
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [StringLength(500)]
        public string Content { get; set; } = string.Empty;

        public string? ImageUrl { get; set; }
        public string? VideoUrl { get; set; }
    }
}

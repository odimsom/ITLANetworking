using System.ComponentModel.DataAnnotations;

namespace ITLANetworking.Core.Application.Dtos.Comment
{
    public class SaveCommentDto
    {
        public int Id { get; set; }

        [Required]
        [StringLength(300)]
        public string Content { get; set; } = string.Empty;

        [Required]
        public int PostId { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty;
    }
}

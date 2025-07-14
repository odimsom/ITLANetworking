using System.ComponentModel.DataAnnotations;

namespace ITLANetworking.Core.Application.Dtos.Comment
{
    public class UpdateCommentDto
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [StringLength(300)]
        public string Content { get; set; } = string.Empty;
    }
}

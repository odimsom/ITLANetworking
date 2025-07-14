using System.ComponentModel.DataAnnotations;

namespace ITLANetworking.Core.Application.ViewModels.Comment
{
    public class SaveCommentViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El contenido del comentario es requerido")]
        [StringLength(500, ErrorMessage = "El comentario no puede exceder los 500 caracteres")]
        public string Content { get; set; } = string.Empty;

        [Required]
        public int PostId { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty;

        // Para respuestas a comentarios
        public int? ParentCommentId { get; set; }
    }
}

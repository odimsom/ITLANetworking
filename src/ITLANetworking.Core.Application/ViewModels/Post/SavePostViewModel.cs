using System.ComponentModel.DataAnnotations;

namespace ITLANetworking.Core.Application.ViewModels.Post
{
    public class SavePostViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El contenido de la publicación es requerido")]
        [StringLength(1000, ErrorMessage = "La publicación no puede exceder los 1000 caracteres")]
        public string Content { get; set; } = string.Empty;

        [Url(ErrorMessage = "La URL de la imagen no es válida")]
        public string? ImageUrl { get; set; }

        [Url(ErrorMessage = "La URL del video no es válida")]
        public string? VideoUrl { get; set; }

        public string UserId { get; set; } = string.Empty;

        // Propiedades de validación
        public bool HasContent => !string.IsNullOrWhiteSpace(Content);
        public bool HasMedia => !string.IsNullOrWhiteSpace(ImageUrl) || !string.IsNullOrWhiteSpace(VideoUrl);
        public bool IsValid => HasContent;

        // Métodos de limpieza
        public void Sanitize()
        {
            Content = Content?.Trim() ?? string.Empty;
            ImageUrl = string.IsNullOrWhiteSpace(ImageUrl) ? null : ImageUrl.Trim();
            VideoUrl = string.IsNullOrWhiteSpace(VideoUrl) ? null : VideoUrl.Trim();
        }
    }
}

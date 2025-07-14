using System.ComponentModel.DataAnnotations;

namespace ITLANetworking.Core.Application.ViewModels.Post
{
    public class SavePostViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El contenido de la publicaci�n es requerido")]
        [StringLength(1000, ErrorMessage = "La publicaci�n no puede exceder los 1000 caracteres")]
        public string Content { get; set; } = string.Empty;

        [Url(ErrorMessage = "La URL de la imagen no es v�lida")]
        public string? ImageUrl { get; set; }

        [Url(ErrorMessage = "La URL del video no es v�lida")]
        public string? VideoUrl { get; set; }

        public string UserId { get; set; } = string.Empty;

        // Propiedades de validaci�n
        public bool HasContent => !string.IsNullOrWhiteSpace(Content);
        public bool HasMedia => !string.IsNullOrWhiteSpace(ImageUrl) || !string.IsNullOrWhiteSpace(VideoUrl);
        public bool IsValid => HasContent;

        // M�todos de limpieza
        public void Sanitize()
        {
            Content = Content?.Trim() ?? string.Empty;
            ImageUrl = string.IsNullOrWhiteSpace(ImageUrl) ? null : ImageUrl.Trim();
            VideoUrl = string.IsNullOrWhiteSpace(VideoUrl) ? null : VideoUrl.Trim();
        }
    }
}

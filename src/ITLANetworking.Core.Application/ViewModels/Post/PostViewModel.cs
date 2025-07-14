using ITLANetworking.Core.Application.ViewModels.Comment;
using ITLANetworking.Core.Application.ViewModels.User;

namespace ITLANetworking.Core.Application.ViewModels.Post
{
    public class PostViewModel
    {
        public int Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public string? ImageUrl { get; set; }
        public string? VideoUrl { get; set; }
        public DateTime Created { get; set; }
        public DateTime? LastModified { get; set; }
        public string UserId { get; set; } = string.Empty;
        public UserViewModel User { get; set; } = null!;
        public List<CommentViewModel> Comments { get; set; } = new();
        public int ReactionsCount { get; set; }
        public string? UserReaction { get; set; }
        public bool HasUserReacted { get; set; }
        public string? UserReactionType { get; set; }

        // Para la vista principal
        public List<PostViewModel> Posts { get; set; } = new();
        public SavePostViewModel NewPost { get; set; } = new();

        // Propiedades calculadas
        public int CommentsCount => Comments?.Count ?? 0;
        public string TimeAgo => GetTimeAgo(Created);
        public bool CanEdit { get; set; }
        public bool CanDelete { get; set; }

        // Propiedades de media
        public bool HasImage => !string.IsNullOrEmpty(ImageUrl);
        public bool HasVideo => !string.IsNullOrEmpty(VideoUrl);
        public string? EmbedVideoUrl => GetEmbedUrl(VideoUrl);

        private static string GetTimeAgo(DateTime dateTime)
        {
            var timeSpan = DateTime.Now - dateTime;

            if (timeSpan.TotalMinutes < 1)
                return "Ahora mismo";
            if (timeSpan.TotalMinutes < 60)
                return $"Hace {(int)timeSpan.TotalMinutes} min";
            if (timeSpan.TotalHours < 24)
                return $"Hace {(int)timeSpan.TotalHours}h";
            if (timeSpan.TotalDays < 7)
                return $"Hace {(int)timeSpan.TotalDays}d";

            return dateTime.ToString("dd/MM/yyyy");
        }

        private static string? GetEmbedUrl(string? videoUrl)
        {
            if (string.IsNullOrEmpty(videoUrl))
                return null;

            if (videoUrl.Contains("youtube.com/watch?v="))
                return videoUrl.Replace("watch?v=", "embed/");
            if (videoUrl.Contains("youtu.be/"))
                return videoUrl.Replace("youtu.be/", "youtube.com/embed/");

            return videoUrl;
        }
    }
}

using ITLANetworking.Core.Application.ViewModels.User;

namespace ITLANetworking.Core.Application.ViewModels.Comment
{
    public class CommentViewModel
    {
        public int Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public int PostId { get; set; }
        public string UserId { get; set; } = string.Empty;
        public DateTime Created { get; set; }
        public DateTime? LastModified { get; set; }
        public UserViewModel User { get; set; } = null!;

        // Para comentarios anidados
        public int? ParentCommentId { get; set; }
        public List<CommentViewModel> Replies { get; set; } = new();
        public bool HasReplies => Replies?.Any() == true;
        public int RepliesCount => Replies?.Count ?? 0;

        // Propiedades calculadas
        public string TimeAgo => GetTimeAgo(Created);
        public bool CanEdit { get; set; }
        public bool CanDelete { get; set; }

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
    }
}

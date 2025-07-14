namespace ITLANetworking.Core.Application.ViewModels.Reaction
{
    public class ReactionViewModel
    {
        public int Id { get; set; }
        public int PostId { get; set; }
        public string UserId { get; set; } = string.Empty;
        public int ReactionType { get; set; }

        public DateTime Created { get; set; }

        // Propiedades de navegación
        public string? UserName { get; set; }
        public string? UserProfilePicture { get; set; }

        // Propiedades de conveniencia
        public string ReactionTypeName => GetReactionTypeName(ReactionType);
        public string ReactionIcon => GetReactionIcon(ReactionType);

        private static string GetReactionTypeName(int reactionType)
        {
            return reactionType switch
            {
                1 => "Like",
                2 => "Love",
                3 => "Laugh",
                4 => "Wow",
                5 => "Sad",
                6 => "Angry",
                _ => "Unknown"
            };
        }

        private static string GetReactionIcon(int reactionType)
        {
            return reactionType switch
            {
                1 => "👍",
                2 => "❤️",
                3 => "😂",
                4 => "😮",
                5 => "😢",
                6 => "😡",
                _ => "👍"
            };
        }
    }
}

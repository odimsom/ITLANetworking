using System;
using System.ComponentModel.DataAnnotations;

namespace ITLANetworking.Core.Application.Dtos.Friendship
{
    public class FriendshipRequestDto
    {
        public int Id { get; set; }
        [Required]
        public string RequesterId { get; set; } = string.Empty;
        [Required]
        public string AddresseeId { get; set; } = string.Empty;
        public string RequesterName { get; set; } = string.Empty;
        public string RequesterUsername { get; set; } = string.Empty;
        public string? RequesterProfilePicture { get; set; }
        public string? ReceiverName { get; set; } = string.Empty;
        public string? ReceiverUsername { get; set; } = string.Empty;
        public string? ReceiverProfilePicture { get; set; }
        public int? MutualFriendsCount { get; set; }
        public DateTime RequestDate { get; set; }
        public string TimeElapsed => GetTimeElapsed();

        private string GetTimeElapsed()
        {
            var elapsed = DateTime.UtcNow - RequestDate;

            if (elapsed.TotalDays > 30)
                return $"{(int)(elapsed.TotalDays / 30)} mes(es) atrás";
            if (elapsed.TotalDays >= 1)
                return $"{(int)elapsed.TotalDays} día(s) atrás";
            if (elapsed.TotalHours >= 1)
                return $"{(int)elapsed.TotalHours} hora(s) atrás";
            if (elapsed.TotalMinutes >= 1)
                return $"{(int)elapsed.TotalMinutes} minuto(s) atrás";

            return "Hace un momento";
        }
    }
}

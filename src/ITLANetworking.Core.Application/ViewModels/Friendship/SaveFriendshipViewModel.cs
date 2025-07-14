using System.ComponentModel.DataAnnotations;

namespace ITLANetworking.Core.Application.ViewModels.Friendship
{
    public class SaveFriendshipViewModel
    {
        [Required]
        public int ReceiverId { get; set; }

        public bool HasError { get; set; }
        public string? Error { get; set; }
    }
}

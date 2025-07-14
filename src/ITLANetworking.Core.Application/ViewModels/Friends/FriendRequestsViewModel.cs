using ITLANetworking.Core.Application.ViewModels.Friendship;

namespace ITLANetworking.Core.Application.ViewModels.Friends
{
    public class FriendRequestsViewModel
    {
        public List<FriendshipViewModel> PendingRequests { get; set; } = new();
        public List<FriendshipViewModel> SentRequests { get; set; } = new();
    }
}

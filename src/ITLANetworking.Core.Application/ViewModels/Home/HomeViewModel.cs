using ITLANetworking.Core.Application.ViewModels.Post;
using ITLANetworking.Core.Application.ViewModels.User;

namespace ITLANetworking.Core.Application.ViewModels.Home
{
    public class HomeViewModel
    {
        public List<PostViewModel> Posts { get; set; } = new();
        public List<UserViewModel> SuggestedFriends { get; set; } = new();
        public int PendingFriendRequests { get; set; }
        public SavePostViewModel NewPost { get; set; } = new();
    }
}

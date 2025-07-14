using ITLANetworking.Core.Application.ViewModels.User;

namespace ITLANetworking.Core.Application.ViewModels.Friends
{
    public class FriendsIndexViewModel
    {
        public List<UserViewModel> Friends { get; set; } = new();
        public List<UserViewModel> SuggestedFriends { get; set; } = new();
        public string SearchTerm { get; set; } = string.Empty;
    }
}

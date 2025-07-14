using ITLANetworking.Core.Application.Dtos.User;
using ITLANetworking.Core.Domain.Enums;

namespace ITLANetworking.Core.Application.Dtos.Friendship
{
    public class FriendshipDto
    {
        public int Id { get; set; }
        public string RequesterId { get; set; } = string.Empty;
        public string AddresseeId { get; set; } = string.Empty;
        public int MutualFriendsCount { get; set; } = 0;
        public FriendshipStatus Status { get; set; }
        public DateTime Created { get; set; }
        public UserDto Requester { get; set; } = null!;
        public UserDto Receiver { get; set; } = null!;
        public UserDto Addressee { get; set; } = null!;
    }
}

using ITLANetworking.Core.Application.ViewModels.User;
using ITLANetworking.Core.Domain.Enums;

namespace ITLANetworking.Core.Application.ViewModels.Friendship
{
    public class FriendshipViewModel
    {
        public int Id { get; set; }
        public UserViewModel Requester { get; set; } = new();
        public UserViewModel Receiver { get; set; } = new();
        public FriendshipStatus Status { get; set; }
        public DateTime Created { get; set; }
        public string StatusText => Status switch
        {
            FriendshipStatus.Pending => "Pendiente",
            FriendshipStatus.Accepted => "Aceptada",
            FriendshipStatus.Rejected => "Rechazada",
            FriendshipStatus.Blocked => "Bloqueada",
            _ => "Desconocido"
        };
    }
}

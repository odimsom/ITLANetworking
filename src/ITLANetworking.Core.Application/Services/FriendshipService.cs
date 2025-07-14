using AutoMapper;
using ITLANetworking.Core.Application.Dtos.Friendship;
using ITLANetworking.Core.Application.Dtos.User;
using ITLANetworking.Core.Application.Interfaces.Services;
using ITLANetworking.Core.Application.Services.Common;
using ITLANetworking.Core.Domain.Entities;
using ITLANetworking.Core.Domain.Enums;
using ITLANetworking.Core.Domain.Interfaces.Repositories;

namespace ITLANetworking.Core.Application.Services
{
    public class FriendshipService : GenericService<FriendshipDto, FriendshipDto, Friendship>, IFriendshipService
    {
        private readonly IFriendshipRepository _friendshipRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public FriendshipService(IFriendshipRepository friendshipRepository, IUserRepository userRepository, IMapper mapper)
            : base(friendshipRepository, mapper)
        {
            _friendshipRepository = friendshipRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task SendFriendRequestAsync(string requesterId, string addresseeId)
        {
            if (requesterId == addresseeId)
                throw new InvalidOperationException("No puedes enviarte una solicitud de amistad a ti mismo");

            if (await _friendshipRepository.AreFriendsAsync(requesterId, addresseeId))
                throw new InvalidOperationException("Ya son amigos");

            if (await _friendshipRepository.HasPendingRequestAsync(requesterId, addresseeId))
                throw new InvalidOperationException("Ya existe una solicitud pendiente");

            if (await _friendshipRepository.HasPendingRequestAsync(addresseeId, requesterId))
                throw new InvalidOperationException("Este usuario ya te envió una solicitud de amistad");

            var friendship = new Friendship
            {
                RequesterId = requesterId,
                ReceiverId = addresseeId,
                Status = FriendshipStatus.Pending,
                RequestDate = DateTime.UtcNow
            };

            await _friendshipRepository.AddAsync(friendship);
        }

        public async Task AcceptFriendRequestAsync(int friendshipId)
        {
            var friendship = await _friendshipRepository.GetByIdAsync(friendshipId);
            if (friendship == null)
                throw new InvalidOperationException("Solicitud de amistad no encontrada");

            if (friendship.Status != FriendshipStatus.Pending)
                throw new InvalidOperationException("La solicitud ya fue procesada");

            friendship.Status = FriendshipStatus.Accepted;
            friendship.ResponseDate = DateTime.UtcNow;

            await _friendshipRepository.UpdateAsync(friendship, friendshipId);
        }

        public async Task RejectFriendRequest(int friendshipId)
        {
            var friendship = await _friendshipRepository.GetByIdAsync(friendshipId);
            if (friendship == null)
                throw new InvalidOperationException("Solicitud de amistad no encontrada");

            if (friendship.Status != FriendshipStatus.Pending)
                throw new InvalidOperationException("La solicitud ya fue procesada");

            friendship.Status = FriendshipStatus.Rejected;
            friendship.ResponseDate = DateTime.UtcNow;

            await _friendshipRepository.UpdateAsync(friendship, friendshipId);
        }

        public async Task RemoveFriend(string userId1, string userId2)
        {
            var friendship = await _friendshipRepository.FindAsync(f =>
                ((f.RequesterId == userId1 && f.ReceiverId == userId2) ||
                 (f.RequesterId == userId2 && f.ReceiverId == userId1)) &&
                f.Status == FriendshipStatus.Accepted);

            var friendshipToRemove = friendship.FirstOrDefault();
            if (friendshipToRemove != null)
            {
                await _friendshipRepository.DeleteAsync(friendshipToRemove);
            }
        }

        public async Task<List<FriendshipDto>> GetPendingRequestsByUserIdAsync(string userId)
        {
            var friendships = await _friendshipRepository.GetPendingRequestsAsync(userId);
            var friendshipDtos = _mapper.Map<List<FriendshipDto>>(friendships);

            foreach (var dto in friendshipDtos)
            {
                dto.MutualFriendsCount = await _friendshipRepository.GetMutualFriendsCountAsync(userId, dto.RequesterId);
            }

            return friendshipDtos;
        }

        public async Task<List<FriendshipDto>> GetSentRequestsByUserIdAsync(string userId)
        {
            var friendships = await _friendshipRepository.GetSentRequestsAsync(userId);
            return _mapper.Map<List<FriendshipDto>>(friendships);
        }

        public async Task<List<FriendshipDto>> GetFriendsByUserIdAsync(string userId)
        {
            var friends = await _friendshipRepository.GetFriendsAsync(userId);

            var friendshipDtos = new List<FriendshipDto>();
            foreach (var friend in friends)
            {
                var friendships = await _friendshipRepository.FindAsync(f =>
                    ((f.RequesterId == userId && f.ReceiverId == friend.Id) ||
                     (f.RequesterId == friend.Id && f.ReceiverId == userId)) &&
                    f.Status == FriendshipStatus.Accepted);

                var friendship = friendships.FirstOrDefault();
                if (friendship != null)
                {
                    var dto = _mapper.Map<FriendshipDto>(friendship);
                    friendshipDtos.Add(dto);
                }
            }

            return friendshipDtos;
        }

        public async Task<bool> AreFriendsAsync(string userId1, string userId2)
        {
            return await _friendshipRepository.AreFriendsAsync(userId1, userId2);
        }

        public async Task<bool> HasPendingRequestAsync(string requesterId, string addresseeId)
        {
            return await _friendshipRepository.HasPendingRequestAsync(requesterId, addresseeId);
        }

        public async Task<List<UserDto>> GetAvailableUsers(string userId, string? searchTerm = null)
        {
            var allUsers = await _userRepository.GetActiveUsersAsync();
            var friends = await _friendshipRepository.GetFriendsAsync(userId);
            var friendIds = friends.Select(f => f.Id).ToList();

            var pendingRequests = await _friendshipRepository.FindAsync(f =>
                (f.RequesterId == userId || f.ReceiverId == userId) &&
                f.Status == FriendshipStatus.Pending);

            var pendingUserIds = pendingRequests
                .Select(f => f.RequesterId == userId ? f.ReceiverId : f.RequesterId)
                .ToList();

            var availableUsers = allUsers
                .Where(u => u.Id != userId &&
                           !friendIds.Contains(u.Id) &&
                           !pendingUserIds.Contains(u.Id))
                .ToList();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                availableUsers = availableUsers
                    .Where(u => u.FirstName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                               u.LastName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            return _mapper.Map<List<UserDto>>(availableUsers);
        }

        public async Task<List<FriendshipRequestDto>> GetPendingRequests(string userId)
        {
            var friendships = await _friendshipRepository.GetPendingRequestsAsync(userId);
            var requests = new List<FriendshipRequestDto>();

            foreach (var friendship in friendships)
            {
                var dto = _mapper.Map<FriendshipRequestDto>(friendship);

                dto.MutualFriendsCount = await _friendshipRepository.GetMutualFriendsCountAsync(userId, dto.RequesterId);

                requests.Add(dto);
            }

            return requests;
        }

        public async Task<List<FriendshipRequestDto>> GetSentRequests(string userId)
        {
            var friendships = await _friendshipRepository.GetSentRequestsAsync(userId);
            return _mapper.Map<List<FriendshipRequestDto>>(friendships);
        }
    }
}

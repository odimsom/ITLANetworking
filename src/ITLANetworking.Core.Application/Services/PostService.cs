using AutoMapper;
using ITLANetworking.Core.Application.Dtos.Post;
using ITLANetworking.Core.Application.Interfaces.Services;
using ITLANetworking.Core.Application.Services.Common;
using ITLANetworking.Core.Domain.Entities;
using ITLANetworking.Core.Domain.Enums;
using ITLANetworking.Core.Domain.Interfaces.Repositories;

namespace ITLANetworking.Core.Application.Services
{
    public class PostService : GenericService<SavePostDto, PostDto, Post>, IPostService
    {
        private readonly IPostRepository _postRepository;
        private readonly IPostReactionRepository _postReactionRepository;
        private readonly IMapper _mapper;

        public PostService(IPostRepository postRepository, IPostReactionRepository postReactionRepository, IMapper mapper)
            : base(postRepository, mapper)
        {
            _postRepository = postRepository;
            _postReactionRepository = postReactionRepository;
            _mapper = mapper;
        }

        public async Task<List<PostDto>> GetAllWithUserAsync()
        {
            var posts = await _postRepository.GetAllWithIncludeAsync(new List<string> { "User" });
            return _mapper.Map<List<PostDto>>(posts);
        }

        public async Task<List<PostDto>> GetByUserIdAsync(string userId)
        {
            var posts = await _postRepository.GetUserPostsAsync(userId);
            return _mapper.Map<List<PostDto>>(posts);
        }

        public async Task<List<PostDto>> GetFriendsPostsAsync(string userId)
        {
            var posts = await _postRepository.GetFriendsPostsAsync(userId);
            return _mapper.Map<List<PostDto>>(posts);
        }

        public async Task<List<PostDto>> GetFeedPostsAsync(List<string> friendIds, int page = 1, int pageSize = 10)
        {
            var posts = await _postRepository.GetFeedPostsAsync(friendIds, page, pageSize);
            return _mapper.Map<List<PostDto>>(posts);
        }

        public async Task<PostDto?> GetByIdWithDetailsAsync(int id)
        {
            var post = await _postRepository.GetPostWithDetailsAsync(id);
            return post == null ? null : _mapper.Map<PostDto>(post);
        }

        public async Task<int> GetPostCountByUserAsync(string userId)
        {
            return await _postRepository.GetPostCountByUserAsync(userId);
        }

        public async Task ReactToPostAsync(int postId, string userId, ReactionType reactionType)
        {
            var existing = await _postReactionRepository.GetUserReactionAsync(postId, userId);

            if (existing != null)
            {
                if (existing.Type == reactionType)
                {
                    await _postReactionRepository.DeleteAsync(existing);
                    return;
                }

                existing.Type = reactionType;
                await _postReactionRepository.UpdateAsync(existing, existing.Id);
                return;
            }

            var reaction = new PostReaction
            {
                PostId = postId,
                UserId = userId,
                Type = reactionType
            };
            await _postReactionRepository.AddAsync(reaction);
        }

        public async Task RemoveReactionAsync(int postId, string userId)
        {
            var existing = await _postReactionRepository.GetUserReactionAsync(postId, userId);
            if (existing != null)
            {
                await _postReactionRepository.DeleteAsync(existing);
            }
        }

        public async Task<List<PostDto>> GetRecentPostsAsync(int count = 20)
        {
            var posts = await _postRepository.GetRecentPostsAsync(count);
            return _mapper.Map<List<PostDto>>(posts);
        }

        public async Task<List<PostDto>> SearchPostsAsync(string searchText, int maxResults = 50)
        {
            var posts = await _postRepository.SearchPostsAsync(searchText, maxResults);
            return _mapper.Map<List<PostDto>>(posts);
        }

        public async Task<List<PostDto>> GetTrendingPostsAsync(int hoursWindow = 24, int maxResults = 20)
        {
            var posts = await _postRepository.GetTrendingPostsAsync(hoursWindow, maxResults);
            return _mapper.Map<List<PostDto>>(posts);
        }

        public async Task<Dictionary<ReactionType, int>> GetPostReactionStatsAsync(int postId)
        {
            var stats = new Dictionary<ReactionType, int>();

            foreach (ReactionType type in Enum.GetValues(typeof(ReactionType)))
            {
                stats[type] = 0;
            }

            var reactions = await _postReactionRepository.GetReactionsByPostIdAsync(postId);
            foreach (var reaction in reactions)
            {
                stats[reaction.Type]++;
            }

            return stats;
        }

        public async Task<List<PostDto>> GetAllPostsWithDetailsAsync()
        {
            var posts = await _postRepository.GetAllWithIncludeAsync(new List<string> { "User", "Comments", "Reactions" });
            return _mapper.Map<List<PostDto>>(posts);
        }
    }
}

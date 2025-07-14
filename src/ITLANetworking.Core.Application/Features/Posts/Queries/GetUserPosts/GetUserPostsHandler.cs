using AutoMapper;
using ITLANetworking.Core.Application.Dtos.Post;
using ITLANetworking.Core.Domain.Interfaces.Repositories;
using MediatR;

namespace ITLANetworking.Core.Application.Features.Posts.Queries.GetUserPosts
{
    public class GetUserPostsHandler : IRequestHandler<GetUserPostsQuery, List<PostDto>>
    {
        private readonly IPostRepository _postRepository;
        private readonly IMapper _mapper;

        public GetUserPostsHandler(IPostRepository postRepository, IMapper mapper)
        {
            _postRepository = postRepository;
            _mapper = mapper;
        }

        public async Task<List<PostDto>> Handle(GetUserPostsQuery request, CancellationToken cancellationToken)
        {
            var posts = await _postRepository.GetUserPostsAsync(request.UserId);
            return _mapper.Map<List<PostDto>>(posts);
        }
    }
}

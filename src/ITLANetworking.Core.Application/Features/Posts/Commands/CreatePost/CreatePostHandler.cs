using AutoMapper;
using ITLANetworking.Core.Domain.Entities;
using ITLANetworking.Core.Domain.Interfaces.Repositories;
using MediatR;

namespace ITLANetworking.Core.Application.Features.Posts.Commands.CreatePost
{
    public class CreatePostHandler : IRequestHandler<CreatePostCommand, CreatePostResponse>
    {
        private readonly IPostRepository _postRepository;
        private readonly IMapper _mapper;

        public CreatePostHandler(IPostRepository postRepository, IMapper mapper)
        {
            _postRepository = postRepository;
            _mapper = mapper;
        }

        public async Task<CreatePostResponse> Handle(CreatePostCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var post = new Post
                {
                    Content = request.Content,
                    ImageUrl = request.ImageUrl,
                    VideoUrl = request.VideoUrl,
                    UserId = request.UserId,
                    Created = DateTime.UtcNow
                };

                var createdPost = await _postRepository.AddAsync(post);

                return new CreatePostResponse
                {
                    Id = createdPost.Id,
                    Success = true,
                    Message = "Post creado exitosamente"
                };
            }
            catch (Exception ex)
            {
                return new CreatePostResponse
                {
                    Success = false,
                    Message = $"Error al crear el post: {ex.Message}"
                };
            }
        }
    }
}

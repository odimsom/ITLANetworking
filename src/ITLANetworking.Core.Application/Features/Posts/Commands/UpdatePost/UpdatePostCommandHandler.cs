namespace ITLANetworking.Core.Application.Features.Posts.Commands.UpdatePost
{
    using ITLANetworking.Core.Domain.Interfaces.Repositories;
    using MediatR;

    public class UpdatePostCommandHandler : IRequestHandler<UpdatePostCommand, bool>
    {
        private readonly IPostRepository _postRepository;

        public UpdatePostCommandHandler(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }

        public async Task<bool> Handle(UpdatePostCommand request, CancellationToken cancellationToken)
        {
            var post = await _postRepository.GetByIdAsync(request.Id);
            if (post == null || post.UserId != request.UserId)
                return false;

            post.Content = request.Content.Trim();
            post.ImageUrl = string.IsNullOrWhiteSpace(request.ImageUrl) ? null : request.ImageUrl.Trim();
            post.VideoUrl = string.IsNullOrWhiteSpace(request.VideoUrl) ? null : request.VideoUrl.Trim();
            post.LastModified = DateTime.UtcNow;

            await _postRepository.UpdateAsync(post, post.Id);
            return true;
        }
    }

}

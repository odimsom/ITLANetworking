namespace ITLANetworking.Core.Application.Features.Posts.Commands.DeletePost
{
    using ITLANetworking.Core.Application.Interfaces.Services;
    using ITLANetworking.Core.Domain.Interfaces.Repositories;
    using MediatR;

    public class DeletePostCommandHandler : IRequestHandler<DeletePostCommand, bool>
    {
        private readonly IPostService _postService;

        public DeletePostCommandHandler(IPostService postservice)
        {
            _postService = postservice;
        }

        public async Task<bool> Handle(DeletePostCommand request, CancellationToken cancellationToken)
        {
            var post = await _postService.GetByIdAsync(request.Id);
            if (post == null || post.UserId != request.UserId)
                return false;

            await _postService.Delete(request.Id);
            return true;
        }
    }

}

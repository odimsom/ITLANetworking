using ITLANetworking.Core.Application.Interfaces.Services;
using ITLANetworking.Core.Domain.Enums;
using MediatR;

namespace ITLANetworking.Core.Application.Features.Posts.Commands.ReactToPost
{
    public class ReactToPostHandler : IRequestHandler<ReactToPostCommand, ReactToPostResponse>
    {
        private readonly IPostService _postService;

        public ReactToPostHandler(IPostService postService)
        {
            _postService = postService;
        }

        public async Task<ReactToPostResponse> Handle(ReactToPostCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if(request.ReactionType < 1 || request.ReactionType > 6)
                {
                    return new ReactToPostResponse
                    {
                        Success = false,
                        Message = "Tipo de reacción inválido. Debe ser un valor entre 1 y 6."
                    };
                }
                await _postService.ReactToPostAsync(request.PostId, request.UserId, (ReactionType)request.ReactionType);

                return new ReactToPostResponse
                {
                    Success = true,
                    Message = "Reacción registrada exitosamente"
                };
            }
            catch (Exception ex)
            {
                return new ReactToPostResponse
                {
                    Success = false,
                    Message = $"Error al procesar la reacción: {ex.Message}"
                };
            }
        }
    }
}

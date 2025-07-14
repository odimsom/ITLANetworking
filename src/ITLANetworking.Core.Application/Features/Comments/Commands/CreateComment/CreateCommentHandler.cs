using AutoMapper;
using ITLANetworking.Core.Domain.Entities;
using MediatR;
using ITLANetworking.Core.Domain.Interfaces.Repositories;

namespace ITLANetworking.Core.Application.Features.Comments.Commands.CreateComment
{
    public class CreateCommentHandler : IRequestHandler<CreateCommentCommand, CreateCommentResponse>
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IMapper _mapper;

        public CreateCommentHandler(ICommentRepository commentRepository, IMapper mapper)
        {
            _commentRepository = commentRepository;
            _mapper = mapper;
        }

        public async Task<CreateCommentResponse> Handle(CreateCommentCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var comment = new Comment
                {
                    Content = request.Content,
                    UserId = request.UserId,
                    PostId = request.PostId,
                    ParentCommentId = request.ParentCommentId,
                    Created = DateTime.UtcNow
                };

                var createdComment = await _commentRepository.AddAsync(comment);

                return new CreateCommentResponse
                {
                    Id = createdComment.Id,
                    Success = true,
                    Message = "Comentario creado exitosamente"
                };
            }
            catch (Exception ex)
            {
                return new CreateCommentResponse
                {
                    Success = false,
                    Message = $"Error al crear el comentario: {ex.Message}"
                };
            }
        }
    }
}

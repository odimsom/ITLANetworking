using AutoMapper;
using ITLANetworking.Core.Application.Dtos.Comment;
using ITLANetworking.Core.Application.Interfaces.Services;
using ITLANetworking.Core.Application.Services.Common;
using ITLANetworking.Core.Domain.Entities;
using ITLANetworking.Core.Domain.Interfaces.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ITLANetworking.Core.Application.Services
{
    public class CommentService : GenericService<SaveCommentDto, CommentDto, Comment>, ICommentService
    {
        private readonly ICommentRepository _commentRepository;
        private readonly new IMapper _mapper;

        public CommentService(ICommentRepository commentRepository, IMapper mapper)
            : base(commentRepository, mapper)
        {
            _commentRepository = commentRepository;
            _mapper = mapper;
        }

        public async Task<List<CommentDto>> GetByPostIdAsync(int postId)
        {
            var comments = await _commentRepository.GetCommentsByPostIdAsync(postId);
            return _mapper.Map<List<CommentDto>>(comments);
        }
        public async Task<List<CommentDto>> GetByPostIdWithUserAsync(int postId)
        {
            var comments = await _commentRepository.GetCommentsByPostIdAsync(postId);

            var commentDtos = _mapper.Map<List<CommentDto>>(comments);

            return commentDtos;
        }
    }
}

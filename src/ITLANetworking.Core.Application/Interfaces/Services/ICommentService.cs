using ITLANetworking.Core.Application.Dtos.Comment;
using ITLANetworking.Core.Application.Interfaces.Services.Common;
using ITLANetworking.Core.Application.ViewModels.Comment;
using ITLANetworking.Core.Domain.Entities;

namespace ITLANetworking.Core.Application.Interfaces.Services
{
    public interface ICommentService : IGenericService<SaveCommentDto, CommentDto, Comment>
    {
        Task<List<CommentDto>> GetByPostIdAsync(int postId);
        Task<List<CommentDto>> GetByPostIdWithUserAsync(int postId);
    }
}

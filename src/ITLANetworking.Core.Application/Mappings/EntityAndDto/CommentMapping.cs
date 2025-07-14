using AutoMapper;
using ITLANetworking.Core.Application.Dtos.Comment;
using ITLANetworking.Core.Application.ViewModels.Comment;
using ITLANetworking.Core.Domain.Entities;

namespace ITLANetworking.Core.Application.Mappings.EntityAndDto
{
    public class CommentMapping : Profile
    {
        public CommentMapping()
        {
            CreateMap<Comment, CommentDto>()
                .ReverseMap();

            CreateMap<Comment, SaveCommentDto>()
                .ReverseMap();

            CreateMap<Comment, UpdateCommentDto>()
                .ReverseMap();

            CreateMap<CommentDto, CommentViewModel>()
                .ForMember(dest => dest.CanEdit, opt => opt.Ignore())
                .ForMember(dest => dest.CanDelete, opt => opt.Ignore());

            CreateMap<CommentViewModel, CommentDto>()
                .ForMember(dest => dest.PostId, opt => opt.Ignore())
                .ForMember(dest => dest.UserId, opt => opt.Ignore())
                .ForMember(dest => dest.LastModified, opt => opt.Ignore());
        }
    }
}

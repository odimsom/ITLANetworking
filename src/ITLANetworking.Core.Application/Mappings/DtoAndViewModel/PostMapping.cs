using AutoMapper;
using ITLANetworking.Core.Application.Dtos.Post;
using ITLANetworking.Core.Application.Dtos.Comment;
using ITLANetworking.Core.Application.Dtos.Reaction;
using ITLANetworking.Core.Application.ViewModels.Post;
using ITLANetworking.Core.Application.ViewModels.Comment;
using ITLANetworking.Core.Application.ViewModels.Reaction;
using ITLANetworking.Core.Domain.Entities;

namespace ITLANetworking.Core.Application.Mappings
{
    public class PostMappingProfile : Profile
    {
        public PostMappingProfile()
        {
            #region Post Mappings

            // Entity to DTO
            CreateMap<Post, PostDto>()
                .ForMember(dest => dest.ReactionStats, opt => opt.MapFrom(src => src.Reactions))
                .ForMember(dest => dest.UserReaction, opt => opt.Ignore());

            // DTO to ViewModel
            CreateMap<PostDto, PostViewModel>()
                .ForMember(dest => dest.Posts, opt => opt.Ignore())
                .ForMember(dest => dest.NewPost, opt => opt.Ignore())
                .ForMember(dest => dest.HasUserReacted, opt => opt.MapFrom(src => !string.IsNullOrEmpty(src.UserReaction)))
                .ForMember(dest => dest.UserReactionType, opt => opt.MapFrom(src => src.UserReaction))
                .ForMember(dest => dest.CanEdit, opt => opt.Ignore())
                .ForMember(dest => dest.CanDelete, opt => opt.Ignore());

            // ViewModel to DTO (for updates)
            CreateMap<PostViewModel, PostDto>()
                .ForMember(dest => dest.ReactionStats, opt => opt.Ignore())
                .ForMember(dest => dest.UserReaction, opt => opt.Ignore());

            // SaveViewModel to Entity
            CreateMap<SavePostViewModel, Post>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Created, opt => opt.Ignore())
                .ForMember(dest => dest.LastModified, opt => opt.Ignore())
                .ForMember(dest => dest.User, opt => opt.Ignore())
                .ForMember(dest => dest.Comments, opt => opt.Ignore())
                .ForMember(dest => dest.Reactions, opt => opt.Ignore());

            #endregion

            #region Comment Mappings

            // Entity to DTO
            CreateMap<Comment, CommentDto>()
                .ForMember(dest => dest.Replies, opt => opt.MapFrom(src => src.Replies ?? new List<Comment>()));

            // DTO to ViewModel
            CreateMap<CommentDto, CommentViewModel>()
                .ForMember(dest => dest.CanEdit, opt => opt.Ignore())
                .ForMember(dest => dest.CanDelete, opt => opt.Ignore());

            // ViewModel to DTO
            CreateMap<CommentViewModel, CommentDto>();

            #endregion

            #region Reaction Mappings

            // Entity to DTO
            CreateMap<PostReaction, ReactionDto>();

            // DTO to ViewModel
            CreateMap<ReactionDto, ReactionViewModel>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User != null ? $"{src.User.FirstName} {src.User.LastName}" : ""))
                .ForMember(dest => dest.UserProfilePicture, opt => opt.MapFrom(src => src.User != null ? src.User.ProfilePicture : ""));

            // ViewModel to DTO
            CreateMap<ReactionViewModel, ReactionDto>();

            #endregion
        }
    }
}

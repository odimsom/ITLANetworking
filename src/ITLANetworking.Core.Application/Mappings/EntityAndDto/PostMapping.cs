using AutoMapper;
using ITLANetworking.Core.Application.Dtos.Post;
using ITLANetworking.Core.Domain.Entities;

namespace ITLANetworking.Core.Application.Mappings.EntityAndDto
{
    public class PostMapping : Profile
    {
        public PostMapping()
        {
            CreateMap<Post, PostDto>()
                .ReverseMap();

            CreateMap<Post, SavePostDto>()
                .ReverseMap();

            CreateMap<Post, UpdatePostDto>()
                .ReverseMap();
        }
    }
}

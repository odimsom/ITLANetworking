using ITLANetworking.Core.Application.Dtos.Post;
using MediatR;

namespace ITLANetworking.Core.Application.Features.Posts.Queries.GetUserPosts
{
    public class GetUserPostsQuery : IRequest<List<PostDto>>
    {
        public string UserId { get; set; } = string.Empty;
    }
}

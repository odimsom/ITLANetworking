using FluentValidation;

namespace ITLANetworking.Core.Application.Features.Posts.Queries.GetUserPosts
{
    public class GetUserPostsValidator : AbstractValidator<GetUserPostsQuery>
    {
        public GetUserPostsValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty()
                .WithMessage("El ID del usuario es requerido");
        }
    }
}

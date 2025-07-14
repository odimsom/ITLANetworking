using FluentValidation;

namespace ITLANetworking.Core.Application.Features.Comments.Commands.CreateComment
{
    public class CreateCommentValidator : AbstractValidator<CreateCommentCommand>
    {
        public CreateCommentValidator()
        {
            RuleFor(x => x.Content)
                .NotEmpty()
                .WithMessage("El contenido es requerido")
                .MaximumLength(300)
                .WithMessage("El comentario no puede exceder 300 caracteres");

            RuleFor(x => x.UserId)
                .NotEmpty()
                .WithMessage("El ID del usuario es requerido");

            RuleFor(x => x.PostId)
                .GreaterThan(0)
                .WithMessage("El ID del post es requerido");
        }
    }
}

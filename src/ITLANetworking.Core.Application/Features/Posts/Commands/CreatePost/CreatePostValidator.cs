using FluentValidation;

namespace ITLANetworking.Core.Application.Features.Posts.Commands.CreatePost
{
    public class CreatePostValidator : AbstractValidator<CreatePostCommand>
    {
        public CreatePostValidator()
        {
            RuleFor(x => x.Content)
                .NotEmpty()
                .WithMessage("El contenido es requerido")
                .MaximumLength(500)
                .WithMessage("El contenido no puede exceder 500 caracteres");

            RuleFor(x => x.UserId)
                .NotEmpty()
                .WithMessage("El ID del usuario es requerido");

            RuleFor(x => x.VideoUrl)
                .Must(BeValidYouTubeUrl)
                .When(x => !string.IsNullOrEmpty(x.VideoUrl))
                .WithMessage("Debe ser una URL válida de YouTube");
        }

        private bool BeValidYouTubeUrl(string? url)
        {
            if (string.IsNullOrEmpty(url)) return true;
            
            return url.Contains("youtube.com/watch?v=") || url.Contains("youtu.be/");
        }
    }
}

using FluentValidation;

namespace ITLANetworking.Core.Application.Features.Users.Commands.RegisterUser
{
    public class RegisterUserValidator : AbstractValidator<RegisterUserCommand>
    {
        public RegisterUserValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty()
                .WithMessage("El nombre es requerido")
                .MaximumLength(50)
                .WithMessage("El nombre no puede exceder 50 caracteres");

            RuleFor(x => x.LastName)
                .NotEmpty()
                .WithMessage("El apellido es requerido")
                .MaximumLength(50)
                .WithMessage("El apellido no puede exceder 50 caracteres");

            RuleFor(x => x.UserName)
                .NotEmpty()
                .WithMessage("El nombre de usuario es requerido")
                .MinimumLength(3)
                .WithMessage("El nombre de usuario debe tener al menos 3 caracteres")
                .MaximumLength(50)
                .WithMessage("El nombre de usuario no puede exceder 50 caracteres");

            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("El email es requerido")
                .EmailAddress()
                .WithMessage("Debe ser un email válido");

            RuleFor(x => x.Phone)
                .NotEmpty()
                .WithMessage("El teléfono es requerido")
                .Matches(@"^(\+1|1)?[-.\s]?([809|829|849]{3})[-.\s]?[0-9]{3}[-.\s]?[0-9]{4}$")
                .WithMessage("El formato del teléfono no es válido para República Dominicana");

            RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage("La contraseña es requerida")
                .MinimumLength(6)
                .WithMessage("La contraseña debe tener al menos 6 caracteres");

            RuleFor(x => x.ConfirmPassword)
                .NotEmpty()
                .WithMessage("Confirmar contraseña es requerido")
                .Equal(x => x.Password)
                .WithMessage("Las contraseñas no coinciden");

            RuleFor(x => x.Origin)
                .NotEmpty()
                .WithMessage("El origen es requerido");
        }
    }
}

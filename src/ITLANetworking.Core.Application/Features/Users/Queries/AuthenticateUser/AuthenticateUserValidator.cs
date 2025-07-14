using FluentValidation;

namespace ITLANetworking.Core.Application.Features.Users.Queries.AuthenticateUser
{
    public class AuthenticateUserValidator : AbstractValidator<AuthenticateUserQuery>
    {
        public AuthenticateUserValidator()
        {
            RuleFor(x => x.UserName)
                .NotEmpty()
                .WithMessage("El nombre de usuario es requerido");

            RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage("La contraseña es requerida");
        }
    }
}

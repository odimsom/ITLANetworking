using FluentValidation;

namespace ITLANetworking.Core.Application.Features.Friends.Commands.SendFriendRequest
{
    public class SendFriendRequestValidator : AbstractValidator<SendFriendRequestCommand>
    {
        public SendFriendRequestValidator()
        {
            RuleFor(x => x.RequesterId)
                .NotEmpty()
                .WithMessage("El ID del solicitante es requerido");

            RuleFor(x => x.ReceiverId)
                .NotEmpty()
                .WithMessage("El ID del receptor es requerido");

            RuleFor(x => x)
                .Must(x => x.RequesterId != x.ReceiverId)
                .WithMessage("No puedes enviarte una solicitud a ti mismo");
        }
    }
}

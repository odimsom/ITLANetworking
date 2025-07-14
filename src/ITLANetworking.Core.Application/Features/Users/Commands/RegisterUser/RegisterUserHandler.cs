using ITLANetworking.Core.Application.Interfaces.Services;
using ITLANetworking.Core.Application.Dtos.Account;
using MediatR;
using AutoMapper;

namespace ITLANetworking.Core.Application.Features.Users.Commands.RegisterUser
{
    public class RegisterUserHandler : IRequestHandler<RegisterUserCommand, RegisterUserResponse>
    {
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;

        public RegisterUserHandler(IAccountService accountService, IMapper mapper)
        {
            _accountService = accountService;
            _mapper = mapper;
        }

        public async Task<RegisterUserResponse> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var registerRequest = _mapper.Map<RegisterRequest>(request);
                var response = await _accountService.RegisterBasicUserAsync(registerRequest, request.Origin);

                return new RegisterUserResponse
                {
                    UserId = response.UserId ?? string.Empty,
                    Success = !response.HasError,
                    Message = response.HasError ? response.Error! : "Usuario registrado exitosamente. Revisa tu email para activar tu cuenta."
                };
            }
            catch (Exception ex)
            {
                return new RegisterUserResponse
                {
                    Success = false,
                    Message = $"Error al registrar usuario: {ex.Message}"
                };
            }
        }
    }
}

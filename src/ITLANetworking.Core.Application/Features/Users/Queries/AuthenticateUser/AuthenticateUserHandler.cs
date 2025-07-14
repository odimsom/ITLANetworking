using AutoMapper;
using ITLANetworking.Core.Application.Dtos.Account;
using ITLANetworking.Core.Application.Interfaces.Services;
using MediatR;

namespace ITLANetworking.Core.Application.Features.Users.Queries.AuthenticateUser
{
    public class AuthenticateUserHandler : IRequestHandler<AuthenticateUserQuery, AuthenticationResponse>
    {
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;

        public AuthenticateUserHandler(IAccountService accountService, IMapper mapper)
        {
            _accountService = accountService;
            _mapper = mapper;
        }

        public async Task<AuthenticationResponse> Handle(AuthenticateUserQuery request, CancellationToken cancellationToken)
        {
            var authRequest = _mapper.Map<AuthenticationRequest>(request);
            return await _accountService.AuthenticateAsync(authRequest);
        }
    }
}

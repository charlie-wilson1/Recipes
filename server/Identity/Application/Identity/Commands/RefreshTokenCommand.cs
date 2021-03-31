using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Recipes.Identity.Application.Contracts.Repositories;
using Recipes.Identity.Application.Contracts.Services;
using Recipes.Identity.Application.Identity.Responses;

namespace Recipes.Identity.Application.Identity.Commands
{
    public class RefreshTokenCommand : IRequest<JwtAuthResponse>
    {
        public string Username { get; set; }
        public string RefreshToken { get; set; }

        public class Handler : IRequestHandler<RefreshTokenCommand, JwtAuthResponse>
        {
            private readonly IUserRepository _userRepository;
            private readonly IUserService _userService;
            private readonly IJwtService _jwtService;

            public Handler(
                IUserRepository userRepository,
                IUserService userService,
                IJwtService jwtService)
            {
                _userRepository = userRepository;
                _userService = userService;
                _jwtService = jwtService;
            }

            public async Task<JwtAuthResponse> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
            {
                var user = await _userRepository.GetByUsernameAsync(request.Username, cancellationToken);
                _userService.ValidateRefreshToken(user, request.RefreshToken);
                var token = _jwtService.GenerateJwtTokens(user);
                user.UpdateRefreshToken(token.RefreshToken);
                await _userRepository.UpdateAsync(user, cancellationToken);
                return token;
            }
        }
    }
}

using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Recipes.Identity.Application.Contracts.Repositories;
using Recipes.Identity.Application.Contracts.Services;
using Recipes.Identity.Application.Identity.Responses;

namespace Recipes.Identity.Application.Identity.Commands
{
    public class AuthenticateCommand : IRequest<JwtAuthResponse>
    {
        public string Username { get; set; }
        public string Password { get; set; }

        public class Handler : IRequestHandler<AuthenticateCommand, JwtAuthResponse>
        {
            private readonly IUserRepository _userRepository;
            private readonly IPasswordService _passwordService;
            private readonly IJwtService _jwtService;

            public Handler(IUserRepository userRepository, IPasswordService passwordService, IJwtService jwtService)
            {
                _userRepository = userRepository;
                _passwordService = passwordService;
                _jwtService = jwtService;
            }

            public async Task<JwtAuthResponse> Handle(AuthenticateCommand request, CancellationToken cancellationToken)
            {
                var user = await _userRepository.GetByUsernameAsync(request.Username, cancellationToken);
                _passwordService.ValidatePasswordHash(request.Password, user.PasswordHash, user.PasswordSalt);
                var token = _jwtService.GenerateJwtTokens(user);
                user.UpdateRefreshToken(token.RefreshToken);
                await _userRepository.UpdateAsync(user, cancellationToken);
                return token;
            }
        }
    }
}

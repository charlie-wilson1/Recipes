using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Recipes.Identity.Application.Contracts.Repositories;
using Recipes.Identity.Application.Contracts.Services;
using Recipes.Identity.Application.Identity.Responses;
using Recipes.Identity.Domain;

namespace Recipes.Identity.Application.Identity.Commands
{
    public class UpdateCurrentUserCommand : IRequest<UserWithTokenResponse>
    {
        public string Username { get; set; }
        public string Email { get; set; }

        public class Handler : IRequestHandler<UpdateCurrentUserCommand, UserWithTokenResponse>
        {
            private readonly IUserRepository _userRepository;
            private readonly IUserService _userServcie;
            private readonly IJwtService _jwtService;
            private readonly ICurrentUserService _currentUserService;

            public Handler(IUserRepository userRepository, IUserService userServcie, IJwtService jwtService, ICurrentUserService currentUserService)
            {
                _userRepository = userRepository;
                _userServcie = userServcie;
                _jwtService = jwtService;
                _currentUserService = currentUserService;
            }

            public async Task<UserWithTokenResponse> Handle(UpdateCurrentUserCommand request, CancellationToken cancellationToken)
            {
                var user = await _userRepository.GetByIdAsync(_currentUserService.UserId, cancellationToken);
                await ValidateNewEmailUsername(user, request, cancellationToken);
                var response = await UpdateUser(user, request, cancellationToken);
                return response;
            }

            public async Task ValidateNewEmailUsername(ApplicationUser user, UpdateCurrentUserCommand request, CancellationToken cancellationToken)
            {
                if (!user.EmailMatches(request.Email))
                {
                    await _userServcie.ValidateNewEmail(request.Email, cancellationToken);
                }

                if (!user.UsernameMatches(request.Username))
                {
                    await _userServcie.ValidateNewUsername(request.Username, cancellationToken);
                }
            }

            public async Task<UserWithTokenResponse> UpdateUser(ApplicationUser user, UpdateCurrentUserCommand request, CancellationToken cancellationToken)
            {
                user.UpdateEmailAndUsername(request.Username, request.Email);
                var token = _jwtService.GenerateRandomToken();
                user.UpdateRefreshToken(token);
                await _userRepository.UpdateAsync(user, cancellationToken);
                return _userServcie.CreateUserWithTokenResponse(user);
            }
        }
    }
}

using MediatR;
using Recipes.Application.Contracts.Identity;
using System.Threading;
using System.Threading.Tasks;

namespace Recipes.Application.Dtos.Identity.Commands
{
    public class UpdateCurrentUserCommand : IRequest
    {
        public string Username { get; set; }
        public string Email { get; set; }

        public class Handler : IRequestHandler<UpdateCurrentUserCommand>
        {
            private readonly IIdentityService _identityService;
            private readonly ICurrentUserService _currentUserService;

            public Handler(IIdentityService identityService, ICurrentUserService currentUserService)
            {
                _identityService = identityService;
                _currentUserService = currentUserService;
            }

            public async Task<Unit> Handle(UpdateCurrentUserCommand request, CancellationToken cancellationToken)
            {
                var currentUsername = _currentUserService.Username;
                var user = await _identityService.FindUserByUsernameAsync(currentUsername);

                if (!string.IsNullOrEmpty(request.Username))
                {
                    await _identityService.UpdateUsernameAsync(user, request.Username);
                }

                if (!string.IsNullOrEmpty(request.Email))
                {
                    await _identityService.UpdateEmailAsync(user, request.Email);
                }

                return Unit.Value;
            }
        }
    }
}

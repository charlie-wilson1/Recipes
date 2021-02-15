using MediatR;
using Recipes.Application.Contracts.Identity;
using System.Threading;
using System.Threading.Tasks;

namespace Recipes.Application.Dtos.Identity.Commands
{
    public class UpdatePasswordCommand : IRequest
    {
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
        public string NewPasswordConfirmation { get; set; }

        public class Handler : IRequestHandler<UpdatePasswordCommand>
        {
            private readonly IIdentityService _identityService;
            private readonly IPasswordService _passwordService;
            private readonly ICurrentUserService _currentUserService;

            public Handler(
                IIdentityService identityService,
                IPasswordService passwordService,
                ICurrentUserService currentUserService)
            {
                _identityService = identityService;
                _passwordService = passwordService;
                _currentUserService = currentUserService;
            }

            public async Task<Unit> Handle(UpdatePasswordCommand request, CancellationToken cancellationToken)
            {
                var currentUsername = _currentUserService.Username;
                var user = await _identityService.FindUserByUsernameAsync(currentUsername);
                await _passwordService.ValidatePasswordAsync(user, request.NewPassword);
                await _passwordService.UpdatePasswordAsync(user, request.CurrentPassword, request.NewPassword);
                return Unit.Value;
            }
        }
    }
}

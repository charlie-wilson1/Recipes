using MediatR;
using Recipes.Identity.Application.Contracts.Repositories;
using Recipes.Identity.Application.Contracts.Services;
using Recipes.Identity.Domain;
using System.Threading;
using System.Threading.Tasks;

namespace Recipes.Identity.Application.Identity.Commands
{
    public class ResetPasswordCommand : IRequest
    {
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
        public string NewPasswordConfirmation { get; set; }

        public class Handler : IRequestHandler<ResetPasswordCommand>
        {
            private readonly IUserRepository _userRepository;
            private readonly IPasswordService _passwordService;
            private readonly ICurrentUserService _currentUserService;

            public Handler(
                IUserRepository userRepository,
                IPasswordService passwordService,
                ICurrentUserService currentUserService)
            {
                _userRepository = userRepository;
                _passwordService = passwordService;
                _currentUserService = currentUserService;
            }

            public async Task<Unit> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
            {
                var user = await _userRepository.GetByIdAsync(_currentUserService.UserId, cancellationToken);
                _passwordService.ValidatePasswordHash(request.CurrentPassword, user.PasswordHash, user.PasswordSalt);
                await Update(user, request.NewPassword, cancellationToken);
                return Unit.Value;
            }

            private async Task Update(ApplicationUser user, string newPassword, CancellationToken cancellationToken)
            {
                _passwordService.CreateHash(newPassword, out byte[] hash, out byte[] salt);
                user.UpdatePassword(hash, salt);
                await _userRepository.UpdateAsync(user, cancellationToken);
            }
        }
    }
}

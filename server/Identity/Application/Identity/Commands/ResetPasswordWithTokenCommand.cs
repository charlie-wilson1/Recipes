using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Recipes.Identity.Application.Contracts.Services;
using Recipes.Identity.Domain;

namespace Recipes.Identity.Application.Identity.Commands
{
    public class ResetPasswordWithTokenCommand : IRequest
    {
        public string Email { get; set; }
        public string Token { get; set; }
        public string Password { get; set; }
        public string PasswordConfirm { get; set; }

        public class Handler : IRequestHandler<ResetPasswordWithTokenCommand>
        {
            private readonly IUserService _userService;
            private readonly IPasswordService _passwordService;

            public Handler(IUserService userService, IPasswordService passwordService)
            {
                _userService = userService;
                _passwordService = passwordService;
            }

            public async Task<Unit> Handle(ResetPasswordWithTokenCommand request, CancellationToken cancellationToken)
            {
                var user = await _userService.GetByEmail(request.Email, cancellationToken);
                user.IsRefreshTokenValid(request.Token);
                await _passwordService.UpdateAsync(user, request.Password, cancellationToken);
                return Unit.Value;
            }
        }
    }
}

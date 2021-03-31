using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Recipes.Identity.Application.Contracts.Repositories;
using Recipes.Identity.Application.Contracts.Services;
using Recipes.Identity.Domain;

namespace Recipes.Identity.Application.Identity.Commands
{
    public class SendResetPasswordEmailCommand : IRequest
    {
        public string Email { get; set; }

        public class Handler : IRequestHandler<SendResetPasswordEmailCommand>
        {
            private readonly IUserRepository _userRepository;
            private readonly IJwtService _jwtService;
            private readonly IEmailService _emailService;

            public Handler(IUserRepository userRepository, IJwtService jwtService, IEmailService emailService)
            {
                _userRepository = userRepository;
                _jwtService = jwtService;
                _emailService = emailService;
            }

            public async Task<Unit> Handle(SendResetPasswordEmailCommand request, CancellationToken cancellationToken)
            {
                var user = await UpdateUserToken(request.Email, cancellationToken);
                await SendEmail(user, cancellationToken);
                return Unit.Value;
            }

            public async Task<ApplicationUser> UpdateUserToken(string email, CancellationToken cancellationToken)
            {
                var user = await _userRepository.GetByEmailAsync(email, cancellationToken);
                var token = _jwtService.GenerateRandomToken();
                user.RequestPasswordReset(token);
                await _userRepository.UpdateAsync(user, cancellationToken);
                return user;
            }

            public async Task SendEmail(ApplicationUser user, CancellationToken cancellationToken)
            {
                var dto = _emailService.CreateTokenizedEmailDto(user.Email, user.RefreshToken, "register");
                await _emailService.SendResetPasswordEmail(dto, cancellationToken);
            }
        }
    }
}

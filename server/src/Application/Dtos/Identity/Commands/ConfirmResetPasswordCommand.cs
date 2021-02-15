using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Recipes.Application.Contracts.Identity;
using Recipes.Application.Contracts.Notifications.SendGrid;
using Recipes.Application.Dtos.Notifications.SendGrid;

namespace Recipes.Application.Dtos.Identity.Commands
{
    public class ConfirmResetPasswordCommand : IRequest
    {
        public string Email { get; set; }
        public string ResetToken { get; set; }
        public string NewPassword { get; set; }
        public string NewPasswordConfirmation { get; set; }

        public class Handler : IRequestHandler<ConfirmResetPasswordCommand>
        {
            private readonly IIdentityService _identityService;
            private readonly IEmailService _emailService;
            private readonly IPasswordService _passwordService;

            public Handler(
                IIdentityService identityService,
                IEmailService emailService,
                IPasswordService passwordService)
            {
                _identityService = identityService;
                _emailService = emailService;
                _passwordService = passwordService;
            }
            public async Task<Unit> Handle(ConfirmResetPasswordCommand request, CancellationToken cancellationToken)
            {
                var user = await _identityService.FindUserByEmailAsync(request.Email);
                await _passwordService.ResetPasswordAsync(user, request.ResetToken, request.NewPassword);

                // send email when SendGrid service completed.
                var clientUri = Environment.GetEnvironmentVariable("CLIENT_URI");

                var emailData = new SendGridTemplatedEmailDto
                {
                    SendToEmail = user.Email,
                    SendToUsername = user.UserName,
                    RedirectUri = new Uri(clientUri)
                };

                await _emailService.SendConfirmResetPasswordTemplatedEmail(emailData);

                // TODO: RECORD DATA WHEN LOGGING IS INSERTED INTO PROJECTS

                return Unit.Value;
            }
        }
    }
}

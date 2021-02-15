using MediatR;
using Recipes.Application.Contracts.Identity;
using Recipes.Application.Contracts.Notifications.SendGrid;
using Recipes.Application.Dtos.Notifications.SendGrid;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Recipes.Application.Dtos.Identity.Commands
{
    public class AdminRegisterUserCommand : IRequest
    {
        public string Email { get; set; }

        public class Handler : IRequestHandler<AdminRegisterUserCommand>
        {
            private readonly IIdentityService _identityService;
            private readonly IEmailService _emailService;

            public Handler(IIdentityService identityService, IEmailService emailService)
            {
                _identityService = identityService;
                _emailService = emailService;
            }

            public async Task<Unit> Handle(AdminRegisterUserCommand request, CancellationToken cancellationToken)
            {
                await _identityService.InsertPasswordlessUserAsync(request.Email);
                _ = await _identityService.FindUserByEmailAsync(request.Email);

                // send email when SendGrid service completed.
                var clientUri = Environment.GetEnvironmentVariable("CLIENT_URI");
                var builder = new UriBuilder(clientUri);
                builder.Path = "/register";
                builder.Query = $"email={request.Email}";

                var emailData = new SendGridTemplatedEmailDto
                {
                    SendToEmail = request.Email,
                    SendToUsername = request.Email,
                    RedirectUri = new Uri(builder.ToString())
                };

                await _emailService.SendTemplatedEmail(emailData);

                return Unit.Value;
            }
        }
    }
}

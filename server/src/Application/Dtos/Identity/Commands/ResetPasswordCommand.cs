using MediatR;
using Recipes.Application.Contracts.Identity;
using Recipes.Application.Contracts.Notifications.SendGrid;
using Recipes.Application.Dtos.Notifications.SendGrid;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Recipes.Application.Dtos.Identity.Commands
{
    public class ResetPasswordCommand : IRequest
    {
        public string Email { get; set; }
        public string RedirectUrl { get; set; }

        public class Handler : IRequestHandler<ResetPasswordCommand>
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

            public async Task<Unit> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
            {
                var user = await _identityService.FindUserByEmailAsync(request.Email);
                var resetToken = await _passwordService.GeneratePasswordResetTokenAsync(user);

                var emailData = new SendGridTemplatedEmailDto
                {
                    SendToEmail = user.Email,
                    SendToUsername = user.UserName,
                    RedirectUri = BuildEmailUri(request.RedirectUrl, user, resetToken)
                };

                await _emailService.SendResetPasswordTemplatedEmail(emailData);

                // TODO: RECORD DATA WHEN LOGGING IS INSERTED INTO PROJECTS

                return Unit.Value;
            }

            private static Uri BuildEmailUri(string redirectUrl, Microsoft.AspNetCore.Identity.IdentityUser user, string resetToken)
            {
                var clientUri = Environment.GetEnvironmentVariable("CLIENT_URI");
                var builder = new UriBuilder(clientUri);
                builder.Path = "/reset-password";
                var queryList = new List<string>
                {
                    { $"token={WebUtility.UrlEncode(resetToken)}" },
                    { $"email={WebUtility.UrlEncode(user.Email)}" }
                };

                if (!string.IsNullOrWhiteSpace(redirectUrl))
                {
                    queryList.Add($"redirectUrl={WebUtility.UrlEncode(redirectUrl)}");
                }

                builder.Query = string.Join("&", queryList);
                return new Uri(builder.ToString());
            }
        }
    }
}

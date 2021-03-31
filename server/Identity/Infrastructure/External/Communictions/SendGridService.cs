using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using Recipes.Identity.Application.Common.Exceptions;
using Recipes.Identity.Application.Common.Models;
using Recipes.Identity.Application.Contracts.Services;
using Recipes.Identity.Infrastructure.Loaders.SettingsModels;
using System.Threading;
using Recipes.Identity.Application.Contracts;
using System.Net;
using System.Collections.Generic;

namespace Recipes.Identity.Infrastructure.External.Communictions
{
    public class SendGridService : IEmailService
    {
        private readonly SendGridSettings _sendGridSettings;
        private readonly SendGridClient _sendGridClient;
        private readonly EmailAddress _defaultFromEmailAddress;
        private readonly IJwtService _jwtService;

        public SendGridService(IOptions<SendGridSettings> sendGridSettings, IJwtService jwtService)
        {
            _sendGridSettings = sendGridSettings.Value;
            _sendGridClient = new SendGridClient(_sendGridSettings.ApiKey);
            _defaultFromEmailAddress = new EmailAddress("info@gardnerwebtech.com", "Recipes @ Gardner Web and Tech");
            _jwtService = jwtService;
        }

        public SendGridTemplatedEmailDto CreateTokenizedEmailDto(string email, string token, string endpoint)
        {
            return new SendGridTemplatedEmailDto
            {
                SendToEmail = email,
                RedirectUri = new Uri(BuildTokenedUrl(email, token, endpoint))
            };
        }

        public async Task SendInvitationEmail(SendGridTemplatedEmailDto dto, CancellationToken cancellationToken)
        {
            var to = new EmailAddress(dto.SendToEmail, dto.SendToUsername);
            var templateId = GetEmailTemplateId(TemplateTypes.Register);

            var templateData = new
            {
                register_url = dto.RedirectUri
            };

            var email = MailHelper.CreateSingleTemplateEmail(_defaultFromEmailAddress, to, templateId, templateData);
            var response = await _sendGridClient.SendEmailAsync(email, cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Email not sent. Please delete user and try again.");
            }
        }

        public async Task SendResetPasswordEmail(SendGridTemplatedEmailDto dto, CancellationToken cancellationToken)
        {
            var to = new EmailAddress(dto.SendToEmail, dto.SendToUsername);
            var templateId = GetEmailTemplateId(TemplateTypes.ResetPassword);

            var templateData = new
            {
                reset_password_url = dto.RedirectUri
            };

            var email = MailHelper.CreateSingleTemplateEmail(_defaultFromEmailAddress, to, templateId, templateData);
            var response = await _sendGridClient.SendEmailAsync(email, cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Email not sent. Please delete user and try again.");
            }
        }

        public async Task SendConfirmResetPasswordEmail(SendGridTemplatedEmailDto dto, CancellationToken cancellationToken)
        {
            var to = new EmailAddress(dto.SendToEmail, dto.SendToUsername);
            var templateId = GetEmailTemplateId(TemplateTypes.ConfirmResetPassword);

            var templateData = new
            {
                login_url = dto.RedirectUri,
                reset_date = DateTime.UtcNow.ToString("g")
            };

            var email = MailHelper.CreateSingleTemplateEmail(_defaultFromEmailAddress, to, templateId, templateData);
            var response = await _sendGridClient.SendEmailAsync(email, cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Email not sent. Please delete user and try again.");
            }

        }

        private string BuildTokenedUrl(string email, string token, string path)
        {
            var builder = new UriBuilder(Environment.GetEnvironmentVariable("ClientUri"));
            builder.Path = path;

            var queryList = new List<string>
            {
                { $"token={WebUtility.UrlEncode(token)}" },
                { $"email={WebUtility.UrlEncode(email)}" }
            };

            builder.Query = string.Join("&", queryList);

            return builder.ToString();
        }

        private enum TemplateTypes
        {
            Register = 1,
            ResetPassword,
            ConfirmResetPassword
        }

        private string GetEmailTemplateId(TemplateTypes templateType) =>
            templateType switch
            {
                TemplateTypes.Register => _sendGridSettings.TemplateIds.RegistrationTemplateId,
                TemplateTypes.ResetPassword => _sendGridSettings.TemplateIds.ResetPasswordTemplateId,
                TemplateTypes.ConfirmResetPassword => _sendGridSettings.TemplateIds.ConfirmResetPasswordTemplateId,
                _ => throw new NotFoundException()
            };
    }
}

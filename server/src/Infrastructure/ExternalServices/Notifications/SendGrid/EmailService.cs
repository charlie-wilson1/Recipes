using System;
using System.Threading.Tasks;
using SendGrid;
using SendGrid.Helpers.Mail;
using Recipes.Application.Contracts.Notifications.SendGrid;
using Recipes.Application.Dtos.Notifications.SendGrid;
using Recipes.Application.Common.Exceptions;
using Microsoft.Extensions.Options;

namespace Recipes.Infrastructure.ExternalServices.Notifications.SendGrid
{
    public class EmailService : IEmailService
    {
        private readonly SendGridSettings _sendGridSettings;
        private readonly SendGridClient _sendGridClient;
        private readonly EmailAddress _defaultFromEmailAddress;

        public EmailService(IOptions<SendGridSettings> sendGridSettings)
        {
            _sendGridSettings = sendGridSettings.Value;
            _sendGridClient = new SendGridClient(_sendGridSettings.ApiKey);
            _defaultFromEmailAddress = new EmailAddress("info@gardnerwebtech.com", "Recipes @ Gardner Web and Tech");
        }

        public async Task SendTemplatedEmail(SendGridTemplatedEmailDto dto)
        {
            var to = new EmailAddress(dto.SendToEmail, dto.SendToUsername);
            var templateId = GetEmailTemplateId(TemplateTypes.Register);

            var templateData = new
            {
                register_url = dto.RedirectUri
            };

            var email = MailHelper.CreateSingleTemplateEmail(_defaultFromEmailAddress, to, templateId, templateData);
            var response = await _sendGridClient.SendEmailAsync(email);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Email not sent. Please delete user and try again.");
            }
        }

        public async Task SendResetPasswordTemplatedEmail(SendGridTemplatedEmailDto dto)
        {
            var to = new EmailAddress(dto.SendToEmail, dto.SendToUsername);
            var templateId = GetEmailTemplateId(TemplateTypes.ResetPassword);

            var templateData = new
            {
                reset_password_url = dto.RedirectUri
            };

            var email = MailHelper.CreateSingleTemplateEmail(_defaultFromEmailAddress, to, templateId, templateData);
            var response = await _sendGridClient.SendEmailAsync(email);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Email not sent. Please delete user and try again.");
            }
        }

        public async Task SendConfirmResetPasswordTemplatedEmail(SendGridTemplatedEmailDto dto)
        {
            var to = new EmailAddress(dto.SendToEmail, dto.SendToUsername);
            var templateId = GetEmailTemplateId(TemplateTypes.ConfirmResetPassword);

            var templateData = new
            {
                login_url = dto.RedirectUri,
                reset_date = DateTime.UtcNow.ToString("g")
            };

            var email = MailHelper.CreateSingleTemplateEmail(_defaultFromEmailAddress, to, templateId, templateData);
            var response = await _sendGridClient.SendEmailAsync(email);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Email not sent. Please delete user and try again.");
            }
            
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

namespace Recipes.Infrastructure.ExternalServices.Notifications.SendGrid
{
    public class SendGridSettings
    {
        public string ApiKey { get; set; }
        public TemplateIdsDto TemplateIds { get; set; }
    }

    public class TemplateIdsDto
    {
        public string RegistrationTemplateId { get; set; }
        public string ResetPasswordTemplateId { get; set; }
        public string ConfirmResetPasswordTemplateId { get; set; }
    }
}

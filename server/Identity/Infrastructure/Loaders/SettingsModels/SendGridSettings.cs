namespace Recipes.Identity.Infrastructure.Loaders.SettingsModels
{
    public class SendGridSettings
    {
        public string ApiKey { get; init; }
        public TemplateIdsDto TemplateIds { get; init; }
    }


    public class TemplateIdsDto
    {
        public string RegistrationTemplateId { get; init; }
        public string ResetPasswordTemplateId { get; init; }
        public string ConfirmResetPasswordTemplateId { get; init; }
    }
}

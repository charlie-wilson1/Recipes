namespace Recipes.Infrastructure.Identity.Models
{
    public class ProviderSettings
    {
        public ProviderSettings() 
        { 
            ProviderPurposes = new ProviderPurposes(); 
        }

        public ProviderSettings(string name)
        {
            Name = name;
            ProviderPurposes = new ProviderPurposes();
        }

        public string Name { get; init; }
        public ProviderPurposes ProviderPurposes { get; }
    }

    public class ProviderPurposes
    {
        public string RefreshToken { get => "RefreshToken"; }
    }
}
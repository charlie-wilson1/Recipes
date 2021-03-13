using Microsoft.AspNetCore.Identity;
using Recipes.Infrastructure.Identity.Models;

namespace Recipes.Infrastructure
{
    public class InfastructureSettingsDto
    {
        public string ConnectionString { get; set; }
        public JwtBearerTokenSettings JwtBearerTokenSettings { get; set; }
        public IdentitySeedSettings IdentitySeedSettings { get; set; }
        public ProviderSettings ProviderSettings { get; set; }
        public bool IsDevelopment { get; set; }
    }
}

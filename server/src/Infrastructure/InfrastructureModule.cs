using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Recipes.Infrastructure.Identity;
using Recipes.Infrastructure.Persistence;
using Recipes.Infrastructure.Persistence.DependencyInjection;

namespace Recipes.Infrastructure
{
    public static class InfrastructureModule
    {
        public static void AddInfrastructureModule(this IServiceCollection services, InfastructureSettingsDto settings)
        {
            services.AddDbContext<ApplicationDbContext>(options => 
                options.UseSqlServer(settings.ConnectionString));

            services.RegisterDependencies();
            services.ConfigureIdentity(settings.JwtBearerTokenSettings, settings.IsDevelopment);

            if (settings.IdentitySeedSettings is not null)
            {
                services.AddIdentityConfiguration(settings.IdentitySeedSettings);
            }
        }
    }
}

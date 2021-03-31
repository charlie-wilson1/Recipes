using Microsoft.Extensions.DependencyInjection;
using Recipes.Identity.Infrastructure.Loaders;
using Recipes.Identity.Infrastructure.Raven;

namespace Recipes.Identity.Infrastructure
{
    public static class InfrastructureModule
    {
        public static void AddInfrastructureModule(this IServiceCollection services)
        {
            services.RegisterDependencies();
            services.AddRavenModule();
        }
    }
}

using Microsoft.Extensions.DependencyInjection;
using Recipes.Core.Infrastructure.Loaders;
using Recipes.Core.Infrastructure.Raven;

namespace Recipes.Core.Infrastructure
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

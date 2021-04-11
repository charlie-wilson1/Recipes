using Microsoft.Extensions.DependencyInjection;
using Recipes.Core.Application.Contracts.Repositories;
using Recipes.Core.Application.Contracts.Services;
using Recipes.Core.Infrastructure.Common;
using Recipes.Core.Infrastructure.External.Files;
using Recipes.Core.Infrastructure.Raven.Repositories;
using Recipes.Core.Infrastructure.Services;

namespace Recipes.Core.Infrastructure.Loaders
{
    internal static class DependencyInjection
    {
        internal static void RegisterDependencies(this IServiceCollection services)
        {
            services.AddTransient<IDateTime, DateTimeProvider>();
            services.AddTransient<IRecipeService, RecipeService>();
            services.AddTransient<IRecipeRepository, RecipesRepository>();
            services.AddSingleton<IFileService, GoogleFileService>();
        }
    }
}

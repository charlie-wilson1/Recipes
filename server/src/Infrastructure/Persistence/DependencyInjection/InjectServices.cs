using Microsoft.Extensions.DependencyInjection;
using Recipes.Application.Contracts;
using Recipes.Application.Contracts.Identity;
using Recipes.Application.Contracts.Notifications.SendGrid;
using Recipes.Application.Contracts.RecipeRepositories;
using Recipes.Infrastructure.ExternalServices.Notifications.SendGrid;
using Recipes.Infrastructure.Identity.Services;
using Recipes.Infrastructure.Persistence.Services;
using Recipes.Infrastructure.RecipeRepositories;

namespace Recipes.Infrastructure.Persistence.DependencyInjection
{
    internal static class InjectServices
    {
        internal static void RegisterDependencies(this IServiceCollection services)
        {
            services.AddScoped<IApplicationDbContext>(provider => provider.GetService<ApplicationDbContext>());

            // Identity
            services.AddTransient<IIdentityService, IdentityService>();
            services.AddTransient<IIdentityRoleService, IdentityRoleService>();
            services.AddTransient<IPasswordService, PasswordService>();
            services.AddTransient<IJwtService, JwtService>();

            // Repos
            services.AddScoped(typeof(IGenericAuditableEntityRepository<>), typeof(GenericAuditableEntityRepository<>));

            // Other Services
            services.AddTransient<IDateTime, DateTimeProvider>();
            services.AddTransient<IEmailService, EmailService>();
        }
    }
}

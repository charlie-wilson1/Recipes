using Microsoft.Extensions.DependencyInjection;
using Recipes.Identity.Application.Contracts.Repositories;
using Recipes.Identity.Application.Contracts.Services;
using Recipes.Identity.Infrastructure.Common;
using Recipes.Identity.Infrastructure.External.Communictions;
using Recipes.Identity.Infrastructure.Raven.Repositories;
using Recipes.Identity.Infrastructure.Services;

namespace Recipes.Identity.Infrastructure.Loaders
{
    internal static class DependencyInjection
    {
        internal static void RegisterDependencies(this IServiceCollection services)
        {
            services.AddTransient<IDateTime, DateTimeProvider>();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IInvitationRepository, InvitationRepository>();
            services.AddTransient<IJwtService, JwtService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IInvitationService, InvitationService>();
            services.AddTransient<IPasswordService, PasswordService>();
            services.AddTransient<IEmailService, SendGridService>();
        }
    }
}

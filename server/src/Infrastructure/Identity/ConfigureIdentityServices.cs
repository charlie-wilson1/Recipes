using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Recipes.Infrastructure.Identity.Models;
using Recipes.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Recipes.Infrastructure.Identity
{
    internal static class ConfigureIdentityServices
    {
        internal static void AddIdentityConfiguration(this IServiceCollection services, IdentitySeedSettings seedSettings)
        {
            var serviceProvider = services.BuildServiceProvider();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var roleNames = new List<string> { "Admin", "Member" };
            
            foreach (var roleName in roleNames)
            {
                var roleExists = roleManager.RoleExistsAsync(roleName).Result;

                if (!roleExists)
                {
                    var role = new IdentityRole
                    {
                        Name = roleName,
                        NormalizedName = roleName.ToUpper()
                    };

                    roleManager.CreateAsync(role).Wait();
                }
            }

            var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
            var userExists = userManager.FindByNameAsync(seedSettings.AdminUsername).Result;

            if (userExists is null)
            {
                var normalizedUsername = userManager.NormalizeName(seedSettings.AdminUsername);
                var normalizedEmail = userManager.NormalizeEmail(seedSettings.AdminEmail);

                var user = new ApplicationUser
                {
                    UserName = seedSettings.AdminUsername,
                    Email = seedSettings.AdminEmail,
                    NormalizedUserName = normalizedUsername,
                    NormalizedEmail = normalizedEmail
                };

                userManager.CreateAsync(user, seedSettings.AdminPassword).Wait();
                userManager.AddToRolesAsync(user, roleNames).Wait();
            }
            else
            {
                var userIsAdmin = userManager.IsInRoleAsync(userExists, "Admin").Result;

                if (!userIsAdmin)
                {
                    userManager.AddToRoleAsync(userExists, "Admin").Wait();
                }
            }
        }

        internal static void ConfigureIdentity(this IServiceCollection services, JwtBearerTokenSettings jwtSettings, bool isDevelopment)
        {
            services.AddIdentity<ApplicationUser, IdentityRole>(options => options.User.RequireUniqueEmail = true)
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.Configure<DataProtectionTokenProviderOptions>(options => options.TokenLifespan = TimeSpan.FromSeconds(jwtSettings.ExpiryTimeInSeconds));

            var builder = new IdentityBuilder(typeof(IdentityUser), services);
            builder.AddTokenProvider("Recipes", typeof(DataProtectorTokenProvider<IdentityUser>));

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options =>
                {
                    if (isDevelopment)
                    {
                        options.RequireHttpsMetadata = false;
                    }

                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ClockSkew = TimeSpan.FromMinutes(2),
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSettings.SecretKey)),
                        RequireExpirationTime = true,
                        RequireSignedTokens = true,
                        ValidateIssuerSigningKey = true,
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = !isDevelopment,
                        ValidIssuer = jwtSettings.Issuer
                    };

                    options.Events = new JwtBearerEvents
                    {
                        OnAuthenticationFailed = context =>
                        {
                            if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                            {
                                context.Response.Headers.Add("Token-Expired", "true");
                            }
                            else
                            {
                                context.Response.Headers.Add("Token-Error", context.Exception.Message);
                            }
                            return Task.CompletedTask;
                        }
                    };
                });
        }
    }
}

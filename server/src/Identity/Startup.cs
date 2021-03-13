using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using FluentValidation.AspNetCore;
using Recipes.Infrastructure.Identity.Models;
using Recipes.Infrastructure;
using Recipes.Infrastructure.ExternalServices.Notifications.SendGrid;
using Recipes.Identity.Filters;
using Recipes.Application;
using Recipes.Application.Contracts.Identity;
using Recipes.Identity.Services;

namespace Recipes.Identity
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment hostEnvironment)
        {
            Configuration = configuration;
            CurrentEnvironment = hostEnvironment;
        }

        public IConfiguration Configuration { get; }
        private IWebHostEnvironment CurrentEnvironment { get; set; }
        private readonly string AllowClientOrigin = "_myAllowSpecificOrigins";


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // get settings from secrets
            var connectionString = Configuration.GetConnectionString("DefaultConnection");
            var jwtSettings = Configuration.GetSection("Auth:JwtBearerTokenSettings").Get<JwtBearerTokenSettings>();
            var providerSettings = Configuration.GetSection("ProviderSettings").Get<ProviderSettings>();
            var clientRoute = Configuration.GetSection("Auth:ClientRoute").Value;

            services.Configure<JwtBearerTokenSettings>(Configuration.GetSection("Auth:JwtBearerTokenSettings"));
            services.Configure<SendGridSettings>(Configuration.GetSection("SendGrid"));
            services.Configure<ProviderSettings>(Configuration.GetSection("ProviderSettings"));

            var adminEmail = Configuration.GetSection("Auth:AdminUser:Email").Value;
            var adminUsername = Configuration.GetSection("Auth:AdminUser:Username").Value;
            var adminPassword = Configuration.GetSection("Auth:AdminUser:Password").Value;

            var adminSettings = new IdentitySeedSettings
            {
                AdminEmail = adminEmail,
                AdminUsername = adminUsername,
                AdminPassword = adminPassword
            };

            var configSettings = new InfastructureSettingsDto
            {
                ConnectionString = connectionString,
                JwtBearerTokenSettings = jwtSettings,
                IdentitySeedSettings = adminSettings,
                ProviderSettings = providerSettings,
                IsDevelopment = CurrentEnvironment.IsDevelopment(),
            };

            // API specific registrations
            services.AddControllers(options => options.Filters.Add(new ApiExceptionFilter()));
            services.AddHttpContextAccessor();
            services.AddMvc().AddFluentValidation();
            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddTransient<ICurrentUserService, CurrentUserService>();

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "Recipes.Identity", Version = "v1" });
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = JwtBearerDefaults.AuthenticationScheme,
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter 'Bearer' [space] and then your valid token in the text input below.\r\n\r\nExample: \"Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9\"",
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                          new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                            new string[] {}
                    }
                });
            });

            services.AddCors(options =>
            {
                options.AddPolicy(name: AllowClientOrigin,
                                  builder =>
                                  {
                                      builder.WithOrigins(clientRoute);
                                      builder.AllowAnyMethod();
                                      builder.AllowAnyHeader();
                                      builder.AllowCredentials();
                                  });
            });

            // other setup
            services.AddInfrastructureModule(configSettings);
            services.AddApplicationModule();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            CurrentEnvironment = env;

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Recipes.Identity v1"));
            }

            app.UseHttpsRedirection();
            app.UseCors(AllowClientOrigin);
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

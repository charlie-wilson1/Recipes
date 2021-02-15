using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Recipes.Application;
using Recipes.Application.Contracts.Identity;
using Recipes.Infrastructure.ExternalServices.Notifications.SendGrid;
using Recipes.Infrastructure.Identity.Models;
using Recipes.Infrastructure;
using Recipes.WebApi.Filters;

namespace Recipes.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment currentEnvironment)
        {
            Configuration = configuration;
            CurrentEnvironment = currentEnvironment;
        }

        public IConfiguration Configuration { get; }
        private IWebHostEnvironment CurrentEnvironment { get; set; }
        private readonly string AllowClientOrigin = "_allowClientOrigin";

        public void ConfigureServices(IServiceCollection services)
        {
            var connectionString = Configuration.GetConnectionString("DefaultConnection");
            var jwtSettings = Configuration.GetSection("Auth:JwtBearerTokenSettings").Get<JwtBearerTokenSettings>();
            var clientRoute = Configuration.GetSection("Auth:ClientRoute").Value;

            services.Configure<JwtBearerTokenSettings>(Configuration.GetSection("Auth:JwtBearerTokenSettings"));
            services.Configure<SendGridSettings>(Configuration.GetSection("SendGrid"));

            var servicesSettings = new InfastructureSettingsDto
            {
                ConnectionString = connectionString,
                JwtBearerTokenSettings = jwtSettings,
                IsDevelopment = CurrentEnvironment.IsDevelopment(),
            };
            
            services.AddControllers(options => options.Filters.Add(new ApiExceptionFilter()));
            services.AddHttpContextAccessor();
            services.AddMvc().AddFluentValidation();
            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddTransient<ICurrentUserService, CurrentUserService>();

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "Recipes.WebApi", Version = "v1" });
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
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

            services.AddInfrastructureModule(servicesSettings);
            services.AddApplicationModule();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            CurrentEnvironment = env;

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Recipes.WebApi v1"));
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

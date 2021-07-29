using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using FluentValidation.AspNetCore;
using Recipes.Core.Infrastructure.Loaders.SettingsModels;
using Recipes.Core.Api.Loaders;
using Recipes.Core.Infrastructure;
using Recipes.Core.Application;
using Recipes.Core.Application.Contracts.Services;
using Recipes.Core.Api.Services;
using Recipes.Core.Api.Filters;
using System.IO;

namespace Recipes.Core.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        private bool isDevelopment { get; set; }
        private readonly string AllowClientOrigin = "recipesOrigin";

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var jwtSettings = Configuration.GetSection("Auth:JwtBearerTokenSettings").Get<JwtBearerTokenSettings>();
            var clientRoute = Configuration.GetSection("Auth:ClientRoute").Value;
            var googleSettings = Configuration.GetSection("Google").Get<GoogleClientSettings>();

            if (jwtSettings == null || clientRoute == null || googleSettings == null || !File.Exists(googleSettings.CredentialsFilePath))
            {
                throw new ArgumentException("Settings cannot be null");
            }

            services.Configure<GoogleClientSettings>(Configuration.GetSection("Google"));
            services.AddControllers(opts => opts.Filters.Add(new ApiExceptionFilter()));
            services.AddHttpContextAccessor();
            services.AddMvc().AddFluentValidation();
            services.AddDatabaseDeveloperPageExceptionFilter();
            services.AddTransient<ICurrentUserService, CurrentUserService>();

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "Recipes.Core", Version = "v1" });
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

            services.ConfigureIdentity(jwtSettings, isDevelopment);
            services.AddInfrastructureModule();
            services.AddApplicationModule();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            isDevelopment = env.IsDevelopment();

            if (isDevelopment)
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Recipes.Core v1"));
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

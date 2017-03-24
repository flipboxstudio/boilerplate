using System;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using Boilerplate.Middlewares;
using Boilerplate.Options;
using Boilerplate.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace Boilerplate
{
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", false, true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add options
            services.AddOptions();

            // Add memory cache support
            services.AddMemoryCache();

            // Add framework services.
            // Make authentication compulsory across the board (i.e. shut
            // down EVERYTHING unless explicitly opened up).
            services.AddMvc(config =>
            {
                var policy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();
                config.Filters.Add(new AuthorizeFilter(policy));
            });

            // Configure Database
            services.AddSingleton(typeof(Database),
                new Database(Configuration.GetSection("ConnectionString")["Default"]));

            // Configure JwtIssuerOptions
            services.Configure<JwtConfig>(options =>
            {
                var jwtAppSettingOptions = Configuration.GetSection(nameof(JwtConfig));

                options.SigningKey = jwtAppSettingOptions[nameof(JwtConfig.SigningKey)];
                options.Issuer = jwtAppSettingOptions[nameof(JwtConfig.Issuer)];
                options.Audience = jwtAppSettingOptions[nameof(JwtConfig.Audience)];
                options.SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.ASCII.GetBytes(options.SigningKey)),
                    SecurityAlgorithms.HmacSha256
                );
            });

            // Use policy auth.
            services.AddAuthorization(options =>
            {
                options.AddPolicy("Admin",
                    policy => policy.RequireClaim(AppConfig.AuthRoleIdentifierName, "Admin"));
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            // Add logging for development
            if (env.IsDevelopment())
            {
                loggerFactory.AddConsole(Configuration.GetSection("Logging"));
                loggerFactory.AddDebug();
            }

            // JSON error response
            app.UseMiddleware(typeof(ErrorHandlerMiddleware));

            // JWT
            var jwtAppSettingOptions = Configuration.GetSection(nameof(JwtConfig));

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = jwtAppSettingOptions[nameof(JwtConfig.Issuer)],
                ValidateAudience = true,
                ValidAudience = jwtAppSettingOptions[nameof(JwtConfig.Audience)],
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.ASCII.GetBytes(
                        jwtAppSettingOptions[nameof(JwtConfig.SigningKey)]
                    )
                ),
                RequireExpirationTime = true,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };
            var jwtBearerOptions = new JwtBearerOptions
            {
                AutomaticAuthenticate = true,
                AutomaticChallenge = true,
                TokenValidationParameters = tokenValidationParameters
            };
            var tokenValidator = new TokenValidator
            {
                Cache = (IMemoryCache) app.ApplicationServices.GetService(typeof(IMemoryCache))
            };

            jwtBearerOptions.SecurityTokenValidators.Clear();
            jwtBearerOptions.SecurityTokenValidators.Add(tokenValidator);

            app.UseJwtBearerAuthentication(jwtBearerOptions);
            // End of JWT

            // Add MVC
            app.UseMvc();
        }
    }
}
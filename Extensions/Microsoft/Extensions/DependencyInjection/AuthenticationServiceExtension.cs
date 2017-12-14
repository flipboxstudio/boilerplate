using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using App;
using App.Services.Auth;
using App.Services.Db;
using App.Services.Db.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class AuthenticationServiceExtension
    {
        /// <summary>
        /// Add authentication service.
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddAuthentication(
            this IServiceCollection serviceCollection,
            IServiceProvider serviceProvider
        )
        {
            var appSettings = serviceProvider.GetService<IOptions<AppSettings>>().Value;

            // ===== Add Identity =====
            serviceCollection.AddIdentity<ApplicationUser, ApplicationRole>(options => {
                // User settings
                options.User.RequireUniqueEmail = true;

                // ===== Password Settings =====
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = true;
                options.Password.RequiredUniqueChars = 1;

                // ===== Lockout settings =====
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                // ===== Signin settings =====
                options.SignIn.RequireConfirmedEmail = false;
                options.SignIn.RequireConfirmedPhoneNumber = false;
            }).AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();

            // ===== Add Jwt Authentication =====
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear(); // => remove default claims
            serviceCollection.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(jwtBearerOptions =>
            {
                jwtBearerOptions.Configuration = new OpenIdConnectConfiguration();
                jwtBearerOptions.Audience = appSettings.Jwt.Issuer;
                jwtBearerOptions.Authority = appSettings.Jwt.Issuer;
                jwtBearerOptions.RequireHttpsMetadata = false;
                jwtBearerOptions.SaveToken = true;
                jwtBearerOptions.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = appSettings.Jwt.Issuer,
                    ValidAudience = appSettings.Jwt.Issuer,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(appSettings.Jwt.Key))
                };
            });

            // ===== Add another service for dependency injection feature =====
            serviceCollection.AddSingleton<TokenGenerator, TokenGenerator>();

            return serviceCollection;
        }
    }
}
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
            serviceCollection.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            // ===== Configure Identity =====
            serviceCollection.Configure<IdentityOptions>(options =>
            {
                //
            });

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
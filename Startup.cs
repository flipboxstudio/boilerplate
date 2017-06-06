using System;
using System.Text;
using App.Services;
using App.Options;
using App.Middlewares;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Serialization;
using System.IO;

namespace App
{
    public class Startup
    {
        /// <summary>
        /// Class constructor.
        /// </summary>
        /// <param name="env"></param>
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{Environment.MachineName}.json", optional: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            // Add options
            services.AddOptions();

            // Add framework services.
            services.AddMvc(config =>
            {
                var policy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();
                config.Filters.Add(new AuthorizeFilter(policy));
            }).AddJsonOptions(option =>
            {
                option.SerializerSettings.ContractResolver = new AppContractResolver();
            });

            // Configure Database
            var databaseConfiguration = Configuration.GetSection("Database").GetSection("Default");
            services.AddSingleton(typeof(Database),
                new Database(
                    databaseConfiguration.GetValue<string>("ConnectionString")
                )
            );

            // Configure JWTConfiguration.
            services.Configure<JwtConfig>(jwtConfig =>
            {
                var jwtAppSettingOptions = Configuration.GetSection("JwtConfig");

                jwtConfig.SigningKey = jwtAppSettingOptions.GetValue<string>("SigningKey");
                jwtConfig.Issuer = jwtAppSettingOptions.GetValue<string>("Issuer");
                jwtConfig.Audience = jwtAppSettingOptions.GetValue<string>("Audience");
                jwtConfig.SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtConfig.SigningKey)),
                    SecurityAlgorithms.HmacSha256
                );
            });

            // Add authentication services.
            services.AddTransient<Authenticator, Authenticator>();

            // Add authorization middleware.
            services.AddAuthorization();

            // Configure Application Configuration
            services.Configure<AppConfig>(appConfig =>
            {
                // Configure Mailer
                var mailerConfiguration = Configuration.GetSection("Mail");
                appConfig.MailerHost = mailerConfiguration.GetValue<string>("Host");
                appConfig.MailerPort = mailerConfiguration.GetValue<int>("Port");
                appConfig.MailerUseSSL = mailerConfiguration.GetValue<bool>("SSL");
                appConfig.MailerUserName = mailerConfiguration.GetValue<string>("Username");
                appConfig.MailerDisplayName = mailerConfiguration.GetValue<string>("DisplayName");
                appConfig.MailerRelayName = mailerConfiguration.GetValue<string>("RelayName");
                appConfig.MailterTemplatePath = Path.Combine(
                    Directory.GetCurrentDirectory(),
                    mailerConfiguration.GetValue<string>("TemplatePath")
                );

                // Configure Uploader
                var uploaderConfiguration = Configuration.GetSection("Uploader");
                appConfig.Host = uploaderConfiguration.GetValue<string>("Host");
                appConfig.RelativeUploadPath = uploaderConfiguration.GetValue<string>("UploadPath");
                appConfig.AbsoluteUploadPath = Path.Combine(
                    Directory.GetCurrentDirectory(),
                    appConfig.RelativeUploadPath
                );
            });

            // Add Mailer services.
            services.AddTransient<Mailer, Mailer>();
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        /// <param name="loggerFactory"></param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            // Add logging for development
            if (env.IsDevelopment())
            {
                loggerFactory.AddConsole(Configuration.GetSection("Logging"));
                loggerFactory.AddDebug();
            }

            app.UseStaticFiles();

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

            app.UseJwtBearerAuthentication(jwtBearerOptions);
            // End of JWT

            app.UseMvc();
        }
    }
}

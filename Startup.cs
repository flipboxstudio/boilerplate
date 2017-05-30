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
                    databaseConfiguration["ConnectionString"]
                )
            );

            // Configure JwtIssuerOptions
            services.Configure<JwtConfig>(options =>
            {
                var jwtAppSettingOptions = Configuration.GetSection(nameof(JwtConfig));

                options.SigningKey = jwtAppSettingOptions[nameof(JwtConfig.SigningKey)];
                options.Issuer = jwtAppSettingOptions[nameof(JwtConfig.Issuer)];
                AppConfig.Host = options.Audience = jwtAppSettingOptions[nameof(JwtConfig.Audience)];
                options.SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.ASCII.GetBytes(options.SigningKey)),
                    SecurityAlgorithms.HmacSha256
                );
            });

            // Add authentication services.
            services.AddTransient<Authenticator, Authenticator>();

            // Add authorization middleware.
            services.AddAuthorization();

            // Configure Uploader
            AppConfig.RelativeUploadPath = Configuration.GetValue<string>("UploadPath");
            AppConfig.FileSystemUploadPath = string.Format(
                "{0}/{1}",
                Directory.GetCurrentDirectory(),
                AppConfig.RelativeUploadPath
            );

            // Configure Mailer
            var mailerConfiguration = Configuration.GetSection("Mail");
            AppConfig.MailerUser = mailerConfiguration.GetValue<string>("Username");
            AppConfig.MailerName = mailerConfiguration.GetValue<string>("Name");
            services.AddSingleton(typeof(Mailer),
                new Mailer(
                    mailerConfiguration.GetValue<string>("Host"),
                    mailerConfiguration.GetValue<int>("Port"),
                    mailerConfiguration.GetValue<bool>("SSL"),
                    mailerConfiguration.GetValue<string>("Username"),
                    mailerConfiguration.GetValue<string>("Password")
                )
            );
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

#region using

using App.Middlewares;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.Webpack;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

#endregion

namespace App
{
    public class Startup
    {
        /// <summary>
        ///     Application configuration.
        /// </summary>
        /// <returns></returns>
        private readonly IConfiguration _configuration;

        /// <summary>
        ///     Class constructor.
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        ///     This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="serviceCollection"></param>
        public void ConfigureServices(IServiceCollection serviceCollection)
        {
            // ===== Strongly typed application settings =====
            serviceCollection.AddSingleton(_configuration);
            serviceCollection.Configure<AppSettings>(_configuration.GetSection("AppSettings"));

            // ===== Add database service =====
            serviceCollection.AddDatabaseService()
                // ===== Add authentication service =====
                .AddAuthenticationService()
                // ===== Add CORS service =====
                .AddCors(corsOptions =>
                {
                    corsOptions.AddPolicy("default", corsPolicyBuilder =>
                    {
                        // ===== Allow all =====
                        corsPolicyBuilder
                            .AllowAnyOrigin()
                            .AllowAnyHeader()
                            .AllowAnyMethod();
                    });
                })
                // ===== Add MVC service =====
                .AddMvc()
                // ===== Configure JSON naming strategy =====
                .AddJsonOptions(mvcJsonOptions =>
                {
                    // ===== Use snake case =====
                    mvcJsonOptions.SerializerSettings.ContractResolver = new DefaultContractResolver
                    {
                        NamingStrategy = new SnakeCaseNamingStrategy()
                    };
                    mvcJsonOptions.SerializerSettings.DefaultValueHandling = DefaultValueHandling.Include;
                    mvcJsonOptions.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;

                    JsonConvert.DefaultSettings = () => mvcJsonOptions.SerializerSettings;
                });
        }

        /// <summary>
        ///     This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="applicationBuilder"></param>
        /// <param name="hostingEnvironment"></param>
        public void Configure(IApplicationBuilder applicationBuilder, IHostingEnvironment hostingEnvironment)
        {
            if (hostingEnvironment.IsDevelopment()) {
                applicationBuilder.UseDeveloperExceptionPage();

                applicationBuilder.UseWebpackDevMiddleware(new WebpackDevMiddlewareOptions
                {
                    HotModuleReplacement = true
                });
            }

            applicationBuilder.UseStaticFiles();

            applicationBuilder.UseAuthentication();

            applicationBuilder.UseMiddleware<HttpExceptionMiddleware>();

            applicationBuilder.UseMvc(routes => {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action}/{id?}",
                    defaults: new { controller = "Home", action = "Index" }
                );

                routes.MapSpaFallbackRoute(
                    name: "spa-fallback",
                    defaults: new { controller = "Home", action = "Index" }
                );
            });
        }
    }
}
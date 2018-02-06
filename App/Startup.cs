#region using

using App.Factories;
using App.Middlewares;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SpaServices.Webpack;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebMarkupMin.AspNetCore2;

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
            serviceCollection.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
            serviceCollection.AddScoped<SpaResponseBuilder>();
            serviceCollection.AddNodeServices();

            // ===== Add Web Minification =====
            serviceCollection
                .AddWebMarkupMin()
                .AddHtmlMinification()
                .AddXmlMinification()
                .AddHttpCompression();

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
                .AddMvc();
        }

        /// <summary>
        ///     This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="applicationBuilder"></param>
        /// <param name="hostingEnvironment"></param>
        public void Configure(IApplicationBuilder applicationBuilder, IHostingEnvironment hostingEnvironment)
        {
            if (hostingEnvironment.IsDevelopment())
            {
                applicationBuilder.UseDeveloperExceptionPage();

                applicationBuilder.UseWebpackDevMiddleware(new WebpackDevMiddlewareOptions
                {
                    HotModuleReplacement = true
                });
            }
            else
            {
                // ===== Only minify on production, speed up development =====
                applicationBuilder.UseWebMarkupMin();
            }

            applicationBuilder.UseStaticFiles();

            applicationBuilder.UseAuthentication();

            applicationBuilder.UseMiddleware<HttpExceptionMiddleware>();

            applicationBuilder.UseMvc(routes =>
            {
                routes.MapRoute(
                    "default",
                    "{controller}/{action}/{id?}",
                    new {controller = "Home", action = "Index"}
                );

                routes.MapSpaFallbackRoute(
                    "spa-fallback",
                    new {controller = "Home", action = "Index"}
                );
            });
        }
    }
}
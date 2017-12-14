using App.Middlewares;
using App.Services.Db;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace App
{
    public class Startup
    {
        /// <summary>
        /// Application configuration.
        /// </summary>
        /// <returns></returns>
        private IConfiguration _configuration;

        /// <summary>
        /// Class constructor.
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            // ===== Strongly typed application settings =====
            services.AddSingleton<IConfiguration>(_configuration);
            services.Configure<AppSettings>(_configuration.GetSection("AppSettings"));

            // ===== Build the intermediate service provider =====
            var serviceProvider = services.BuildServiceProvider();

            // ===== Add database service =====
            services.AddDatabase(serviceProvider);

            // ===== Add authentication service =====
            services.AddAuthentication(serviceProvider);

            // ===== Add MVC service =====
            services.AddMvc();
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        /// <param name="applicationDbContext"></param>
        public void Configure(
            IApplicationBuilder app,
            IHostingEnvironment env,
            ApplicationDbContext applicationDbContext
        )
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            app.UseAuthentication();

            app.UseMiddleware<HttpExceptionMiddleware>();

            app.UseMvc();

            // ===== Create tables =====
            applicationDbContext.Database.EnsureCreated();
        }
    }
}

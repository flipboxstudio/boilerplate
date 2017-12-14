using System;
using App;
using App.Services.Db;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DatabaseServiceExtensions
    {
        /// <summary>
        /// Add database service.
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <param name="serviceProvider"></param>
        /// <returns></returns>
        public static IServiceCollection AddDatabase(
            this IServiceCollection serviceCollection,
            IServiceProvider serviceProvider
        )
        {
            return serviceCollection.AddDbContext<ApplicationDbContext>(optionsBuilder =>
            {
                var appSettings = serviceProvider.GetService<IOptions<AppSettings>>().Value;
                var configuration = serviceProvider.GetService<IConfiguration>();

                var databaseDriver = appSettings.Database.Driver;
                var connectionString = configuration.GetConnectionString($"{databaseDriver}Connection");

                switch (databaseDriver)
                {
                    case "Sqlite":
                        optionsBuilder.UseSqlite(connectionString);
                        break;
                    case "Mysql":
                        optionsBuilder.UseMySQL(connectionString);
                        break;
                    case "SqlServer":
                        optionsBuilder.UseSqlServer(connectionString);
                        break;
                    default:
                        throw new ApplicationException($"{databaseDriver} is not supported yet.");
                }
            });
        }
    }
}
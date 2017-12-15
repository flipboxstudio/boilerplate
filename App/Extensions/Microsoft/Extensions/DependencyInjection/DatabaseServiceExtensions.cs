#region using

#region using

using System;
using App;
using App.Services.Db;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

#endregion

// ReSharper disable CheckNamespace

#endregion

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DatabaseServiceExtensions
    {
        /// <summary>
        ///     Add database service.
        /// </summary>
        /// <param name="serviceCollection"></param>
        /// <returns></returns>
        public static IServiceCollection AddDatabaseService(this IServiceCollection serviceCollection)
        {
            return serviceCollection.AddDbContext<ApplicationDbContext>(optionsBuilder =>
            {
                var appSettings = serviceCollection.BuildServiceProvider().GetService<IOptions<AppSettings>>().Value;
                var configuration = serviceCollection.BuildServiceProvider().GetService<IConfiguration>();

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
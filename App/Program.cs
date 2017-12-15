#region using

using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

#endregion

namespace App
{
    public class Program
    {
        /// <summary>
        ///     Run the application.
        /// </summary>
        /// <param name="arguments"></param>
        public static void Main(string[] arguments)
        {
            BuildWebHost(arguments).Run();
        }

        /// <summary>
        ///     Build web host environment.
        /// </summary>
        /// <param name="arguments"></param>
        /// <returns></returns>
        private static IWebHost BuildWebHost(string[] arguments)
        {
            return WebHost.CreateDefaultBuilder(arguments)
                .UseStartup<Startup>()
                .Build();
        }
    }
}
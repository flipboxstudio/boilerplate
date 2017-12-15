using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace App
{
    public class Program
    {
        /// <summary>
        /// Run the application.
        /// </summary>
        /// <param name="arguments"></param>
        public static void Main(string[] arguments) => BuildWebHost(arguments).Run();

        /// <summary>
        /// Build web host environment.
        /// </summary>
        /// <param name="arguments"></param>
        /// <returns></returns>
        public static IWebHost BuildWebHost(string[] arguments) =>
            WebHost.CreateDefaultBuilder(arguments)
                .UseStartup<Startup>()
                .Build();
    }
}

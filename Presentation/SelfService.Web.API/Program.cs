using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System.IO;

namespace SelfService.Web.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host
                .CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    var env = hostingContext.HostingEnvironment;
                    var devAppSettingsPath = Path.Combine(env.ContentRootPath, "..\\..\\Infrastructure\\Externals\\appsettings.json");

                    // Web api specific settings
                    config.AddJsonFile("appsettings.web.api.json", optional: true);
                    // Shared settings in published mode -> Infrastructure\Externals\appsettings.json is copied on Build
                    config.AddJsonFile("appsettings.json", optional: true);
                    // Shared settings in dev mode: Infrastructure\Externals\appsettings.json
                    config.AddJsonFile(devAppSettingsPath, optional: true);
                    //       .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
        }
    }
}

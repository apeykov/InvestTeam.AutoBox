namespace InvestTeam.AutoBox.Infrastructure.Externals
{
    using InvestTeam.AutoBox.Application.Common.Interfaces;
    using Microsoft.Extensions.Configuration;
    using System.IO;
    using System.Reflection;
    using System.Text.RegularExpressions;

    /// <summary>
    ///  Used in PowerShell environment.
    ///  In Web environment ASP.NET is responsible of supplying `IConfiguration` object out of the box
    /// </summary>
    public class AutoBoxConfiguration : IApplicationConfiguration
    {
        private readonly IConfigurationRoot configuration;

        public AutoBoxConfiguration()
        {
            // App settings JSON file should be in the same location as DLLs of the App/Library
            string appSettingsFilePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            // IConfiguration object will be supplied out of the box by the ASP.NET runtime when ASP.NET WebAPI is implemented. 
            // Each level of settings overrides the previous level of settings in case of collision (commented lines).
            configuration = new ConfigurationBuilder()
                    .SetBasePath(appSettingsFilePath)
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .Build();

        }

        public IConfigurationRoot Configuration
        {
            get
            {
                return configuration;
            }
        }

        public string DbConnectionString
        {
            get
            {
                return configuration.GetConnectionString("DefaultConnection");
            }
        }

        public string DbConnectionStringPasswordExcluded
        {
            get
            {
                var regex = new Regex("Password=.*?;");

                string dbConnectionStringPasswordExcluded = regex
                    .Replace(
                        configuration.GetConnectionString("DefaultConnection"),
                        "Password=******;");

                return dbConnectionStringPasswordExcluded;
            }
        }
    }
}

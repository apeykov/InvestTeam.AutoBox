namespace InvestTeam.AutoBox.Application.Common
{
    using InvestTeam.AutoBox.Application.Common.Interfaces;
    using InvestTeam.AutoBox.Domain.Common;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class AppSettingsStore
    {
        private enum AppModes
        {
            Development = 1,
            Test = 2,
            Production = 3
        }

        private readonly IUnitOfWork unitOfWork;
        protected Dictionary<string, object> settings;

        public AppSettingsStore(IApplicationConfiguration fileBasedAppSettings, IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;

            LoadInternalSettings();
            LoadExternalSettingsFromFile(fileBasedAppSettings);
            LoadExternalSettingsFromDb();

            ValidateAppModes();
        }

        protected virtual void LoadInternalSettings()
        {
            settings = new Dictionary<string, object>
            {
                { "TestSetting", "XXX" }
            };
        }

        protected virtual void LoadExternalSettingsFromFile(IApplicationConfiguration fileBasedAppSettings)
        {
            // Merge file-based appsettings.json (external) settings with internal (hard-coded)
            var appExternalSettings = (fileBasedAppSettings.Configuration
                // Load from: Infrastructure/Externals/appsettings.json
                .GetSection("AppExternalSettings"))
                .GetChildren();

            foreach (var conf in appExternalSettings)
            {
                settings.Add(conf.Key, conf.Value);
            }
        }

        protected virtual void LoadExternalSettingsFromDb()
        {
            var appSettings = unitOfWork.Repository<AppSetting>().GetAllAsync().Result;

            foreach (var setting in appSettings)
            {
                settings.Add(setting.Key, setting.Value);
            }
        }

        private void ValidateAppModes()
        {
            string[] supportedAppModes = System.Enum.GetNames(typeof(AppModes));
            string validAppMode = supportedAppModes
                .FirstOrDefault(mode => mode.Equals(Environment, StringComparison.InvariantCultureIgnoreCase));

            if (validAppMode is null)
            {
                throw new NotImplementedException($"Configured Environment `{ Environment }` is not valid. " +
                    $"Supported app modes are: { string.Join(',', supportedAppModes) }. " +
                    $"Please verify the validity of appsettings.json");
            }
        }

        public Dictionary<string, object> AppSettings
        {
            get
            {
                return settings;
            }
        }

        private string Environment { get => settings["Environment"].ToString().ToLower(); }

        public bool IsDevMode()
        {
            return Environment.Equals(AppModes.Development.ToString(), StringComparison.InvariantCultureIgnoreCase);
        }

        internal bool IsProdMode()
        {
            return Environment.Equals(AppModes.Production.ToString(), StringComparison.InvariantCultureIgnoreCase);
        }
    }
}

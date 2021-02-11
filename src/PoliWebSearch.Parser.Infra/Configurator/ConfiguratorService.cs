﻿using Newtonsoft.Json;
using System.IO;

namespace PoliWebSearch.Parser.Infra.Configurator
{
    /// <summary>
    /// Implementation of IConfigurationService
    /// </summary>
    public class ConfiguratorService : IConfiguratorService
    {
        private AppConfigurationData appConfiguration;

        public AppConfigurationData AppConfig => appConfiguration;

        // <inheritdoc/>
        public void Initialize(string envDir)
        {
            appConfiguration = new AppConfigurationData();
            appConfiguration = JsonConvert.DeserializeObject<AppConfigurationData>(File.ReadAllText(Path.Combine(envDir, "appConfig.json")));
            appConfiguration.EnvDirectory = envDir;
            appConfiguration.StorageDirectory = Path.Join(envDir, "storage");
            appConfiguration.LogDirectory = Path.Join(envDir, "log");
        }
    }
}
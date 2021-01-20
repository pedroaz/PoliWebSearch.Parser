using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace PoliWebSearch.Parser.Shared.Configurator
{
    public class ConfiguratorService : IConfiguratorService
    {
        private AppConfigurationData appConfiguration;

        public AppConfigurationData AppConfig => appConfiguration;

        public void Initialize(string envDir)
        {
            appConfiguration = new AppConfigurationData();
            appConfiguration.EnvDirectory = envDir;
            appConfiguration.StorageDirectory = Path.Join(envDir, "storage");
        }
    }
}

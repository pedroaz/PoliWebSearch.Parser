using Newtonsoft.Json;
using PoliWebSearch.Parser.Infra.Services.IO;
using PoliWebSearch.Parser.Infra.Services.Log;
using System;
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
            appConfiguration.ResultsDirectory = Path.Join(envDir, "results");
            CreateDirectories();
            SetExecutionId();
        }

        private void CreateDirectories()
        {
            Directory.CreateDirectory(appConfiguration.LogDirectory);
            Directory.CreateDirectory(appConfiguration.ResultsDirectory);
        }

        private void SetExecutionId()
        {
            int amountOfFilesInLogFolder = Directory.GetFiles(appConfiguration.LogDirectory).Length;
            int amountOfFilesInResultFolder = Directory.GetFiles(appConfiguration.ResultsDirectory).Length;
            int maxIndex = Math.Max(amountOfFilesInResultFolder, amountOfFilesInLogFolder) + 1;
            appConfiguration.ExecutionId = maxIndex.ToString();
            LogService.Print($"*** Execution Id:  [{appConfiguration.ExecutionId}] ***");
        }
    }
}

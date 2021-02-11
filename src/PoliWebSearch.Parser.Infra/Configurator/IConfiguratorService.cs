namespace PoliWebSearch.Parser.Infra.Configurator
{
    /// <summary>
    /// Handles all application configuration
    /// </summary>
    public interface IConfiguratorService
    {
        /// <summary>
        /// Initializae all the configuration from file and envDir
        /// </summary>
        /// <param name="envDir"></param>
        void Initialize(string envDir);

        /// <summary>
        /// Get the application configuration
        /// </summary>
        AppConfigurationData AppConfig { get; }
    }
}

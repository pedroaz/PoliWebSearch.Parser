namespace PoliWebSearch.Parser.Shared.Configurator
{
    public interface IConfiguratorService
    {
        void Initialize(string envDir);

        AppConfigurationData AppConfig { get; }
    }
}

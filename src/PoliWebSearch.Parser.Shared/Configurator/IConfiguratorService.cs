using System;
using System.Collections.Generic;
using System.Text;

namespace PoliWebSearch.Parser.Shared.Configurator
{
    public interface IConfiguratorService
    {
        void Initialize(string envDir);

        AppConfigurationData AppConfig { get; }
    }
}

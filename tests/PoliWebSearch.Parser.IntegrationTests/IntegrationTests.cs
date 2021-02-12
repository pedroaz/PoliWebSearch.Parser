using PoliWebSearch.Parser.ConsoleApp;
using System;
using Xunit;

namespace PoliWebSearch.Parser.IntegrationTests
{
    public class IntegrationTests
    {
        [Fact]
        public void RunApplication()
        {
            Program.Main(new string[] { "../../../IntegrationTestAppEnv", "admin --operation=count" });
        }
    }
}

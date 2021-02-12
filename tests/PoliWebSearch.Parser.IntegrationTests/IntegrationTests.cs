using FluentAssertions;
using Newtonsoft.Json;
using PoliWebSearch.Parser.ConsoleApp;
using PoliWebSearch.Parser.Infra.Services.Result;
using System;
using System.Collections.Generic;
using System.IO;
using Xunit;

namespace PoliWebSearch.Parser.IntegrationTests
{
    public class IntegrationTests
    {

        private static string EnvDir = "../../../IntegrationTestAppEnv";
        private static string LogDir = Path.Join(EnvDir, "log");
        private static string ResultsDir = Path.Join(EnvDir, "results");

        public IntegrationTests()
        {

            var directoriesToCheck = new List<string>() {
                EnvDir
            };

            var filesToCheck = new List<string>() {
                Path.Join(EnvDir, "/Storage/Tse/Candidatos/consulta_cand_2020_SP.csv"),
                Path.Join(EnvDir, "appConfig.json")
            };


            foreach (var dir in directoriesToCheck) {
                if (!Directory.Exists(dir)) {
                    throw new Exception($"{dir} should exist before executing integration test");
                }
            }

            foreach (var file in filesToCheck) {
                if (!File.Exists(file)) {
                    throw new Exception($"{file} should exist before executing integration test");
                }
            }

            ClearDirectories();
        }

        private static void ClearDirectories()
        {
            ClearDir(LogDir);
            ClearDir(ResultsDir);
        }

        private static void ClearDir(string dir)
        {
            if (Directory.Exists(dir)) {
                Directory.Delete(dir, true);
                Directory.CreateDirectory(dir);
            }
        }

        ResultData GetResultData(int index) => JsonConvert.DeserializeObject<ResultData>(File.ReadAllText(Path.Join(ResultsDir, $"{index}_Parser_Result.json")));

        [Fact]
        public void IntegrationTest()
        {

            // 1
            // Arrange
            // Act
            int index = Program.Main(new string[] { EnvDir, "admin --operation=drop", "admin --operation=count"});
            // Assert
            var result = GetResultData(index);
            result.GraphCounts[0].Should().Be(0);

            // 2
            // Act
            index = Program.Main(new string[] { EnvDir, "tse --source=candidatos --dropfirst", "admin --operation=count" });
            // Assert
            result = GetResultData(index);
            result.GraphCounts[0].Should().BeGreaterThan(100);


            // 3
            // Arrange
            // Act
            index = Program.Main(new string[] { EnvDir, "admin --operation=drop", "admin --operation=count" });
            // Assert
            result = GetResultData(index);
            result.GraphCounts[0].Should().Be(0);

        }
    }
}

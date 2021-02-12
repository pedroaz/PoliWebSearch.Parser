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

            // 1 - Clear the database and guarantee it's clear
            // Arrange
            // Act
            int index = Program.Main(new string[] { EnvDir, "admin --operation=drop", "admin --operation=count"});
            // Assert
            var result = GetResultData(index);
            result.GraphCounts[0].Should().Be(0);

            // 2 - Execute subset of candidatos file
            // Act

            var query = "g.V().has('Cpf','29422644895')";

            index = Program.Main(new string[] { EnvDir, 
                "tse --source=candidatos --dropfirst", 
                "admin --operation=count",
                $"admin --operation=custom --query={query}"
            });
            // Assert
            result = GetResultData(index);
            result.GraphCounts[0].Should().BeGreaterThan(100);

            var model = result.QueryResults[query];
            model[0].properties["Cpf"][0].value.Should().Be("29422644895");
            model[0].properties["Emails"][0].value.Should().Be("ELEICOES2020MUNICIPAL@GMAIL.COM");


            // 3 - Clear the database again and guarantee it's clear
            // Arrange
            // Act
            index = Program.Main(new string[] { EnvDir, "admin --operation=drop", "admin --operation=count" });
            // Assert
            result = GetResultData(index);
            result.GraphCounts[0].Should().Be(0);

        }
    }
}

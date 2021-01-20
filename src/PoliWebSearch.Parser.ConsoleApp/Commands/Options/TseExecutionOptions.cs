using CommandLine;
using PoliWebSearch.Parser.Tse.Service;
using System;
using System.Collections.Generic;
using System.Text;

namespace PoliWebSearch.Parser.ConsoleApp.Commands.Options
{
    [Verb("tse", HelpText = "Commands Related to Tse operations")]
    public class TseExecutionOptions
    {
        [Option(Default = "", Required = true, HelpText = "Source types: [candidatos|resultados].")]
        public TseDataSourceType source { get; set; }
    }
}

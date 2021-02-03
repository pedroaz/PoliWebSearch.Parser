﻿using CommandLine;
using PoliWebSearch.Parser.Tse.Service;

namespace PoliWebSearch.Parser.ConsoleApp.Commands.Options
{
    [Verb("tse", HelpText = "Commands Related to Tse operations")]
    public class TseExecutionOptions
    {
        [Option(Default = "", Required = true, HelpText = "Source types: [candidatos|resultados].")]
        public TseDataSourceType source { get; set; }

        [Option(Default = 0, Required = false, HelpText = "Row limit from file: [0 to ignore].")]
        public long? rowlimit { get; set; }
    }
}

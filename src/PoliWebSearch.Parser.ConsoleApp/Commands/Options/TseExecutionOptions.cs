using CommandLine;
using PoliWebSearch.Parser.FileParsers.Tse.Service;

namespace PoliWebSearch.Parser.ConsoleApp.Commands.Options
{
    /// <summary>
    /// Tribunal Superior Eleitoral Execution Options
    /// </summary>
    [Verb("tse", HelpText = "Commands Related to Tse operations")]
    public class TseExecutionOptions
    {
        /// <summary>
        /// What is the source of your data
        /// </summary>
        [Option(Default = "", Required = true, HelpText = "Source types: [candidatos|resultados].")]
        public TseDataSourceType source { get; set; }

        /// <summary>
        /// Maximum amount of rows that should be processed.
        /// Useful for testing
        /// </summary>
        [Option(Default = 0, Required = false, HelpText = "Row limit from file. 0 to ignore.")]
        public int rowlimit { get; set; }

        /// <summary>
        /// Should the operation drop the entire database before inserting data
        /// Useful for testing
        /// </summary>
        [Option(Default = false, Required = false, HelpText = "If the database should be droped first.")]
        public bool dropfirst { get; set; }
    }
}

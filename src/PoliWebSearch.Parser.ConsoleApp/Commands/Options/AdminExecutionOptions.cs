using CommandLine;
using PoliWebSerach.Parser.DB.Services.Admin;

namespace PoliWebSearch.Parser.ConsoleApp.Commands.Options
{
    /// <summary>
    /// Administrator Console Options
    /// </summary>
    [Verb("admin", HelpText = "Commands Related to Admin operations")]
    public class AdminExecutionOptions
    {
        /// <summary>
        /// Which operation will be executed
        /// </summary>
        [Option(Default = "", Required = true, HelpText = "Admin operations: [count|drop].")]
        public AdminOperations operation { get; set; }
    }
}

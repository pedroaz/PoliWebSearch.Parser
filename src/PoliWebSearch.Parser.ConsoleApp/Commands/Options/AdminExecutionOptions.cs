using CommandLine;
using PoliWebSerach.Parser.DB.Services.Admin;

namespace PoliWebSearch.Parser.ConsoleApp.Commands.Options
{
    [Verb("admin", HelpText = "Commands Related to Admin operations")]
    public class AdminExecutionOptions
    {
        [Option(Default = "", Required = true, HelpText = "Admin operations: [count|drop].")]
        public AdminOperations operation { get; set; }
    }
}

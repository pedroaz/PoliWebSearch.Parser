using CommandLine;
using PoliWebSearch.Parser.ConsoleApp.Commands.Options;
using PoliWebSearch.Parser.Shared.Services.Log;
using PoliWebSearch.Parser.Tse.Service;
using System;
using System.Threading.Tasks;

namespace PoliWebSearch.Parser.ConsoleApp.Commands
{
    public class CommandsManager : ICommandsManager
    {
        // Commands
        static string currentCommand;

        // Services
        private readonly ILogService logService;
        private readonly ITseParserService tseParserService;

        public CommandsManager(ILogService logService, ITseParserService tseParserService)
        {
            this.logService = logService;
            this.tseParserService = tseParserService;
        }

        public async Task Loop()
        {
            while (true) {

                logService.Log($"Please type a new command. Or type --help if you don't know what you are doing");

                // Read
                currentCommand = Console.ReadLine();
                logService.Log($"Command recieved: {currentCommand}");

                // Exit
                if (currentCommand == "exit") {
                    break;
                }

                // Actions
                var splittedCommand = currentCommand.Split(" ");
                var result = await CommandLine.Parser.Default.ParseArguments<TseExecutionOptions, PdtExecutionOptions>(splittedCommand)
                    .MapResult(
                        (TseExecutionOptions opts) => ExecuteTse(opts),
                        _ => Task.FromResult(1)
                    );
            }
        }

        private async Task<int> ExecuteTse(TseExecutionOptions opts)
        {
            logService.Log($"Executing TSE Parser");
            await tseParserService.ParseFiles(opts.source);
            return 0;
        }
    }
}

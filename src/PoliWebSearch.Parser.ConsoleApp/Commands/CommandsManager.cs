using CommandLine;
using PoliWebSearch.Parser.ConsoleApp.Commands.Options;
using PoliWebSearch.Parser.FileParsers.Tse.Service;
using PoliWebSearch.Parser.Shared.Services.Log;
using PoliWebSerach.Parser.DB.Services.Admin;
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
        private readonly IAdminService adminService;

        public CommandsManager(ILogService logService, ITseParserService tseParserService,
            IAdminService adminService)
        {
            this.logService = logService;
            this.tseParserService = tseParserService;
            this.adminService = adminService;
        }

        public async Task Loop()
        {
            while (true) {

                logService.Log($"Please type a new command. Or type --help if you don't know what you are doing");

                currentCommand = Console.ReadLine();
                logService.Log($"Command recieved: {currentCommand}");

                if (currentCommand == "exit") {
                    break;
                }

                //await Execute(new TseExecutionOptions() {
                //    dropfirst = false,
                //    rowlimit = 10,
                //    source = TseDataSourceType.candidatos
                //});
                //break;
                
                var splittedCommand = currentCommand.Split(" ");
                var result = await CommandLine.Parser.Default.ParseArguments<TseExecutionOptions, PdtExecutionOptions, AdminExecutionOptions>(splittedCommand)
                    .MapResult(
                        (TseExecutionOptions options) => Execute(options),
                        (AdminExecutionOptions options) => Execute(options),
                        _ => Task.FromResult(1)
                    );
            }
        }

        private async Task<int> Execute(AdminExecutionOptions options)
        {
            logService.Log($"Executing Admin Operation: {options.operation}");
            switch (options.operation) {
                case AdminOperations.count:
                    await adminService.CountDatabase();
                    break;
                case AdminOperations.drop:
                    await adminService.DropDatabase();
                    break;
            }
            return 0;
        }

        private async Task<int> Execute(TseExecutionOptions options)
        {
            logService.Log($"Executing TSE Parser");
            await tseParserService.ParseFiles(options.source, options.rowlimit, options.dropfirst);
            return 0;
        }
    }
}

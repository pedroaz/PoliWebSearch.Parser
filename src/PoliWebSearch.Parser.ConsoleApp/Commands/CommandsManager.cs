﻿using CommandLine;
using PoliWebSearch.Parser.ConsoleApp.Commands.Options;
using PoliWebSearch.Parser.FileParsers.Tse.Service;
using PoliWebSearch.Parser.Shared.Services.Log;
using PoliWebSerach.Parser.DB.Services.Admin;
using System;
using System.Threading.Tasks;

namespace PoliWebSearch.Parser.ConsoleApp.Commands
{
    /// <summary>
    /// Implementation of the ICommandsManager interface
    /// </summary>
    public class CommandsManager : ICommandsManager
    {
        // Commands
        static string currentCommand;

        // Services
        private readonly ILogService logService;
        private readonly ITseService tseService;
        private readonly IAdminService adminService;

        public CommandsManager(ILogService logService, ITseService tseParserService,
            IAdminService adminService)
        {
            this.logService = logService;
            this.tseService = tseParserService;
            this.adminService = adminService;
        }

        // <inheritdoc/>
        public async Task Loop()
        {
            while (true) {

                logService.Log($"Please type a new command. Or type --help if you don't know what you are doing", LogType.Loop);

                currentCommand = Console.ReadLine();
                logService.Log($"Command recieved: {currentCommand}", LogType.Loop);

                if (currentCommand == "exit") {
                    break;
                }

                var splittedCommand = currentCommand.Split(" ");
                var result = await CommandLine.Parser.Default.ParseArguments<TseExecutionOptions, PdtExecutionOptions, AdminExecutionOptions>(splittedCommand)
                    .MapResult(
                        (TseExecutionOptions options) => Execute(options),
                        (AdminExecutionOptions options) => Execute(options),
                        _ => Task.FromResult(1)
                    );
            }
        }


        /// <summary>
        /// Executes Admin Operations
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        private async Task<int> Execute(AdminExecutionOptions options)
        {
            logService.Log($"Executing Admin Operation: {options.operation}", LogType.Loop);
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

        /// <summary>
        /// Executes TSE operations
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        private async Task<int> Execute(TseExecutionOptions options)
        {
            logService.Log($"Executing TSE Parser", LogType.Loop);
            await tseService.ParseFiles(options.source, options.rowlimit, options.dropfirst);
            return 0;
        }
    }
}

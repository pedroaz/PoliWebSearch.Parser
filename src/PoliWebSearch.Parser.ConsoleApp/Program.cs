﻿using Autofac;
using PoliWebSearch.Parser.ConsoleApp.Arguments;
using PoliWebSearch.Parser.ConsoleApp.Commands;
using PoliWebSearch.Parser.FileParsers.Tse.Candidate;
using PoliWebSearch.Parser.FileParsers.Tse.Service;
using PoliWebSearch.Parser.Infra.Configurator;
using PoliWebSearch.Parser.Infra.Resolver;
using PoliWebSearch.Parser.Infra.Services.Clock;
using PoliWebSearch.Parser.Infra.Services.File;
using PoliWebSearch.Parser.Infra.Services.Log;
using PoliWebSerach.Parser.DB.Operator;
using PoliWebSerach.Parser.DB.Services.Admin;
using PoliWebSerach.Parser.DB.Services.Database;
using System;
using System.Threading.Tasks;

namespace PoliWebSearch.Parser.ConsoleApp
{
    /// <summary>
    /// Static Program class
    /// </summary>
    public static class Program
    {
        // Services 
        private static IContainer container;
        private static ILogService logService;
        private static ICommandsManager commandsManager;
        private static IConfiguratorService configuratorService;

        /// <summary>
        /// Main entry point for the application
        /// </summary>
        /// <param name="args">Should recieve the App Env</param>
        public static void Main(string[] args)
        {
            try {
                MainAsync(args).Wait();
            }
            catch (Exception e) {
                Console.WriteLine(e.Message);
                throw e;
            }
        }

        /// <summary>
        /// Main Async method of the application
        /// </summary>
        /// <param name="args">Should recieve the App Env</param>
        /// <returns></returns>
        private async static Task MainAsync(string[] args)
        {
            LogService.Print("*** Starting Poli Web Search ***");
            LogService.Print("*** Initializing intefaces ***");
            RegisterInterfaces();
            var commandArgs = new ProgramArguments(args);
            if (!commandArgs.HasEnvFolder) {
                throw new Exception("Need to pass env folder for the app to execute");
            }
            
            ResolveInterfaces();
            InitializeIntefaces(commandArgs.EnvFolder);
            
            if (commandArgs.HasCommand) {
                await commandsManager.ExecuteSingleCommand(commandArgs.Command);
            }
            else {
                await commandsManager.Loop();
            }
            LogService.Print("*** Finished console app ***");
        }

        /// <summary>
        /// Register all interfaces on the container
        /// </summary>
        private static void RegisterInterfaces()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<LogService>().As<ILogService>().SingleInstance();
            builder.RegisterType<TseService>().As<ITseService>().SingleInstance();
            builder.RegisterType<CommandsManager>().As<ICommandsManager>().SingleInstance();
            builder.RegisterType<ConfiguratorService>().As<IConfiguratorService>().SingleInstance();
            builder.RegisterType<TseCandidatesFileParser>().As<ITseCandidatesFileParser>().SingleInstance();
            builder.RegisterType<FileService>().As<IFileService>().SingleInstance();
            builder.RegisterType<ClockService>().As<IClockService>().SingleInstance();
            builder.RegisterType<DatabaseService>().As<IDatabaseService>().SingleInstance();
            builder.RegisterType<DatabaseOperator>().As<IDatabaseOperator>();
            builder.RegisterType<ServiceResolver>().As<IServiceResolver>().SingleInstance();
            builder.RegisterType<AdminService>().As<IAdminService>().SingleInstance();
            builder.RegisterType<TseCandidateParser>().As<ITseCandidateParser>().SingleInstance();
            container = builder.Build();
        }

        /// <summary>
        /// Resolve interfaces used by the console application
        /// </summary>
        private static void ResolveInterfaces()
        {
            logService = container.Resolve<ILogService>();
            commandsManager = container.Resolve<ICommandsManager>();
            configuratorService = container.Resolve<IConfiguratorService>();
        }

        /// <summary>
        /// Initialize the interfaces which need initialization
        /// </summary>
        /// <param name="envPath"></param>
        private static void InitializeIntefaces(string envPath)
        {
            configuratorService.Initialize(envPath);
            logService.Initialize();
            logService.Log("Log service and configurator initialized correctly", LogType.Initialization);
            container.Resolve<IServiceResolver>().Initialize(container);
            container.Resolve<IDatabaseService>().Initialize();
            logService.Log("Other interfaces initialized correctly", LogType.Initialization);
        }

    }
}

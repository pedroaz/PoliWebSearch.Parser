using Autofac;
using PoliWebSearch.Parser.ConsoleApp.Commands;
using PoliWebSearch.Parser.FileParsers.Tse.FileParser.Candidates;
using PoliWebSearch.Parser.FileParsers.Tse.Service;
using PoliWebSearch.Parser.Shared.Configurator;
using PoliWebSearch.Parser.Shared.Resolver;
using PoliWebSearch.Parser.Shared.Services.Clock;
using PoliWebSearch.Parser.Shared.Services.File;
using PoliWebSearch.Parser.Shared.Services.Log;
using PoliWebSerach.Parser.DB.Operator;
using PoliWebSerach.Parser.DB.Services.Admin;
using PoliWebSerach.Parser.DB.Services.Database;
using System;
using System.Threading.Tasks;

namespace PoliWebSearch.Parser.ConsoleApp
{
    static class Program
    {
        // Services 
        private static IContainer container;
        private static ILogService logService;
        private static ICommandsManager commandsManager;
        private static IConfiguratorService configuratorService;

        static void Main(string[] args)
        {

            try {
                MainAsync(args).Wait();
            }
            catch (Exception e) {
                Console.WriteLine(e.Message);
                throw;
            }
        }

        private async static Task MainAsync(string[] args)
        {
            LogService.Print("*** Starting Poli Web Search ***");
            LogService.Print("*** Initializing intefaces ***");
            RegisterInterfaces();
            string envPath = GetEnvPathFromArgs(args);
            if (envPath.Equals(string.Empty)) {
                LogService.Print("*** Need to pass env folder for the app to execute ***");
                return;
            }

            ResolveInterfaces();
            InitializeIntefaces(envPath);
            await commandsManager.Loop();
            LogService.Print("*** Finished console app ***");
        }

        private static string GetEnvPathFromArgs(string[] args)
        {
            if (args.Length != 1) {
                return "";
            }
            else {
                return args[0];
            }
        }

        private static void RegisterInterfaces()
        {
            var builder = new ContainerBuilder();
            builder.RegisterType<LogService>().As<ILogService>().SingleInstance();
            builder.RegisterType<TseParserService>().As<ITseParserService>().SingleInstance();
            builder.RegisterType<CommandsManager>().As<ICommandsManager>().SingleInstance();
            builder.RegisterType<ConfiguratorService>().As<IConfiguratorService>().SingleInstance();
            builder.RegisterType<TseCandidatesFileParser>().As<ITseCandidatesFileParser>().SingleInstance();
            builder.RegisterType<FileService>().As<IFileService>().SingleInstance();
            builder.RegisterType<ClockService>().As<IClockService>().SingleInstance();
            builder.RegisterType<DatabaseService>().As<IDatabaseService>().SingleInstance();
            builder.RegisterType<DatabaseOperator>().As<IDatabaseOperator>();
            builder.RegisterType<ServiceResolver>().As<IServiceResolver>().SingleInstance();
            builder.RegisterType<AdminService>().As<IAdminService>().SingleInstance();
            container = builder.Build();
        }

        private static void ResolveInterfaces()
        {
            logService = container.Resolve<ILogService>();
            commandsManager = container.Resolve<ICommandsManager>();
            configuratorService = container.Resolve<IConfiguratorService>();
        }

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

using Gremlin.Net.Driver;
using Gremlin.Net.Driver.Exceptions;
using Gremlin.Net.Structure.IO.GraphSON;
using Newtonsoft.Json;
using PoliWebSearch.Parser.Shared.Configurator;
using PoliWebSearch.Parser.Shared.Models;
using PoliWebSearch.Parser.Shared.Resolver;
using PoliWebSerach.Parser.DB.Operator;
using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Threading.Tasks;

namespace PoliWebSerach.Parser.DB.Services
{
    public class DatabaseService : IDatabaseService
    {
        private GremlinServer server;
        private readonly IServiceResolver serviceResolver;
        private readonly IConfiguratorService configuratorService;

        public DatabaseService(IServiceResolver serviceResolver, IConfiguratorService configuratorService)
        {
            this.serviceResolver = serviceResolver;
            this.configuratorService = configuratorService;
        }

        public void Initialize()
        {
            
            var userName = $"/dbs/{configuratorService.AppConfig.DatabaseName}/colls/{configuratorService.AppConfig.GraphName}";
            server = new GremlinServer(configuratorService.AppConfig.HostName, 443, true, userName, configuratorService.AppConfig.MasterKey);
            var databaseOperator1 = serviceResolver.ResolveService<IDatabaseOperator>();
            var databaseOperator2 = serviceResolver.ResolveService<IDatabaseOperator>();
        }

        
        public async Task AddListOfObjects<T>(List<T> list)
        {
            
        }
    }
}

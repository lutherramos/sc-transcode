using System;
using System.Linq;
using SC.Core.Lookuplist;
using SC.Core.Logging.Model;
using System.Threading.Tasks;
using SC.Core.Logging.Tracing;
using System.Collections.Generic;
using SC.Core.Data.Model.Utility;
using Microsoft.Extensions.Hosting;
using SC.Core.ConfigurationManagement;
using Microsoft.Extensions.DependencyInjection;

namespace SC.SC.Transcode.WorkerProcess
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    DBBAppConfiguration dBBAppConfiguration = DBBConfiguration.GetConfigValues();
                    dBBAppConfiguration.dBBCommonConfiguration.schedulerServiceInvocationID = LookupLists.SystemIntegrationServiceType.Values.Transcode.ID;
                    TraceService traceService = new TraceService(dBBAppConfiguration.dBBCommonConfiguration.MSSQLConnectionString, TraceLevel.Database, LookupLists.SystemIntegrationServiceType.Values.Transcode.SystemName);

                    Initialize(dBBAppConfiguration, traceService);

                    services.AddHostedService<ExecutionWorker>(w => new ExecutionWorker(dBBAppConfiguration, traceService));

                    //not using this... it doesn't make sense!
                    //services.AddHostedService<StatusWorker>(w => new StatusWorker(dBBAppConfiguration, traceService));
                });


        public static void Initialize(DBBAppConfiguration dBBAppConfiguration, TraceService traceService)
        {
            Environment.SetEnvironmentVariable("TraceLevel", dBBAppConfiguration.defaultAppConfiguration.GetSection("app:TraceLevel").Value);
            Environment.SetEnvironmentVariable("InstanceTimeoutInSeconds", dBBAppConfiguration.defaultAppConfiguration.GetSection("app:TimeoutInSeconds").Value);
            Environment.SetEnvironmentVariable("ConnectionString", dBBAppConfiguration.dBBCommonConfiguration.MSSQLConnectionString);

            IServiceCollection serviceCollection = IOCFactory.Initializer;
            serviceCollection.AddScoped<ITraceService>(Pro => traceService);
        }
    }
}

using System;
using NUnit.Framework;
using SC.Core.Lookuplist;
using SC.Core.Logging.Model;
using SC.Core.Logging.Tracing;
using SC.Core.Data.Model.Utility;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SC.SC.Transcode.Service;
using System.Threading.Tasks;

namespace SC.SC.Transcode.Test
{
    public class TranscodeUnitTest
    {
        [SetUp]
        public void Setup()
        {
            string env = "stg";
            var config = new ConfigurationBuilder()
                                .AddJsonFile("appsettings.json", optional: true)
                                .AddSystemsManager("/supplychain/" + env)
                                .Build();

            string Connectionstring = config.GetSection("db:mssqlconnectionstring").Value;
            Environment.SetEnvironmentVariable("TraceLevel", config.GetSection("app:TraceLevel").Value);
            Environment.SetEnvironmentVariable("InstanceTimeoutInSeconds", config.GetSection("app:TimeoutInSeconds").Value);
            Environment.SetEnvironmentVariable("ConnectionString", Connectionstring);

            IServiceCollection serviceCollection = IOCFactory.Initializer;
            TraceService traceService = new TraceService(Connectionstring, TraceLevel.Database, LookupLists.SystemIntegrationServiceType.Values.Transcode.SystemName);
            serviceCollection.AddScoped<ITraceService>(Pro => traceService);

        }

        [Test]
        public async Task ValidateAndExecute()
        {
            var ExecutionstepID = new Guid("D84DCB17-AE7D-EA11-82A8-A78CC897533A");

            TransocdeService transocdeService = new TransocdeService();
            var executionStep = await transocdeService.Execute(ExecutionstepID);
            
            Assert.AreEqual(executionStep.ExecutionStepStatusTypeId, new Guid("D1BD4AC3-FB5B-4F10-85AE-29BECA79E92F"));
        }
    }
}
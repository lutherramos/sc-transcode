using SC.Core.ConfigurationManagement;
using SC.Core.Logging.Model;
using SC.Core.WorkerProcess;
using SC.Core.WorkerProcess.Model;
using SC.SC.Transcode.Service;
using Sony.DBB.SC.WorkerProcess.Core;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SC.SC.Transcode.WorkerProcess
{
    public class StatusWorker : StatusSQSConsumer<SQSAttribute>, ISQSWorker<SQSAttribute>
    {
        #region Constructor
        public StatusWorker(DBBAppConfiguration dBBAppConfiguration, ITraceService traceService, ConsumerHandlerConfig handlerConfig = null, int asyncMessageProcessCount = 1)
               : base(dBBAppConfiguration.dBBCommonConfiguration.MSSQLConnectionString, dBBAppConfiguration.dBBCommonConfiguration.appSettingMaintenanceModeID, dBBAppConfiguration.dBBCommonConfiguration.schedulerServiceInvocationID, traceService, dBBAppConfiguration.dBBCommonConfiguration.traceLevel, handlerConfig, asyncMessageProcessCount)
        {
            _traceService = traceService;
        }
        #endregion

        public override async Task<bool> ProcessRunAsync(SQSAttribute message, string messageKey, TraceContext traceContext)
        {
            string methodName = MethodInfo.GetCurrentMethod().ReflectedType.FullName + "." + MethodInfo.GetCurrentMethod().Name;
            try
            {
                TransocdeService transcode = new TransocdeService();
                _traceService.Write(traceContext, TraceLevel.Error, methodName, string.Format(TraceHelp.TraceMessageFormat, TraceHelp.Info, "Transocde Service as GetStatus for Executionstep: ", message.ExecutionStepID));
                await transcode.GetStatus(message.ExecutionStepID);

            }
            catch (Exception ex)
            {
                _traceService.Write(traceContext, TraceLevel.Error, methodName, ex);
            }
            return true;
        }
    }
}

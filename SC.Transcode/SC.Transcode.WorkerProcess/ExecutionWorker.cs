using System;
using System.Reflection;
using SC.Core.Logging.Model;
using SC.Core.WorkerProcess;
using System.Threading.Tasks;
using SC.Core.WorkerProcess.Model;
using Sony.DBB.SC.WorkerProcess.Core;
using SC.Core.ConfigurationManagement;
using SC.SC.Transcode.Service;
using Newtonsoft.Json;
using SC.Core.ExecutionService.Model;

namespace SC.SC.Transcode.WorkerProcess
{
    public class ExecutionWorker : ExecuteSQSConsumer<SQSAttribute>, ISQSWorker<SQSAttribute>
    {

        #region Constructor
        public ExecutionWorker(DBBAppConfiguration dBBAppConfiguration, ITraceService traceService, ConsumerHandlerConfig handlerConfig = null, int asyncMessageProcessCount = 1)
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
                _traceService.Write(traceContext, TraceLevel.Error, methodName, string.Format(TraceHelp.TraceMessageFormat, TraceHelp.Info, "Begin of Transcode Service in Worker for Executionstep: ", message.ExecutionStepID));

                TransocdeService transcode = new TransocdeService();
                switch (message.ProcessType)
                {
                    case "Launch":
                        _traceService.Write(traceContext, TraceLevel.Error, methodName, string.Format(TraceHelp.TraceMessageFormat, TraceHelp.Info, "Transcode Service as Launcher for Executionstep: ", message.ExecutionStepID));
                        await transcode.ValidateAndExecute(message.ExecutionStepID);
                        break;
                    case "GetStatus":
                        _traceService.Write(traceContext, TraceLevel.Error, methodName, string.Format(TraceHelp.TraceMessageFormat, TraceHelp.Info, "Transcode Service as GetStatus for Executionstep: ", message.ExecutionStepID));
                        await transcode.GetStatus(message.ExecutionStepID);
                        break;
                    case "ReceiveStatus":
                        _traceService.Write(traceContext, TraceLevel.Error, methodName, string.Format(TraceHelp.TraceMessageFormat, TraceHelp.Info, "Transcode Service as ReceiveStatus for Executionstep: ", message.ExecutionStepID));
                        //await transcode.ReceiveStatus(JsonConvert.DeserializeObject<ExternalStatus>(message.message));
                        break;
                    case "PostStatus":
                        _traceService.Write(traceContext, TraceLevel.Error, methodName, string.Format(TraceHelp.TraceMessageFormat, TraceHelp.Info, "Transcode Service as PostStatus for Executionstep: ", message.ExecutionStepID));
                        await transcode.PostStatus(message.ExecutionStepID);
                        break;
                    case "ExceptionStatus":
                        _traceService.Write(traceContext, TraceLevel.Error, methodName, string.Format(TraceHelp.TraceMessageFormat, TraceHelp.Info, "Transcode Service as GetStatus for Executionstep: ", message.ExecutionStepID));
                        await transcode.ProcessException(message.ExecutionStepID);
                        break;
                    default:
                        break;
                }

                _traceService.Write(traceContext, TraceLevel.Error, methodName, string.Format(TraceHelp.TraceMessageFormat, TraceHelp.Info, "Begin of Transcode Service in Worker for Executionstep: ", message.ExecutionStepID));
            }
            catch (Exception ex)
            {
                _traceService.Write(traceContext, TraceLevel.Error, methodName, ex);
            }
            return true;
        }

    }
}

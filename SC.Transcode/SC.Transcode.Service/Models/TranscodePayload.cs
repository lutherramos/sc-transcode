using SC.Core.Data.Sql;
using SC.Core.ExecutionService.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SC.Transcode.Service.Models
{
    public class TranscodePayload
    {
        public String DBBJobId { get; set; }
        public String Provider { get; set; }
        public String StepType { get; set; }
        public Int32 AdminPriority { get; set; }
        public GetTranscodeRequest_Result PayloadRequest { get; set; }
        public List<GetTranscodeAssembly_Result> PayloadAssemblies { get; set; }
        public List<GetTranscodeInputAudioChannel_Result> PayloadComponentAudioChannels { get; set; }
        public List<GetTranscodeInputAudioTrack_Result> PayloadComponentAudioTracks { get; set; }
        public List<GetTranscodeInputFile_Result> PayloadInputFiles { get; set; }
        public List<GetTranscodeOutputAudioChannel_Result> PayloadOutputAudioChannels { get; set; }
        public List<GetTranscodeOutputAudioTrack_Result> PayloadOutputAudioTracks { get; set; }
        public List<GetTranscodeOutputFile_Result> PayloadOutputFiles { get; set; }
        public List<GetTranscodeOutputSubtitle_Result> PayloadOutputSubtitles { get; set; }
        public List<GetTranscodeOutputVideo_Result> PayloadOutputVideos { get; set; }
        public List<GetTranscodeSubtitle_Result> PayloadComponentSubtitles { get; set; }
        public List<GetTranscodeTimecode_Result> PayloadTimecodes { get; set; }
        public List<GetTranscodeVideo_Result> PayloadComponentVideos { get; set; }
        public Guid? PayloadExecutionStepParent { get; set; }

        private TranscodePayload(Guid DBBJobId, String Provider, String StepType, Int32 AdminPriority)
        {
            this.DBBJobId = DBBJobId.ToString();
            this.Provider = Provider;
            this.StepType = StepType;
            this.AdminPriority = AdminPriority;

            //var dataConfig = (DatabaseSettings)ConfigurationManager.GetSection("dataConfiguration");
            //string connectionString = ConfigurationManager.ConnectionStrings[dataConfig.DefaultDatabase].ConnectionString;
            //DynamicDataUtility dataUtility = new DynamicDataUtility(connectionString, new ReflectionParameterBuilder());

            //PayloadRequest = dataUtility.ExecQueryString(String.Format("exec VAL.GetTranscodeRequestWithVine @DBBJobID = '{0}'", DBBJobId)).FirstOrDefault();
            //PayloadAssemblies = dataUtility.ExecQueryString(String.Format("exec VAL.GetTranscodeAssembly @DBBJobID = '{0}'", DBBJobId)).ToList();
            //PayloadComponentAudioChannels = dataUtility.ExecQueryString(String.Format("exec VAL.GetTranscodeInputAudioChannel @DBBJobID = '{0}'", DBBJobId)).ToList();
            //PayloadComponentAudioTracks = dataUtility.ExecQueryString(String.Format("exec VAL.GetTranscodeInputAudioTrack @DBBJobID = '{0}'", DBBJobId)).ToList();
            //PayloadInputFiles = dataUtility.ExecQueryString(String.Format("exec VAL.GetTranscodeInputFile @DBBJobID = '{0}'", DBBJobId)).ToList();
            //PayloadOutputAudioChannels = dataUtility.ExecQueryString(String.Format("exec VAL.GetTranscodeOutputAudioChannel @DBBJobID = '{0}'", DBBJobId)).ToList();
            //PayloadOutputAudioTracks = dataUtility.ExecQueryString(String.Format("exec VAL.GetTranscodeOutputAudioTrack @DBBJobID = '{0}'", DBBJobId)).ToList();
            //PayloadOutputFiles = dataUtility.ExecQueryString(String.Format("exec VAL.GetTranscodeOutputFile @DBBJobID = '{0}'", DBBJobId)).ToList();
            //PayloadOutputSubtitles = dataUtility.ExecQueryString(String.Format("exec VAL.GetTranscodeOutputSubtitle @DBBJobID = '{0}'", DBBJobId)).ToList();
            //PayloadOutputVideos = dataUtility.ExecQueryString(String.Format("exec VAL.GetTranscodeOutputVideo @DBBJobID = '{0}'", DBBJobId)).ToList();
            //PayloadComponentSubtitles = dataUtility.ExecQueryString(String.Format("exec VAL.GetTranscodeSubtitle @DBBJobID = '{0}'", DBBJobId)).ToList();
            //PayloadTimecodes = dataUtility.ExecQueryString(String.Format("exec VAL.GetTranscodeTimecode @DBBJobID = '{0}'", DBBJobId)).ToList();
            //PayloadComponentVideos = dataUtility.ExecQueryString(String.Format("exec VAL.GetTranscodeVideo @DBBJobID = '{0}'", DBBJobId)).ToList();

            //var dynPayloadExecutionStepParent = dataUtility.ExecQueryString(String.Format("exec VAL.GetTranscodeExecutionStepParent @DBBJobID = '{0}'", DBBJobId)).FirstOrDefault();
            //if (dynPayloadExecutionStepParent != null)
            //    PayloadExecutionStepParent = dynPayloadExecutionStepParent.ParentJobId;

            //if (PayloadInputFiles != null)
            //{
            //    PayloadInputFiles.ForEach(input => { input.MadeForEye = null; });
            //    var madeForeEyeMapping = PayloadComponentVideos.Select(t => new { t.ComponentID, t.MadeForEye }).Distinct().ToList();
            //    madeForeEyeMapping.ForEach(mapping =>
            //    {
            //        var matched = (from i in PayloadInputFiles
            //                       where i.ComponentID == mapping.ComponentID
            //                       select i).ToList();
            //        matched.ForEach(input =>
            //        {
            //            input.MadeForEye = mapping.MadeForEye;
            //        });
            //    });
            //}
        }

        public override string ToString()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(this);
        }

        public static async Task<TranscodePayload> CreateTranscodePayload(Guid DBBJobId, String Provider, String StepType, Int32 AdminPriority)
        {
            var result = new TranscodePayload(DBBJobId, Provider, StepType, AdminPriority);
            var connectionString = Environment.GetEnvironmentVariable("ConnectionString").ToString();

            var dbbContext = new ExecutionContext(connectionString);
            var sqlHelper = new TranscodeRepository(dbbContext);

            result.PayloadRequest = await sqlHelper.RetrievePayloadRequest(DBBJobId);
            result.PayloadAssemblies = await sqlHelper.RetrievePayloadAssemblies(DBBJobId);
            result.PayloadComponentAudioChannels = await sqlHelper.RetrievePayloadComponentAudioChannels(DBBJobId);
            result.PayloadComponentAudioTracks = await sqlHelper.RetrievePayloadComponentAudioTracks(DBBJobId);
            result.PayloadInputFiles = await sqlHelper.RetrievePayloadInputFiles(DBBJobId);
            result.PayloadOutputAudioChannels = await sqlHelper.RetrievePayloadOutputAudioChannels(DBBJobId);
            result.PayloadOutputAudioTracks = await sqlHelper.RetrievePayloadOutputAudioTracks(DBBJobId);
            result.PayloadOutputFiles = await sqlHelper.RetrievePayloadOutputFiles(DBBJobId);
            result.PayloadOutputSubtitles = await sqlHelper.RetrievePayloadOutputSubtitles(DBBJobId);
            result.PayloadOutputVideos = await sqlHelper.RetrievePayloadOutputVideos(DBBJobId);
            result.PayloadComponentSubtitles = await sqlHelper.RetrievePayloadComponentSubtitles(DBBJobId);
            result.PayloadTimecodes = await sqlHelper.RetrievePayloadTimecodes(DBBJobId);
            result.PayloadComponentVideos = await sqlHelper.RetrievePayloadComponentVideos(DBBJobId);

            result.PayloadExecutionStepParent = await sqlHelper.RetrieveExecutionStepParent(DBBJobId);

            if (result.PayloadInputFiles != null)
            {
                result.PayloadInputFiles.ForEach(input => { input.MadeForEye = null; });
                var madeForeEyeMapping = result.PayloadComponentVideos.Select(t => new { t.ComponentID, t.MadeForEye }).Distinct().ToList();
                madeForeEyeMapping.ForEach(mapping =>
                {
                    var matched = (from i in result.PayloadInputFiles
                                   where i.ComponentID == mapping.ComponentID
                                   select i).ToList();
                    matched.ForEach(input =>
                    {
                        input.MadeForEye = mapping.MadeForEye;
                    });
                });
            }

            return result;
        }
    }
}

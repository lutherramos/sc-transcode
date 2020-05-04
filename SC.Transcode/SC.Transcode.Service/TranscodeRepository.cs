using Microsoft.Data.SqlClient;
using SC.Core.Data.Sql;
using SC.Core.ExecutionService.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SC.Transcode.Service.Models;
using SC.Core.Data.Model;
using System.Linq;

namespace SC.Transcode.Service
{
    public class TranscodeRepository : SqlRepository
    {
        ExecutionContext Context = null;

        public TranscodeRepository(ExecutionContext context) : base(context)
        {
            Context = context;
        }

        public async Task<GetTranscodeRequest_Result> RetrievePayloadRequest(Guid jobID)
        {
            var sqlparmeter = new SqlParameter[]
            {
                     new SqlParameter("@DBBJobID", SqlDbType.UniqueIdentifier)
                     {
                        Direction = ParameterDirection.Input,
                        SqlValue = jobID
                     }
            };

            var result = await Context.Set<GetTranscodeRequest_Result>().FromSqlRaw("VAL.GetTranscodeRequestWithVine @DBBJobID", sqlparmeter).ToListAsync();

            return result.FirstOrDefault();
        }

        public async Task<List<GetTranscodeAssembly_Result>> RetrievePayloadAssemblies(Guid jobID)
        {
            var sqlparmeter = new SqlParameter[]
            {
                     new SqlParameter("@DBBJobID", SqlDbType.UniqueIdentifier)
                     {
                        Direction = ParameterDirection.Input,
                        SqlValue = jobID
                     }
            };

            var result = await Context.Set<GetTranscodeAssembly_Result>().FromSqlRaw("VAL.GetTranscodeAssembly @DBBJobID", sqlparmeter).ToListAsync();

            return result;
        }

        public async Task<List<GetTranscodeInputAudioChannel_Result>> RetrievePayloadComponentAudioChannels(Guid jobID)
        {
            var sqlparmeter = new SqlParameter[]
            {
                     new SqlParameter("@DBBJobID", SqlDbType.UniqueIdentifier)
                     {
                        Direction = ParameterDirection.Input,
                        SqlValue = jobID
                     }
            };

            var result = await Context.Set<GetTranscodeInputAudioChannel_Result>().FromSqlRaw("VAL.GetTranscodeInputAudioChannel @DBBJobID", sqlparmeter).ToListAsync();

            return result;
        }

        public async Task<List<GetTranscodeInputAudioTrack_Result>> RetrievePayloadComponentAudioTracks(Guid jobID)
        {
            var sqlparmeter = new SqlParameter[]
            {
                     new SqlParameter("@DBBJobID", SqlDbType.UniqueIdentifier)
                     {
                        Direction = ParameterDirection.Input,
                        SqlValue = jobID
                     }
            };

            var result = await Context.Set<GetTranscodeInputAudioTrack_Result>().FromSqlRaw("VAL.GetTranscodeInputAudioTrack @DBBJobID", sqlparmeter).ToListAsync();

            return result;
        }

        public async Task<List<GetTranscodeInputFile_Result>> RetrievePayloadInputFiles(Guid jobID)
        {
            var sqlparmeter = new SqlParameter[]
            {
                     new SqlParameter("@DBBJobID", SqlDbType.UniqueIdentifier)
                     {
                        Direction = ParameterDirection.Input,
                        SqlValue = jobID
                     }
            };

            var result = await Context.Set<GetTranscodeInputFile_Result>().FromSqlRaw("VAL.GetTranscodeInputFile @DBBJobID", sqlparmeter).ToListAsync();

            return result;
        }

        public async Task<List<GetTranscodeOutputAudioChannel_Result>> RetrievePayloadOutputAudioChannels(Guid jobID)
        {
            var sqlparmeter = new SqlParameter[]
            {
                     new SqlParameter("@DBBJobID", SqlDbType.UniqueIdentifier)
                     {
                        Direction = ParameterDirection.Input,
                        SqlValue = jobID
                     }
            };

            var result = await Context.Set<GetTranscodeOutputAudioChannel_Result>().FromSqlRaw("VAL.GetTranscodeOutputAudioChannel @DBBJobID", sqlparmeter).ToListAsync();

            return result;
        }

        public async Task<List<GetTranscodeOutputAudioTrack_Result>> RetrievePayloadOutputAudioTracks(Guid jobID)
        {
            var sqlparmeter = new SqlParameter[]
            {
                     new SqlParameter("@DBBJobID", SqlDbType.UniqueIdentifier)
                     {
                        Direction = ParameterDirection.Input,
                        SqlValue = jobID
                     }
            };

            var result = await Context.Set<GetTranscodeOutputAudioTrack_Result>().FromSqlRaw("VAL.GetTranscodeOutputAudioTrack @DBBJobID", sqlparmeter).ToListAsync();

            return result;
        }

        public async Task<List<GetTranscodeOutputFile_Result>> RetrievePayloadOutputFiles(Guid jobID)
        {
            var sqlparmeter = new SqlParameter[]
            {
                     new SqlParameter("@DBBJobID", SqlDbType.UniqueIdentifier)
                     {
                        Direction = ParameterDirection.Input,
                        SqlValue = jobID
                     }
            };

            var result = await Context.Set<GetTranscodeOutputFile_Result>().FromSqlRaw("VAL.GetTranscodeOutputFile @DBBJobID", sqlparmeter).ToListAsync();

            return result;
        }

        public async Task<List<GetTranscodeOutputSubtitle_Result>> RetrievePayloadOutputSubtitles(Guid jobID)
        {
            var sqlparmeter = new SqlParameter[]
            {
                     new SqlParameter("@DBBJobID", SqlDbType.UniqueIdentifier)
                     {
                        Direction = ParameterDirection.Input,
                        SqlValue = jobID
                     }
            };

            var result = await Context.Set<GetTranscodeOutputSubtitle_Result>().FromSqlRaw("VAL.GetTranscodeOutputSubtitle @DBBJobID", sqlparmeter).ToListAsync();

            return result;
        }

        public async Task<List<GetTranscodeOutputVideo_Result>> RetrievePayloadOutputVideos(Guid jobID)
        {
            var sqlparmeter = new SqlParameter[]
            {
                     new SqlParameter("@DBBJobID", SqlDbType.UniqueIdentifier)
                     {
                        Direction = ParameterDirection.Input,
                        SqlValue = jobID
                     }
            };

            var result = await Context.Set<GetTranscodeOutputVideo_Result>().FromSqlRaw("VAL.GetTranscodeOutputVideo @DBBJobID", sqlparmeter).ToListAsync();

            return result;
        }

        public async Task<List<GetTranscodeSubtitle_Result>> RetrievePayloadComponentSubtitles(Guid jobID)
        {
            var sqlparmeter = new SqlParameter[]
            {
                     new SqlParameter("@DBBJobID", SqlDbType.UniqueIdentifier)
                     {
                        Direction = ParameterDirection.Input,
                        SqlValue = jobID
                     }
            };

            var result = await Context.Set<GetTranscodeSubtitle_Result>().FromSqlRaw("VAL.GetTranscodeSubtitle @DBBJobID", sqlparmeter).ToListAsync();

            return result;
        }

        public async Task<List<GetTranscodeTimecode_Result>> RetrievePayloadTimecodes(Guid jobID)
        {
            var sqlparmeter = new SqlParameter[]
            {
                     new SqlParameter("@DBBJobID", SqlDbType.UniqueIdentifier)
                     {
                        Direction = ParameterDirection.Input,
                        SqlValue = jobID
                     }
            };

            var result = await Context.Set<GetTranscodeTimecode_Result>().FromSqlRaw("VAL.GetTranscodeTimecode @DBBJobID", sqlparmeter).ToListAsync();

            return result;
        }

        public async Task<List<GetTranscodeVideo_Result>> RetrievePayloadComponentVideos(Guid jobID)
        {
            var sqlparmeter = new SqlParameter[]
            {
                     new SqlParameter("@DBBJobID", SqlDbType.UniqueIdentifier)
                     {
                        Direction = ParameterDirection.Input,
                        SqlValue = jobID
                     }
            };

            var result = await Context.Set<GetTranscodeVideo_Result>().FromSqlRaw("VAL.GetTranscodeVideo @DBBJobID", sqlparmeter).ToListAsync();

            return result;
        }

        public async Task<Guid?> RetrieveExecutionStepParent(Guid jobID)
        {
            Guid? result = await (
                from esi in Context.Set<ExecutionStep_IONTranscode>()
                join esg in Context.ExecutionStepGroup.Where(a => a.IsActive) on esi.ExecutionStepID equals esg.ChildExecutionStepId
                join pesi in Context.Set<ExecutionStep_IONTranscode>() on esg.ParentExecutionStepId equals pesi.ExecutionStepID
                where esi.ExecutionStep_IONTranscodeID == jobID
                select pesi.ExecutionStep_IONTranscodeID
                ).FirstOrDefaultAsync();

            if (result == Guid.Empty)
                result = null;

            return result;
        }

        public async Task CompleteExecutionStepsForION(Guid executionStepID)
        {
            var sqlparmeter = new SqlParameter[]
            {
                     new SqlParameter("@ExecutionStepID", SqlDbType.UniqueIdentifier)
                     {
                        Direction = ParameterDirection.Input,
                        SqlValue = executionStepID
                     },
                     new SqlParameter("@PersistenceUserID", SqlDbType.UniqueIdentifier)
                     {
                        Direction = ParameterDirection.Input,
                        SqlValue = UserGuidID
                     }
            };

            await Context.Database.ExecuteSqlRawAsync("CompleteExecutionStepsForION @ExecutionStepID, @PersistenceUserID", sqlparmeter);
        }
    }
}

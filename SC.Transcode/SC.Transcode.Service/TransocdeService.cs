using System;
using System.Linq;
using System.Reflection;
using SC.Core.Lookuplist;
using SC.Core.Data.Model;
using SC.Core.Logging.Model;
using System.Threading.Tasks;
using SC.Core.ExecutionService;
using System.Collections.Generic;
using SC.Core.ExecutionService.Model;
using Amazon.SQS;
using Microsoft.EntityFrameworkCore;
using SC.Transcode.Service.Models;
using SC.Core.Data.Sql.Model;
using SC.Core.FileSystemService.Model;
using SC.Core.NamingEngineServiceService;
using System.Threading;
using Refit;
using SC.Transcode.Service;
using System.ServiceModel;
using System.Xml.Serialization;
using System.IO;
using System.Xml;
using SC.Core.Data.Model.Enums;

namespace SC.SC.Transcode.Service
{
    public class TransocdeService : CoreServiceBase
    {
        //todo: move to app settings

        protected Guid isDBBStreamCalcAttributeID { get { return new Guid("298672D0-190C-4915-9C1A-36FCE347307D"); } }
        protected Guid isDBBRuntimeAttributeID { get { return new Guid("62910443-5A0F-492F-9759-74CCB11B6974"); } }
        protected Guid audioChannelCountAttributeID { get { return new Guid("704DA768-E3C3-4A44-A092-3039F8400FB2"); } }
        protected Guid sendToInjectorAttributeID { get { return new Guid("C44DB438-FF8D-40C0-B3D0-CF043A8E1DB1"); } }
        protected Guid jobTargetAttributeID { get { return new Guid("813A09D8-03E3-49D5-B706-C4990D10F39E"); } }
        protected Guid transcodeProcessTypeAttributeID { get { return new Guid("190C957A-4E8F-4229-ACDB-820DFBA49026"); } }

        public TransocdeService() : base()
        {

            sqlHelper.DbbModelCreating += Context_DbbModelCreating;
        }
        protected virtual void Context_DbbModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ExecutionStep_IONTranscode>().HasKey(a => a.ExecutionStep_IONTranscodeID);
            modelBuilder.Entity<ExecutionStep_IONTranscode>().HasQueryFilter(a => a.IsActive);
            modelBuilder.Entity<PackageSpecification>().HasKey(a => a.PackageSpecificationID);
            modelBuilder.Entity<PackageSpecification>().HasQueryFilter(a => a.IsActive);
            modelBuilder.Entity<GetTranscodeRequest_Result>().HasNoKey();
            modelBuilder.Entity<GetTranscodeVideo_Result>().HasNoKey();
            modelBuilder.Entity<GetTranscodeInputAudioTrack_Result>().HasNoKey();
            modelBuilder.Entity<GetTranscodeInputAudioChannel_Result>().HasNoKey();
            modelBuilder.Entity<GetTranscodeOutputAudioTrack_Result>().HasNoKey();
            modelBuilder.Entity<GetTranscodeOutputAudioChannel_Result>().HasNoKey();
            modelBuilder.Entity<GetTranscodeSubtitle_Result>().HasNoKey();
            modelBuilder.Entity<GetTranscodeInputFile_Result>().HasNoKey();
            modelBuilder.Entity<GetTranscodeInputFile_Result>().Ignore(a => a.MadeForEye);
            modelBuilder.Entity<GetTranscodeOutputFile_Result>().HasNoKey();
            modelBuilder.Entity<GetTranscodeAssembly_Result>().HasNoKey();
            modelBuilder.Entity<GetTranscodeTimecode_Result>().HasNoKey();
            modelBuilder.Entity<GetTranscodeOutputSubtitle_Result>().HasNoKey();
            modelBuilder.Entity<GetTranscodeOutputVideo_Result>().HasNoKey();
        }

        #region overrides

        public override async Task<bool> ExecuteCoreServiceBaseInternal()
        {
            string methodName = MethodInfo.GetCurrentMethod().ReflectedType.FullName + "." + MethodInfo.GetCurrentMethod().Name;
            bool result = false;
            ExecutionStep_IONTranscode newExecutionStepIONTranscode = null;
            try
            {
                var serviceNodeAttributes = await sqlHelper.Get<ServiceNodeAttribute>().Where(a => a.IsActive && a.ServiceNodeId == ExecutionStep.ServiceNodeId).ToListAsync();
                var trajectoryAttributes = await sqlHelper.Get<TrajectoryAttribute>().Where(a => a.IsActive && a.TrajectoryId == ExecutionStep.TrajectoryId).ToListAsync();
                var executionStepIONTranscode = await sqlHelper.Get<ExecutionStep_IONTranscode>().Where(a => a.ExecutionStepID == ExecutionStep.ExecutionStepId).SingleOrDefaultAsync();
                string transcodePayload = null;

                #region Validate Stream In File and Channel In Stream

                TraceHelper.Write(traceContext, TraceLevel.Informational, methodName, string.Format(TraceMessageFormat, info, _executionStepID, "Calculating AudioChannelInStream and AudioStreamInFile."));

                List<ComponentAudioChannelFilePath> audioChannelInfoList = new List<ComponentAudioChannelFilePath>();
                List<MediaInfoAudioChannels> mediaInfoAudioChannelsList = new List<MediaInfoAudioChannels>();
                var audioComponents = InboundWorkingSet.Components.Where(t => t.ComponentTypeId == LookupLists.ComponentType.Values.Audio_Channel_Component.ID);
                var audioChannelComponent = new List<ComponentAudioChannel>();
                foreach (Component component in audioComponents)
                {
                    ComponentAudioChannel componentAudioChannel = await sqlHelper.Get<ComponentAudioChannel>().Where(a => a.IsActive && a.ComponentId == component.ComponentId).SingleAsync();
                    audioChannelComponent.Add(componentAudioChannel);
                }

                foreach (ComponentAudioChannel component in audioChannelComponent)
                {
                    EntityAttributeValue isDBBStreamCalcAttributeValue = await sqlHelper.Get<EntityAttributeValue>().Where(a => a.IsActive && a.AttributeId == isDBBStreamCalcAttributeID && a.EntityInstanceId == component.ComponentId).SingleOrDefaultAsync();
                    var componentFileDescriptor = InboundWorkingSet.ComponentFileDescriptor.Where(a => a.ComponentId == component.ComponentId).Single();
                    var fileLocation = await GetFileLocation(InputStorageZone, InboundWorkingSet.Components.Where(a => a.ComponentId == component.ComponentId).Single(), InboundWorkingSet.FileDescriptor.Where(a => a.FileDescriptorId == componentFileDescriptor.FileDescriptorId).Single(), false);
                    string filePath = _inputFileSystemService.GetFilePath(fileLocation.FileLocationId, traceContext);

                    ComponentAudioChannelFilePath componentAudioChannelFilePath = new ComponentAudioChannelFilePath() { ComponentAudioChannel = component, FilePath = filePath };
                    audioChannelInfoList.Add(componentAudioChannelFilePath);

                    var checkedFile = (from m in mediaInfoAudioChannelsList where m.FilePath == filePath select m).Any();

                    if (!checkedFile && (isDBBStreamCalcAttributeValue == null || isDBBStreamCalcAttributeValue.AttributeValueBit == false))
                    {
                        int mediaInfoChannelInFile = 0;
                        int streamCount = 0;
                        try
                        {
                            streamCount = await _inputFileSystemService.GetMediaInfoCount(filePath, StreamKind.Audio, traceContext);
                        }
                        catch (Exception ex)
                        {
                            TraceHelper.Write(traceContext, TraceLevel.Error, methodName, ex);
                        }
                        for (int x = 0; x < streamCount; x++)
                        {
                            //List<string> layouts;
                            var layouts = int.Parse(await _inputFileSystemService.GetMediaInfo(filePath, StreamKind.Audio, x, "Channel(s)", traceContext));
                            for (int y = 0; y < layouts; y++)
                            {
                                MediaInfoAudioChannels mediaInfoAudioChannels = new MediaInfoAudioChannels();
                                mediaInfoAudioChannels.FilePath = filePath;
                                //var newValue = _hybridIngestLookupListValueService.Retrieve(new HybridIngestLookupListValueMapCriteria() { HybridIngestXmlValue = layouts[y] }, _traceContext).FirstOrDefault();
                                //if (newValue != null)
                                //    mediaInfoAudioChannels.ChannelLayout = newValue.LookupListValue.SystemName;
                                mediaInfoAudioChannels.ChannelInFile = ++mediaInfoChannelInFile;
                                mediaInfoAudioChannels.ChannelInStream = y + 1;
                                mediaInfoAudioChannels.StreamInFile = x + 1;
                                mediaInfoAudioChannelsList.Add(mediaInfoAudioChannels);
                            }
                        }
                    }
                    if (isDBBStreamCalcAttributeValue == null)
                    {
                        isDBBStreamCalcAttributeValue = new EntityAttributeValue()
                        {
                            AttributeValueBit = true,
                            EntityInstanceId = component.ComponentId,
                            AttributeId = isDBBStreamCalcAttributeID
                        };

                        await sqlHelper.InsertAsync<EntityAttributeValue>(isDBBStreamCalcAttributeValue);
                        TraceHelper.Write(traceContext, TraceLevel.Informational, methodName, string.Format(TraceMessageFormat, info, _executionStepID, "Insert EntityAttributeValue isDBBStreamCalc."));
                    }
                    else
                    {
                        isDBBStreamCalcAttributeValue.AttributeValueBit = true;
                        await sqlHelper.UpdateAsync<EntityAttributeValue>(isDBBStreamCalcAttributeValue);
                        TraceHelper.Write(traceContext, TraceLevel.Informational, methodName, string.Format(TraceMessageFormat, info, _executionStepID, "Updated EntityAttributeValue isDBBStreamCalc."));
                    }
                }

                foreach (ComponentAudioChannelFilePath audioChannelInfo in audioChannelInfoList)
                {
                    var mediaInfoAudioChannel = (from m in mediaInfoAudioChannelsList
                                                 where m.ChannelInFile == audioChannelInfo.ComponentAudioChannel.AudioChannelInFile && m.FilePath == audioChannelInfo.FilePath
                                                 select m).FirstOrDefault();

                    if (mediaInfoAudioChannel != null)
                    {
                        audioChannelInfo.ComponentAudioChannel.AudioChannelInStream = mediaInfoAudioChannel.ChannelInStream;
                        audioChannelInfo.ComponentAudioChannel.AudioStreamInFile = mediaInfoAudioChannel.StreamInFile;
                        await sqlHelper.UpdateAsync<ComponentAudioChannel>(audioChannelInfo.ComponentAudioChannel);
                    }
                }

                #endregion

                #region Update ComponentVideo Runtime and Audio Channel Count
                var videoComponentIDs = InboundWorkingSet.Components.Where(t => t.ComponentTypeId == LookupLists.ComponentType.Values.Video.ID).Select(a => a.ComponentId);
                foreach (var componentVideo in sqlHelper.Get<ComponentVideo>().Where(a => videoComponentIDs.Contains(a.ComponentId) && a.VideoCodecTypeId != LookupLists.VideoCodecType.Values.J2K.ID && a.VideoCodecTypeId != LookupLists.VideoCodecType.Values.J2K_IMF.ID))
                {
                    var component = InboundWorkingSet.Components.Where(a => a.ComponentId == componentVideo.ComponentId).Single();
                    var componentFileDescriptor = InboundWorkingSet.ComponentFileDescriptor.Where(a => a.ComponentId == componentVideo.ComponentId).Single();
                    var fileLocation = await GetFileLocation(InputStorageZone, component, InboundWorkingSet.FileDescriptor.Where(a => a.FileDescriptorId == componentFileDescriptor.FileDescriptorId).Single(), false);

                    if (fileLocation == null || fileLocation.FileLocationId == Guid.Empty)
                        throw new ManagedServiceException("File Location is null");

                    string filePath = _inputFileSystemService.GetFilePath(fileLocation.FileLocationId, traceContext);

                    EntityAttributeValue isDBBRuntimeAttributeValue = await sqlHelper.Get<EntityAttributeValue>().Where(a => a.IsActive && a.AttributeId == isDBBRuntimeAttributeID && a.EntityInstanceId == componentVideo.ComponentId).SingleOrDefaultAsync();

                    if (isDBBRuntimeAttributeValue == null || isDBBRuntimeAttributeValue.AttributeValueBit == false) //&& componentVideo.RuntimeInMilliseconds == 0)
                    {
                        string duration = await _inputFileSystemService.GetMediaInfo(filePath, StreamKind.General, 0, "Duration", traceContext);
                        TraceHelper.Write(traceContext, TraceLevel.Informational, methodName, string.Format(TraceMessageFormat, info, String.Format("TranscodeIONService: File: {0}, Duration: {1}", filePath, duration), ExecutionStep.ExecutionStepId));

                        decimal runtime = 0.0M;
                        if (decimal.TryParse(duration, out runtime))
                        {
                            TraceHelper.Write(traceContext, TraceLevel.Informational, methodName, string.Format(TraceMessageFormat, info, String.Format("TranscodeIONService: updating runtime for ComponentVideo {0}", componentVideo.ComponentId), ExecutionStep.ExecutionStepId));
                            componentVideo.RuntimeInMilliseconds = Convert.ToInt64(runtime);
                            await sqlHelper.UpdateAsync<ComponentVideo>(componentVideo);

                            if (isDBBRuntimeAttributeValue == null)
                            {
                                TraceHelper.Write(traceContext, TraceLevel.Informational, methodName, string.Format(TraceMessageFormat, info, String.Format("TranscodeIONService: Adding IsDBBRuntime 4NF for ComponentVideo {0}", componentVideo.ComponentId), ExecutionStep.ExecutionStepId));

                                EntityAttributeValue entityAttributeValue = new EntityAttributeValue()
                                {
                                    AttributeValueBit = true,
                                    AttributeId = isDBBRuntimeAttributeID,
                                    EntityInstanceId = componentVideo.ComponentId
                                };
                                await sqlHelper.InsertAsync<EntityAttributeValue>(entityAttributeValue);
                            }
                            else
                            {
                                TraceHelper.Write(traceContext, TraceLevel.Informational, methodName, string.Format(TraceMessageFormat, info, String.Format("TranscodeIONService: Updating IsDBBRuntime 4NF for ComponentVideo {0} to true", componentVideo.ComponentId), ExecutionStep.ExecutionStepId));
                                isDBBRuntimeAttributeValue.AttributeValueBit = true;
                                await sqlHelper.UpdateAsync<EntityAttributeValue>(isDBBRuntimeAttributeValue);
                            }
                        }
                    }

                    if (component.ComponentEntityTypeId == LookupLists.ComponentEntityType.Values.Supporting.ID)
                    {
                        EntityAttributeValue audioChannelCountAttributeValue = await sqlHelper.Get<EntityAttributeValue>().Where(a => a.IsActive && a.AttributeId == audioChannelCountAttributeID && a.EntityInstanceId == componentVideo.ComponentId).SingleOrDefaultAsync();
                        int audioStreams = await _inputFileSystemService.GetMediaInfoCount(filePath, StreamKind.Audio, traceContext);
                        int channelCount = 0;
                        for (int i = 0; i < audioStreams; i++)
                        {
                            channelCount += int.Parse(await _inputFileSystemService.GetMediaInfo(filePath, StreamKind.Audio, i, "Channel(s)", traceContext));
                        }

                        if (audioChannelCountAttributeValue == null)
                        {
                            TraceHelper.Write(traceContext, TraceLevel.Informational, methodName, string.Format(TraceMessageFormat, info, String.Format("TranscodeIONService: Adding AudioChannelCount 4NF for ComponentVideo {0}", componentVideo.ComponentId), ExecutionStep.ExecutionStepId));

                            audioChannelCountAttributeValue = new EntityAttributeValue()
                            {
                                AttributeValueInt = channelCount,
                                AttributeId = audioChannelCountAttributeID,
                                EntityInstanceId = componentVideo.ComponentId
                            };
                            await sqlHelper.InsertAsync<EntityAttributeValue>(audioChannelCountAttributeValue);
                        }
                        else
                        {
                            TraceHelper.Write(traceContext, TraceLevel.Informational, methodName, string.Format(TraceMessageFormat, info, String.Format("TranscodeIONService: Updating AudioChannelCount 4NF for ComponentVideo {0}", componentVideo.ComponentId), ExecutionStep.ExecutionStepId));
                            audioChannelCountAttributeValue.AttributeValueInt = channelCount;
                            await sqlHelper.UpdateAsync<EntityAttributeValue>(audioChannelCountAttributeValue);
                        }
                    }
                }
                #endregion

                var packageElementExternalProcessing = sqlHelper.Get<PackageElementExternalProcessing>().Where(a => a.IsActive && a.PackageElementID == PackageElement.PackageElementId).FirstOrDefault();

                #region create filelocation for single w/ output
                if (packageElementExternalProcessing != null && packageElementExternalProcessing.IONCompletionFollowOnTypeID == LookupLists.CompletionFollowOnType.Values.SingleWithOutput.ID)
                {
                    // validate the FileLocations
                    foreach (var fileDescriptor in OutboundWorkingSet.FileDescriptor)
                    {
                        var componentFileDescriptor = OutboundWorkingSet.ComponentFileDescriptor.Where(a => a.FileDescriptorId == fileDescriptor.FileDescriptorId).First();
                        var component = OutboundWorkingSet.Components.Where(a => a.ComponentId == componentFileDescriptor.ComponentId).Single();
                        var fileLocation = await GetFileLocation(OutputStorageZone, component, fileDescriptor, true);

                        if (fileLocation == null || fileLocation.FileLocationId == Guid.Empty)
                        {
                            TraceHelper.Write(traceContext, TraceLevel.Error, methodName, string.Format(TraceMessageFormat, error, "Single Trajectory, Single File, Internal Storage Zone, Could not verify Filelocation for the storage zone: ", OutputStorageZone.StorageZoneId));
                            throw new ManagedServiceException(string.Format(TraceMessageFormat, error, "Single Trajectory, Single File, Internal Storage Zone, Could not verify Filelocation for the storage zone: ", OutputStorageZone.StorageZoneId));
                        }
                    }
                }
                #endregion

                var isSubmitToInjector = await IsSubmitInjector(trajectoryAttributes);

                var isPostTranscodeQC = await sqlHelper.Get<PackageSpecification>().Where(a => a.ClientProfileID == RequestItem.ClientProfileId && a.BatonQCTypeID == LookupLists.BatonQCType.Values.Post_Transcode_QC.ID).AnyAsync();

                TraceHelper.Write(traceContext, TraceLevel.Informational, methodName, string.Format(TraceMessageFormat, info, ExecutionStep.ExecutionStepId, "isPostTranscodeQC:" + isPostTranscodeQC.ToString()));

                #region update extension table
                if (executionStepIONTranscode != null)
                    await sqlHelper.InactivateAsync<ExecutionStep_IONTranscode>(executionStepIONTranscode);

                newExecutionStepIONTranscode = new ExecutionStep_IONTranscode()
                {
                    ExecutionStepID = ExecutionStep.ExecutionStepId,
                    ProcessStatus = "PendingIndirect"
                };
                await sqlHelper.InsertAsync<ExecutionStep_IONTranscode>(newExecutionStepIONTranscode);
                #endregion

                string jobTargetValue = null;
                if (isSubmitToInjector && !isPostTranscodeQC && (packageElementExternalProcessing == null || packageElementExternalProcessing.IONCompletionFollowOnTypeID != LookupLists.CompletionFollowOnType.Values.Full.ID))
                {
                    var folderFileDescriptors = OutboundWorkingSet.FileDescriptor.Where(b => b.FileFormatTypeId == LookupLists.FileFormatType.Values.Folder_folder.ID);

                    foreach (var folderFileDescriptor in folderFileDescriptors)
                    {
                        var componentFileDescriptor = OutboundWorkingSet.ComponentFileDescriptor.Where(a => a.FileDescriptorId == folderFileDescriptor.FileDescriptorId).First();
                        var folderComponent = OutboundWorkingSet.Components.Where(a => a.ComponentId == componentFileDescriptor.ComponentId).Single();


                        var storageZoneName = (OutputStorageZone == null) ? "" : (OutputStorageZone.StorageZoneName ?? "");
                        TraceHelper.Write(traceContext, TraceLevel.Informational, methodName, string.Format("OutputStorageZone: {0}", storageZoneName));

                        var dynamicDataFunctionService = new NamingEngine(ConnectionString);
                        var fileName = dynamicDataFunctionService.ApplyNamingRules(PackageElement.FileNamingRuleGroupId.GetValueOrDefault(), PackageElement, folderComponent, ExecutionStep, false, traceContext);
                        TraceHelper.Write(traceContext, TraceLevel.Informational, methodName, "FileName = " + fileName);

                        if (!string.IsNullOrEmpty(fileName))
                        {
                            var fLocation = await GetFileLocation(OutputStorageZone, folderComponent, folderFileDescriptor, false);
                            fLocation.FileName = fileName;
                            await sqlHelper.UpdateAsync<FileLocation>(fLocation);
                        }
                    }

                    string injectorURL = trajectoryAttributes.Where(a => a.LookupListValueId == LookupLists.TrajectoryAttributeName.Values.InjectorURL.ID).Select(b => b.AttributeValue).Single();
                    transcodePayload = await CreateInjectorJob(newExecutionStepIONTranscode.ExecutionStep_IONTranscodeID, injectorURL);

                    jobTargetValue = "Injector";
                }
                else
                {
                    string ionURL = trajectoryAttributes.Where(a => a.LookupListValueId == LookupLists.TrajectoryAttributeName.Values.IONURL.ID).Select(b => b.AttributeValue).Single();
                    transcodePayload = await CreateIONJob(newExecutionStepIONTranscode.ExecutionStep_IONTranscodeID, ionURL);

                    //delay for ION
                    int delay = 10;
                    int.TryParse(serviceNodeAttributes.Where(x => x.LookupListValueId == LookupLists.ServiceNodeAttributeName.Values.ION_Transcode_Delay.ID).Select(y => y.AttributeValue).SingleOrDefault(), out delay);
                    Thread.Sleep(new TimeSpan(0, 0, delay));

                    jobTargetValue = "ION";
                }

                await CpySrcSubLangAsTrgVideoBrnSub();

                #region update job target attribute
                EntityAttributeValue jobTargetAttributeValue = await sqlHelper.Get<EntityAttributeValue>().Where(a => a.IsActive && a.AttributeId == jobTargetAttributeID && a.EntityInstanceId == ExecutionStep.ExecutionStepId).SingleOrDefaultAsync();

                if (jobTargetAttributeValue == null)
                {
                    TraceHelper.Write(traceContext, TraceLevel.Informational, methodName, string.Format(TraceMessageFormat, info, String.Format("TranscodeIONService: Adding JobTarget 4NF with {0}", jobTargetValue), ExecutionStep.ExecutionStepId));

                    EntityAttributeValue entityAttributeValue = new EntityAttributeValue()
                    {
                        AttributeValueNvarchar255 = jobTargetValue,
                        AttributeId = jobTargetAttributeID,
                        EntityInstanceId = ExecutionStep.ExecutionStepId
                    };
                    await sqlHelper.InsertAsync<EntityAttributeValue>(entityAttributeValue);
                }
                else
                {
                    TraceHelper.Write(traceContext, TraceLevel.Informational, methodName, string.Format(TraceMessageFormat, info, String.Format("TranscodeIONService: Updating JobTarget 4NF to {0}", jobTargetValue), ExecutionStep.ExecutionStepId));
                    jobTargetAttributeValue.AttributeValueNvarchar255 = jobTargetValue;
                    await sqlHelper.UpdateAsync<EntityAttributeValue>(jobTargetAttributeValue);
                }
                #endregion

                newExecutionStepIONTranscode.JobXML = transcodePayload;
                await sqlHelper.UpdateAsync<ExecutionStep_IONTranscode>(newExecutionStepIONTranscode);
                await UpdateExecutionStepStatus(_executionStepPendingIndirectStatusID, "ION Order created.", "PendingIndirect", 0);
            }
            catch (System.ServiceModel.EndpointNotFoundException ex)
            {
                if (newExecutionStepIONTranscode != null)
                {
                    newExecutionStepIONTranscode.PercentComplete = 0;
                    newExecutionStepIONTranscode.ProcessStatus = "IONCommunicationError";
                    newExecutionStepIONTranscode.ErrorMessage = ex.Message;
                    await sqlHelper.UpdateAsync<ExecutionStep_IONTranscode>(newExecutionStepIONTranscode);
                }
                await UpdateExecutionStepStatus(LookupLists.ExecutionStepStatusType.Values.Error.ID, "Endpoint not found exception", "IONCommunicationError", 0, "IONCommunicationError");
                TraceHelper.Write(traceContext, TraceLevel.Error, methodName, ex);
                result = false;
            }
            catch (System.ServiceModel.CommunicationException ex)
            {
                if (newExecutionStepIONTranscode != null)
                {
                    newExecutionStepIONTranscode.PercentComplete = 0;
                    newExecutionStepIONTranscode.ProcessStatus = "IONCommunicationError";
                    newExecutionStepIONTranscode.ErrorMessage = ex.Message;
                    await sqlHelper.UpdateAsync<ExecutionStep_IONTranscode>(newExecutionStepIONTranscode);
                }
                await UpdateExecutionStepStatus(LookupLists.ExecutionStepStatusType.Values.Error.ID, "Time out exception", "IONCommunicationError", 0, "IONCommunicationError");
                TraceHelper.Write(traceContext, TraceLevel.Error, methodName, ex);
                result = false;
            }
            catch (System.TimeoutException ex)
            {
                if (newExecutionStepIONTranscode != null)
                {
                    newExecutionStepIONTranscode.PercentComplete = 0;
                    newExecutionStepIONTranscode.ProcessStatus = "LaunchTimeOut";
                    newExecutionStepIONTranscode.ErrorMessage = ex.Message;
                    await sqlHelper.UpdateAsync<ExecutionStep_IONTranscode>(newExecutionStepIONTranscode);
                }
                await UpdateExecutionStepStatus(LookupLists.ExecutionStepStatusType.Values.Error.ID, "Time out exception", "LaunchTimeOut", 0, "LaunchTimeOut");
                TraceHelper.Write(traceContext, TraceLevel.Error, methodName, ex);
                result = false;
            }
            catch (Exception ex)
            {
                //await UpdateExecutionStepStatus(_executionStepAugmentedStatusID, ex.Message, "Error", 0, null);
                TraceHelper.Write(traceContext, TraceLevel.Informational, methodName, ex);
                throw ex;
            }

            return result;
        }

        public override async Task<bool> GetStatusInternal()
        {
            string methodName = MethodInfo.GetCurrentMethod().ReflectedType.FullName + "." + MethodInfo.GetCurrentMethod().Name;
            bool result = true;

            //var serviceNodeAttributes = await sqlHelper.Get<ServiceNodeAttribute>().Where(a => a.IsActive && a.ServiceNodeId == ExecutionStep.ServiceNodeId).ToListAsync();
            var trajectoryAttributes = await sqlHelper.Get<TrajectoryAttribute>().Where(a => a.IsActive && a.TrajectoryId == ExecutionStep.TrajectoryId).ToListAsync();
            var executionStepIONTranscode = await sqlHelper.Get<ExecutionStep_IONTranscode>().Where(a => a.ExecutionStepID == ExecutionStep.ExecutionStepId).SingleAsync();

            StatusRequest request = new StatusRequest();
            StatusResponse statusResponse = new StatusResponse();
            try
            {
                string ionURL = trajectoryAttributes.Where(a => a.LookupListValueId == LookupLists.TrajectoryAttributeName.Values.IONURL.ID).Select(b => b.AttributeValue).Single();
                request.JobId = executionStepIONTranscode.ExecutionStep_IONTranscodeID;
                IIONService ionClinet = CreateIONClient(ionURL);

                statusResponse = ionClinet.GetStatus(request);

                TraceHelper.Write(traceContext, TraceLevel.Informational, methodName, string.Format(TraceMessageFormat, info, _executionStepID, "ION Status:" + statusResponse.Status.ToString()));

                string errorCode = "UnHandled";

                if (!String.IsNullOrEmpty(statusResponse.StatusCode))
                {
                    errorCode = statusResponse.StatusCode;
                }

                if (ExecutionStep.ExecutionStepStatusTypeId == LookupLists.ExecutionStepStatusType.Values.PendingCancel.ID)
                {
                    switch (statusResponse.Status)
                    {
                        case JobStatus.Cancelled:
                            executionStepIONTranscode.PercentComplete = statusResponse.PercentageCompleted;
                            executionStepIONTranscode.ProcessStatus = "Canceled";
                            executionStepIONTranscode.ErrorMessage = "DBB cancelled order, cancelled order in ION, so marking to Canceled.";

                            await UpdateExecutionStepStatus(LookupLists.ExecutionStepStatusType.Values.Canceled.ID, "Cancelled order in ION.", "Cancelled", statusResponse.PercentageCompleted);
                            TraceHelper.Write(traceContext, TraceLevel.Informational, methodName, string.Format(TraceMessageFormat, info, _executionStepID, "IONOrder ExecutionStep Cancelled."));
                            break;
                        case JobStatus.Error:
                            executionStepIONTranscode.PercentComplete = statusResponse.PercentageCompleted;
                            executionStepIONTranscode.ProcessStatus = "Error";
                            executionStepIONTranscode.ErrorMessage = statusResponse.StatusMessage;

                            await UpdateExecutionStepStatus(LookupLists.ExecutionStepStatusType.Values.Error.ID, "IONTranscoding resulted error:" + statusResponse.StatusMessage, "IONCancelError", statusResponse.PercentageCompleted, "IONCancelError");
                            TraceHelper.Write(traceContext, TraceLevel.Error, methodName, string.Format(TraceMessageFormat, error, _executionStepID, "IONOrder returned Error on cancel"));
                            result = false;
                            break;
                    }
                    await sqlHelper.UpdateAsync<ExecutionStep_IONTranscode>(executionStepIONTranscode);
                }
                else
                {
                    switch (statusResponse.Status)
                    {
                        case JobStatus.Pending:
                            executionStepIONTranscode.PercentComplete = statusResponse.PercentageCompleted;
                            executionStepIONTranscode.ProcessStatus = "PendingInDirect";
                            //Not updating the executionstep status, as it is same.     
                            TraceHelper.Write(traceContext, TraceLevel.Informational, methodName, string.Format(TraceMessageFormat, info, _executionStepID, "IONOrder ExecutionStep Pending"));
                            break;
                        case JobStatus.InProgress:
                        case JobStatus.InProgressNonCancellable:
                            executionStepIONTranscode.PercentComplete = statusResponse.PercentageCompleted;
                            executionStepIONTranscode.ProcessStatus = statusResponse.Status.ToString();
                            if (ExecutionStep.ExecutionStepStatusTypeId != LookupLists.ExecutionStepStatusType.Values.InProgress.ID || ExecutionStep.ProcessPercentComplete != executionStepIONTranscode.PercentComplete)
                                await UpdateExecutionStepStatus(LookupLists.ExecutionStepStatusType.Values.InProgress.ID, "IONTranscoding is in progress", "InProgress", statusResponse.PercentageCompleted);

                            TraceHelper.Write(traceContext, TraceLevel.Informational, methodName, string.Format(TraceMessageFormat, info, _executionStepID, "IONOrder ExecutionStep Inprogress"));
                            break;
                        case JobStatus.Cancelled:
                            executionStepIONTranscode.PercentComplete = statusResponse.PercentageCompleted;
                            executionStepIONTranscode.ProcessStatus = "Error";
                            executionStepIONTranscode.ErrorMessage = "Cancelled order in ION, so marking to error.";
                            await UpdateExecutionStepStatus(LookupLists.ExecutionStepStatusType.Values.Error.ID, "Cancelled order in ION, so marking to error.", "Cancelled", statusResponse.PercentageCompleted, errorCode);
                            TraceHelper.Write(traceContext, TraceLevel.Error, methodName, string.Format(TraceMessageFormat, error, _executionStepID, "IONOrder ExecutionStep Cancelled, so marking to error."));
                            result = false;
                            break;
                        case JobStatus.Complete:
                            executionStepIONTranscode.PercentComplete = 100;
                            executionStepIONTranscode.ProcessStatus = "Complete";
                            await UpdateFileMovementStatus(LookupLists.FileMovementStateType.Values.Complete.ID);
                            await UpdateExecutionStepStatus(LookupLists.ExecutionStepStatusType.Values.Complete.ID, "IONTranscoding is completed", "Complete", statusResponse.PercentageCompleted);

                            //ION V2 Scenario 
                            var packageElementExternalProcessing = sqlHelper.Get<PackageElementExternalProcessing>().Where(a => a.IsActive && a.PackageElementID == PackageElement.PackageElementId &&
                                a.IONCompletionFollowOnTypeID == LookupLists.CompletionFollowOnType.Values.Full.ID).FirstOrDefault();
                            if (packageElementExternalProcessing != null)
                            {
                                var dbbContext = new Core.ExecutionService.Data.ExecutionContext(ConnectionString);
                                var transcodeRepository = new TranscodeRepository(dbbContext);
                                await transcodeRepository.CompleteExecutionStepsForION(ExecutionStep.ExecutionStepId);
                            }
                            else
                            {
                                TraceHelper.Write(traceContext, TraceLevel.Informational, methodName, string.Format(TraceMessageFormat, info, _executionStepID, "ION COmpletionTypeID is not Full"));
                            }
                            TraceHelper.Write(traceContext, TraceLevel.Informational, methodName, string.Format(TraceMessageFormat, info, _executionStepID, "IONOrder ExecutionStep Completed"));
                            break;
                        case JobStatus.Error:
                            executionStepIONTranscode.PercentComplete = statusResponse.PercentageCompleted;
                            executionStepIONTranscode.ProcessStatus = "Error";
                            executionStepIONTranscode.ErrorMessage = statusResponse.StatusMessage;
                            await UpdateExecutionStepStatus(LookupLists.ExecutionStepStatusType.Values.Error.ID, "IONTranscoding resulted error:" + statusResponse.StatusMessage, "Error", statusResponse.PercentageCompleted, errorCode);
                            TraceHelper.Write(traceContext, TraceLevel.Error, methodName, string.Format(TraceMessageFormat, error, _executionStepID, "IONOrder returned Error"));
                            result = false;
                            break;
                        default:
                            executionStepIONTranscode.PercentComplete = 0;
                            executionStepIONTranscode.ProcessStatus = "Error";
                            executionStepIONTranscode.ErrorMessage = "This case is not handled:" + statusResponse.Status;
                            await UpdateExecutionStepStatus(LookupLists.ExecutionStepStatusType.Values.Error.ID, "Unhandled case", "Error", statusResponse.PercentageCompleted, errorCode);
                            TraceHelper.Write(traceContext, TraceLevel.Error, methodName, string.Format(TraceMessageFormat, error, _executionStepID, "IONOrder status was not recogonized"));
                            result = false;
                            break;
                    }
                    await sqlHelper.UpdateAsync<ExecutionStep_IONTranscode>(executionStepIONTranscode);
                }
            }
            catch (System.ServiceModel.EndpointNotFoundException ex)
            {
                executionStepIONTranscode.PercentComplete = 0;
                executionStepIONTranscode.ProcessStatus = "IONCommunicationError";
                executionStepIONTranscode.ErrorMessage = ex.Message;
                await sqlHelper.UpdateAsync<ExecutionStep_IONTranscode>(executionStepIONTranscode);
                if (ExecutionStep.ExecutionStepStatusTypeId == LookupLists.ExecutionStepStatusType.Values.PendingCancel.ID)
                {
                    TraceHelper.Write(traceContext, TraceLevel.Informational, methodName, string.Format(TraceMessageFormat, info, _executionStepID, "ION Gestatus failed, DBB in cancel so marking IONCancelError"));

                    await UpdateExecutionStepStatus(LookupLists.ExecutionStepStatusType.Values.Error.ID, "IONCancelError", "IONTranscoding resulted error:" + statusResponse.StatusMessage, statusResponse.PercentageCompleted, "IONCancelError");
                }
                else
                {
                    await UpdateExecutionStepStatus(LookupLists.ExecutionStepStatusType.Values.Error.ID, "Endpoint not found exception", "IONCommunicationError", 0, "IONCommunicationError");
                }
                TraceHelper.Write(traceContext, TraceLevel.Error, methodName, ex);
                result = false;
            }
            catch (System.ServiceModel.CommunicationException ex)
            {
                executionStepIONTranscode.PercentComplete = 0;
                executionStepIONTranscode.ProcessStatus = "StatusIONCommunicationError";
                executionStepIONTranscode.ErrorMessage = ex.Message;
                await sqlHelper.UpdateAsync<ExecutionStep_IONTranscode>(executionStepIONTranscode);
                if (ExecutionStep.ExecutionStepStatusTypeId == LookupLists.ExecutionStepStatusType.Values.PendingCancel.ID)
                {
                    TraceHelper.Write(traceContext, TraceLevel.Informational, methodName, string.Format(TraceMessageFormat, info, _executionStepID, "ION Gestatus failed, DBB in cancel so marking IONCancelError"));

                    await UpdateExecutionStepStatus(LookupLists.ExecutionStepStatusType.Values.Error.ID, "IONCancelError", "IONTranscoding resulted error:" + statusResponse.StatusMessage, statusResponse.PercentageCompleted, "IONCancelError");
                }
                else
                {
                    await UpdateExecutionStepStatus(LookupLists.ExecutionStepStatusType.Values.Error.ID, "IONCommunicationError", "Time out exception", 0, "StatusIONCommunicationError");
                }
                TraceHelper.Write(traceContext, TraceLevel.Error, methodName, ex);
                result = false;
            }
            catch (System.TimeoutException ex)
            {
                executionStepIONTranscode.PercentComplete = 0;
                executionStepIONTranscode.ProcessStatus = "StatusTimeOut";
                executionStepIONTranscode.ErrorMessage = ex.Message;
                await sqlHelper.UpdateAsync<ExecutionStep_IONTranscode>(executionStepIONTranscode);
                if (ExecutionStep.ExecutionStepStatusTypeId == LookupLists.ExecutionStepStatusType.Values.PendingCancel.ID)
                {
                    TraceHelper.Write(traceContext, TraceLevel.Informational, methodName, string.Format(TraceMessageFormat, info, _executionStepID, "ION Gestatus failed, DBB in cancel so marking IONCancelError"));
                    await UpdateExecutionStepStatus(LookupLists.ExecutionStepStatusType.Values.Error.ID, "IONCancelError", "IONTranscoding resulted error:" + statusResponse.StatusMessage, statusResponse.PercentageCompleted, "IONCancelError");
                }
                else
                {
                    await UpdateExecutionStepStatus(LookupLists.ExecutionStepStatusType.Values.Error.ID, "StatusTimeOut", "Time out exception", 0, "StatusTimeOut");
                }
                TraceHelper.Write(traceContext, TraceLevel.Error, methodName, ex);
                result = false;
            }
            catch (Exception ex)
            {
                TraceHelper.Write(traceContext, TraceLevel.Error, methodName, ex.ToString());
                if (ExecutionStep.ExecutionStepStatusTypeId == LookupLists.ExecutionStepStatusType.Values.PendingCancel.ID)
                {
                    TraceHelper.Write(traceContext, TraceLevel.Error, methodName, string.Format(TraceMessageFormat, error, _executionStepID, "ION Gestatus failed, DBB in cancel so marking IONCancelError"));
                    await UpdateExecutionStepStatus(LookupLists.ExecutionStepStatusType.Values.Error.ID, "IONCancelError", "ION cancellation resulted error", 0, "IONCancelError");
                }
                else
                {
                    await UpdateExecutionStepStatus(LookupLists.ExecutionStepStatusType.Values.Error.ID, "ERROR", "Exception:" + ex.ToString(), 0, "UnHandled");
                }
                result = false;
            }

            return result;
        }

        /// <summary>
        /// Publish the status to ION.
        /// </summary>
        /// <returns></returns>
        public override async Task<bool> PostStatusInternal()
        {
            string methodName = MethodInfo.GetCurrentMethod().ReflectedType.FullName + "." + MethodInfo.GetCurrentMethod().Name;
            try
            {
                StatusResponse statusResponse = new StatusResponse();

                TraceHelper.Write(traceContext, TraceLevel.Diagnostic, methodName, string.Format(TraceMessageFormat, beg, "IONPostStatusServiceBaseInternal executionStepID", ExecutionStep.ExecutionStepId));

                //var serviceNodeAttributes = await sqlHelper.Get<ServiceNodeAttribute>().Where(a => a.IsActive && a.ServiceNodeId == ExecutionStep.ServiceNodeId).ToListAsync();
                var trajectoryAttributes = await sqlHelper.Get<TrajectoryAttribute>().Where(a => a.IsActive && a.TrajectoryId == ExecutionStep.TrajectoryId).ToListAsync();
                var executionStepIONTranscode = await sqlHelper.Get<ExecutionStep_IONTranscode>().Where(a => a.ExecutionStepID == ExecutionStep.ExecutionStepId).SingleAsync();

                var injectorPostStatusUrl = trajectoryAttributes.Where(a => a.LookupListValueId == LookupLists.TrajectoryAttributeName.Values.InjectorPostStatusURL.ID).Select(b => b.AttributeValue).Single();
                TraceHelper.Write(traceContext, TraceLevel.Informational, methodName, string.Format(TraceMessageFormat, info, "Trajectory Attributes - injectorPostStatusUrl:" + injectorPostStatusUrl, ExecutionStep.ExecutionStepId));
                var ionUrl = trajectoryAttributes.Where(a => a.LookupListValueId == LookupLists.TrajectoryAttributeName.Values.IONURL.ID).Select(b => b.AttributeValue).Single();
                TraceHelper.Write(traceContext, TraceLevel.Informational, methodName, string.Format(TraceMessageFormat, info, "Trajectory Attributes - IONURL:" + ionUrl, ExecutionStep.ExecutionStepId));

                var submitInjector = await IsSubmitInjector(trajectoryAttributes);

                //todo: should have a check for proper action
                if (submitInjector)
                {
                    var injectorClient = RestService.For<IPostStatusInjectorService>(injectorPostStatusUrl);

                    TraceHelper.Write(traceContext, TraceLevel.Informational, methodName, string.Format(TraceMessageFormat, info, "Going to call Injector Client PostStatus for executionStepID", ExecutionStep.ExecutionStepId));

                    InjectorPostStatusRequest request = new InjectorPostStatusRequest()
                    {
                        DBBJobId = executionStepIONTranscode.ExecutionStep_IONTranscodeID,
                        Status = "Cancelled"
                    };
                    await injectorClient.PostStatus(request);
                }
                else
                {
                    IIONService ionClinet = CreateIONClient(ionUrl);

                    TraceHelper.Write(traceContext, TraceLevel.Informational, methodName, string.Format(TraceMessageFormat, info, "Going to call ION Client PostStatus for executionStepID", ExecutionStep.ExecutionStepId));

                    PostStatusRequest request = new PostStatusRequest()
                    {
                        JobId = executionStepIONTranscode.ExecutionStep_IONTranscodeID,
                        JobStatus = ExecutionStatus.Cancelled
                    };
                    ionClinet.PostStatus(request);
                }

                TraceHelper.Write(traceContext, TraceLevel.Informational, methodName, string.Format(TraceMessageFormat, info, executionStepIONTranscode.ExecutionStep_IONTranscodeID, "Cancel request successfully submitted to ION., now getstatus gives the results from ION"));
                await UpdateExecutionStepStatus(LookupLists.ExecutionStepStatusType.Values.PendingCancel.ID, "Mark the execution step to PendingCancel.", "PendingCancel", 0);

                return true;

            }
            catch (Exception ex)
            {
                TraceHelper.Write(traceContext, TraceLevel.Error, methodName, ex.ToString());
                await UpdateExecutionStepStatus(LookupLists.ExecutionStepStatusType.Values.Error.ID, "Exception:" + ex.ToString(), "IONCancelError", 0, "IONCancelError");
                return false;
            }
        }

        public override async Task<bool> ReceiveStatusInternal(ExternalStatus status)
        {
            string methodName = MethodInfo.GetCurrentMethod().ReflectedType.FullName + "." + MethodInfo.GetCurrentMethod().Name;

            //var serviceNodeAttributes = await sqlHelper.Get<ServiceNodeAttribute>().Where(a => a.IsActive && a.ServiceNodeId == ExecutionStep.ServiceNodeId).ToListAsync();
            var trajectoryAttributes = await sqlHelper.Get<TrajectoryAttribute>().Where(a => a.IsActive && a.TrajectoryId == ExecutionStep.TrajectoryId).ToListAsync();

            if (status == null || status.JobId == Guid.Empty)
            {
                TraceHelper.Write(traceContext, TraceLevel.Error, methodName, string.Format(TraceMessageFormat, info, "IONTranscode", "Status Object is null"));
                throw new ManagedServiceException("StatusDataRetrievalError", "Status Object is null");
            }

            ExecutionStep_IONTranscode executionStepIONTranscode = await sqlHelper.Get<ExecutionStep_IONTranscode>().Where(a => a.ExecutionStep_IONTranscodeID == status.JobId).SingleAsync();
            if (executionStepIONTranscode == null)
            {
                TraceHelper.Write(traceContext, TraceLevel.Error, methodName, string.Format(TraceMessageFormat, info, "IONTranscode", "Child table not retrieved"));
                throw new ManagedServiceException("StatusDataRetrievalError", "Child table not retrieved");
            }

            _executionStepID = executionStepIONTranscode.ExecutionStepID;

            string errorCode = "UnHandled";

            if (!String.IsNullOrEmpty(status.StatusCode))
            {
                errorCode = status.StatusCode;
            }
            ExternalStatus statusResponse = status;

            if (ExecutionStep.ExecutionStepStatusTypeId == LookupLists.ExecutionStepStatusType.Values.PendingCancel.ID)
            {
                switch (statusResponse.Status)
                {
                    case JobStatusEnum.Cancelled:
                        executionStepIONTranscode.PercentComplete = statusResponse.PercentageCompleted;
                        executionStepIONTranscode.ProcessStatus = "Canceled";
                        executionStepIONTranscode.ErrorMessage = "DBB cancelled order, cancelled order in ION, so marking to Canceled.";

                        await UpdateExecutionStepStatus(LookupLists.ExecutionStepStatusType.Values.Canceled.ID, "Cancelled order in ION.", "Cancelled", statusResponse.PercentageCompleted);
                        TraceHelper.Write(traceContext, TraceLevel.Informational, methodName, string.Format(TraceMessageFormat, info, _executionStepID, "IONOrder ExecutionStep Cancelled."));
                        break;
                    case JobStatusEnum.Error:
                        executionStepIONTranscode.PercentComplete = statusResponse.PercentageCompleted;
                        executionStepIONTranscode.ProcessStatus = "Error";
                        executionStepIONTranscode.ErrorMessage = statusResponse.StatusMessage;

                        await UpdateExecutionStepStatus(LookupLists.ExecutionStepStatusType.Values.Error.ID, "IONTranscoding resulted error:" + statusResponse.StatusMessage, "IONCancelError", statusResponse.PercentageCompleted, "IONCancelError");
                        TraceHelper.Write(traceContext, TraceLevel.Error, methodName, string.Format(TraceMessageFormat, error, _executionStepID, "IONOrder returned Error on cancel"));
                        break;
                }
            }
            else if (ExecutionStep.ExecutionStepStatusTypeId == LookupLists.ExecutionStepStatusType.Values.PendingIndirect.ID || ExecutionStep.ExecutionStepStatusTypeId == LookupLists.ExecutionStepStatusType.Values.InProgress.ID)
            {
                switch (status.Status)
                {
                    case JobStatusEnum.Pending:
                        executionStepIONTranscode.PercentComplete = statusResponse.PercentageCompleted;
                        executionStepIONTranscode.ProcessStatus = "PendingInDirect";
                        //Not updating the executionstep status, as it is same.     
                        TraceHelper.Write(traceContext, TraceLevel.Informational, methodName, string.Format(TraceMessageFormat, info, ExecutionStep.ExecutionStepId, "IONOrder ExecutionStep Pending"));
                        break;
                    case JobStatusEnum.InProgress:
                    case JobStatusEnum.InProgressNonCancellable:
                        executionStepIONTranscode.PercentComplete = statusResponse.PercentageCompleted;
                        executionStepIONTranscode.ProcessStatus = status.Status.ToString();
                        if (ExecutionStep.ExecutionStepStatusTypeId != LookupLists.ExecutionStepStatusType.Values.InProgress.ID || ExecutionStep.ProcessPercentComplete != executionStepIONTranscode.PercentComplete)
                            await UpdateExecutionStepStatus(LookupLists.ExecutionStepStatusType.Values.InProgress.ID, "ION Transcoding is in progress", "InProgress", statusResponse.PercentageCompleted);

                        TraceHelper.Write(traceContext, TraceLevel.Informational, methodName, string.Format(TraceMessageFormat, info, ExecutionStep.ExecutionStepId, "IONOrder ExecutionStep Inprogress"));
                        break;
                    case JobStatusEnum.Cancelled:
                        executionStepIONTranscode.PercentComplete = statusResponse.PercentageCompleted;
                        executionStepIONTranscode.ProcessStatus = "Error";
                        executionStepIONTranscode.ErrorMessage = "Cancelled order in ION, so marking to error.";
                        await UpdateExecutionStepStatus(LookupLists.ExecutionStepStatusType.Values.Error.ID, "Cancelled order in ION, so marking to error.", "Cancelled", statusResponse.PercentageCompleted, errorCode);
                        TraceHelper.Write(traceContext, TraceLevel.Diagnostic, methodName, string.Format(TraceMessageFormat, error, ExecutionStep.ExecutionStepId, "IONOrder ExecutionStep Cancelled, so marking to error."));
                        break;
                    case JobStatusEnum.Complete:
                        executionStepIONTranscode.PercentComplete = 100;
                        executionStepIONTranscode.ProcessStatus = "Complete";
                        await UpdateFileMovementStatus(LookupLists.FileMovementStateType.Values.Complete.ID);
                        await UpdateExecutionStepStatus(LookupLists.ExecutionStepStatusType.Values.Complete.ID, "IONTranscoding is completed", "Complete", statusResponse.PercentageCompleted);

                        //ION V2 Scenario 
                        var packageElementExternalProcessing = sqlHelper.Get<PackageElementExternalProcessing>().Where(a => a.IsActive && a.PackageElementID == PackageElement.PackageElementId &&
                            a.IONCompletionFollowOnTypeID == LookupLists.CompletionFollowOnType.Values.Full.ID).FirstOrDefault();
                        if (packageElementExternalProcessing != null)
                        {
                            var dbbContext = new Core.ExecutionService.Data.ExecutionContext(ConnectionString);
                            var transcodeRepository = new TranscodeRepository(dbbContext);
                            await transcodeRepository.CompleteExecutionStepsForION(ExecutionStep.ExecutionStepId);
                        }
                        else
                        {
                            TraceHelper.Write(traceContext, TraceLevel.Informational, methodName, string.Format(TraceMessageFormat, info, _executionStepID, "ION COmpletionTypeID is not Full"));
                        }
                        TraceHelper.Write(traceContext, TraceLevel.Diagnostic, methodName, string.Format(TraceMessageFormat, info, _executionStepID, "IONOrder ExecutionStep Completed"));
                        break;
                    case JobStatusEnum.Error:
                        executionStepIONTranscode.PercentComplete = statusResponse.PercentageCompleted;
                        executionStepIONTranscode.ProcessStatus = "Error";
                        executionStepIONTranscode.ErrorMessage = statusResponse.StatusMessage;
                        await UpdateExecutionStepStatus(LookupLists.ExecutionStepStatusType.Values.Error.ID, "IONTranscoding resulted error:" + statusResponse.StatusMessage, "Error", statusResponse.PercentageCompleted, errorCode);
                        TraceHelper.Write(traceContext, TraceLevel.Error, methodName, string.Format(TraceMessageFormat, error, ExecutionStep.ExecutionStepId, "IONOrder returned Error"));
                        break;
                    default:
                        executionStepIONTranscode.PercentComplete = 0;
                        executionStepIONTranscode.ProcessStatus = "Error";
                        executionStepIONTranscode.ErrorMessage = "This case is not handled:" + statusResponse.Status;
                        await UpdateExecutionStepStatus(LookupLists.ExecutionStepStatusType.Values.Error.ID, "Unhandled case", "Error", statusResponse.PercentageCompleted, errorCode);
                        TraceHelper.Write(traceContext, TraceLevel.Error, methodName, string.Format(TraceMessageFormat, error, ExecutionStep.ExecutionStepId, "IONOrder status was not recogonized"));
                        break;
                }
            }
            else
            {
                var invalidMessage = string.Format("Invalid status: JobId: {0}, Status: {1}", status.JobId, status.StatusCode);
                TraceHelper.Write(traceContext, TraceLevel.Error, methodName, invalidMessage);
                throw new ManagedServiceException(invalidMessage);
            }

            await sqlHelper.UpdateAsync<ExecutionStep_IONTranscode>(executionStepIONTranscode);
            return true;
        }

        public override StorageZone GetOutputStorageZone(Guid executionStepId)
        {
            string methodName = MethodInfo.GetCurrentMethod().ReflectedType.FullName + "." + MethodInfo.GetCurrentMethod().Name;

            var packageElementExternalProcessing = sqlHelper.Get<PackageElementExternalProcessing>().Where(a => a.IsActive && a.PackageElementID == PackageElement.PackageElementId &&
             (a.IONCompletionFollowOnTypeID == LookupLists.CompletionFollowOnType.Values.Full.ID //full
             || a.IONCompletionFollowOnTypeID == LookupLists.CompletionFollowOnType.Values.SingleWithOutput.ID)).FirstOrDefault(); //single with output

            if (packageElementExternalProcessing != null)
            {
                return GetDeliveryStorageZone().Result;
            }
            else
            {
                return OutputStorageZone;
            }
        }

        #endregion

        #region private methods

        private async Task<StorageZone> GetDeliveryStorageZone()
        {
            var deliveryPackageElement = await sqlHelper.Get<PackageElement_PackageElement>().Where(a => a.IsActive && a.ChildPackageElementId == PackageElement.PackageElementId && a.PackageSpecificationAssociationTypeId == LookupLists.PackageSpecificationAssociationType.Values.DeliveryScope.ID).FirstAsync();
            var deliveryExecutionSteps = await sqlHelper.RetrieveExecutionStepByPackageElement(deliveryPackageElement.ParentPackageElementId);
            return await sqlHelper.Get<StorageZone>().Where(a => a.IsActive && a.StorageZoneId == deliveryExecutionSteps.First().OutputStorageZoneId).SingleAsync();
        }

        private async Task<bool> IsSubmitInjector(List<TrajectoryAttribute> trajectoryAttributes)
        {
            string methodName = MethodInfo.GetCurrentMethod().ReflectedType.FullName + "." + MethodInfo.GetCurrentMethod().Name;

            // Call Injector directly if v1 
            // Look for TrajectoryAttribute
            var submitInjector = false;
            if (trajectoryAttributes != null && trajectoryAttributes.Count > 0)
            {
                var strSubmitInjector = GetTrajectoryAttributeValue(trajectoryAttributes, LookupLists.TrajectoryAttributeName.Values.ION_Transcode_Submit_Injector.ID);

                if (string.IsNullOrEmpty(strSubmitInjector) == false && strSubmitInjector.ToUpper() == "TRUE")
                {
                    submitInjector = true;
                }

                TraceHelper.Write(traceContext, TraceLevel.Informational, methodName, string.Format(TraceMessageFormat, info, "Trajectory Attributes - ION Transcode - Submit Injector:" + strSubmitInjector, _executionStepID));
            }

            var requestItem = RequestItem;

            // Look for ClientProfile 4NF - SendToInjector
            if (submitInjector == false)
            {
                if (requestItem != null && requestItem.ClientProfileId != null)
                {
                    var sendToInjectorAttributeValue = await sqlHelper.Get<EntityAttributeValue>().Where(a => a.IsActive && a.AttributeId == sendToInjectorAttributeID && a.EntityInstanceId == requestItem.ClientProfileId).SingleOrDefaultAsync();
                    if (sendToInjectorAttributeValue != null)
                    {
                        if (sendToInjectorAttributeValue.AttributeValueBit == true)
                            submitInjector = true;
                    }
                }
            }

            return submitInjector;
        }

        private async Task<string> CreateInjectorJob(Guid dbbJobId, string injectorURL)
        {
            string methodName = MethodInfo.GetCurrentMethod().ReflectedType.FullName + "." + MethodInfo.GetCurrentMethod().Name;

            /*
             * new payload field - StepType
             *     non-AWS = Transcode
             *     s3 and no ExecutionStep 4nf TranscodeProcessType (190C957A-4E8F-4229-ACDB-820DFBA49026) = Initialize
             *     s3 + ExecutionStep 4nf TranscodeProcessType = EZConvert or Rhozet or Digital Rapids
             *     
             *  new payload field = AdminPriority
             */
            //injectorURL = "http://localhost:12194/Service/SubmitJob.aspx";

            int adminPriority = RequestItem.AdminPriority.GetValueOrDefault();
            TraceHelper.Write(traceContext, TraceLevel.Informational, methodName, string.Format(TraceMessageFormat, info, "AdminPriority: " + adminPriority.ToString(), _executionStepID));

            var transcodeProcessTypeEAV = await sqlHelper.Get<EntityAttributeValue>().Where(a => a.IsActive && a.AttributeId == transcodeProcessTypeAttributeID && a.EntityInstanceId == ExecutionStep.ExecutionStepId).SingleOrDefaultAsync();
            string stepType = null;

            if (transcodeProcessTypeEAV != null)
            {
                //stepType = LLV
                var transcodeProcessTypeLLV = await sqlHelper.Get<LookupListValue>().Where(a => a.IsActive && a.LookupListValueId == transcodeProcessTypeEAV.AttributeValueUniqueIdentifier.GetValueOrDefault()).SingleAsync();

                stepType = transcodeProcessTypeLLV.LookupListValue1;

                if (stepType == "Tachyon" || stepType == "R128 Audio Normalization")
                    stepType = "Rhozet";
            }
            else
            {
                stepType = "Initialize";
            }
            TraceHelper.Write(traceContext, TraceLevel.Informational, methodName, string.Format(TraceMessageFormat, info, "Setting Payload.StepType:" + stepType, _executionStepID));


            // Submit 
            //var payload = "{\"DBBJobId\":\"" + dbbJobId.ToString() + "\", \"Provider\":\"DBB\", \"StepType\":\"" + stepType + "\", \"AdminPriority\":" + adminPriority + "}";
            var payload = await TranscodePayload.CreateTranscodePayload(dbbJobId, "DBB", stepType, adminPriority);

            var injectorClient = RestService.For<IInjectorService>(injectorURL);
            await injectorClient.SubmitJob(payload);

            return payload.ToString();
        }

        private async Task<string> CreateIONJob(Guid dbbJobId, string ionURL)
        {
            var ionServiceClient = CreateIONClient(ionURL);

            //create job request
            JobRequestMessage jobRequest = await JobRequestMessage.CreateJobRequestMessage(dbbJobId);
            ionServiceClient.SubmitJob(jobRequest);

            //serialize job request
            return jobRequest.ToString();
        }

        private IIONService CreateIONClient(string ionURL)
        {
            var uri = new Uri(ionURL);
            var binding = new BasicHttpBinding();

            binding.SendTimeout = TimeSpan.FromMinutes(120);
            binding.ReceiveTimeout = TimeSpan.FromMinutes(120);
            binding.MaxBufferSize = 2147483647;
            binding.MaxReceivedMessageSize = 2147483647;

            if (uri.Scheme == "https")
                binding.Security.Mode = BasicHttpSecurityMode.Transport;

            var endpoint = new EndpointAddress(uri);
            var channelFactory = new ChannelFactory<IIONService>(binding, endpoint);
            var ionServiceClient = channelFactory.CreateChannel();

            return ionServiceClient;
        }

        private async Task CpySrcSubLangAsTrgVideoBrnSub()
        {
            string methodName = MethodInfo.GetCurrentMethod().ReflectedType.FullName + "." + MethodInfo.GetCurrentMethod().Name;

            TraceHelper.Write(traceContext, TraceLevel.Informational, methodName, string.Format(TraceMessageFormat, beg, "CpySrcSubLangAsTrgVideoBrnSub", string.Empty));

            var ComponentSubtitleIDs = InboundWorkingSet.Components.Where(a => a.ComponentTypeId == LookupLists.ComponentType.Values.Subtitle.ID).Select(b => b.ComponentId).ToList();
            List<ComponentSubtitle> subtitleList = await sqlHelper.Get<ComponentSubtitle>().Where(a => a.IsActive && ComponentSubtitleIDs.Contains(a.ComponentId) && (a.SubtitleFileLayoutTypeId == LookupLists.SubtitleFileLayout.Values.Discrete.ID || a.SubtitleFileLayoutTypeId == LookupLists.SubtitleFileLayout.Values.ImageArchive.ID
                      || a.SubtitleFileLayoutTypeId == LookupLists.SubtitleFileLayout.Values.TextArchive.ID)).ToListAsync();


            ComponentSubtitle componentSubtitle = subtitleList.FirstOrDefault(x => x.ContentTypeId == LookupLists.SubtitleContentType.Values.Full.ID);

            if (componentSubtitle == null)
                componentSubtitle = subtitleList.FirstOrDefault(x => x.ContentTypeId == LookupLists.SubtitleContentType.Values.Forced.ID);

            List<Component> targetVideoList = OutboundWorkingSet.Components.Where(c => c.ComponentTypeId == LookupLists.ComponentType.Values.Video.ID).ToList();
            if (componentSubtitle != null && targetVideoList != null && targetVideoList.Count > 0)
            {
                TraceHelper.Write(traceContext, TraceLevel.Informational, methodName, string.Format(TraceMessageFormat, info, "componentSubtitle: ", componentSubtitle.ComponentId));
                foreach (Component targetVideo in targetVideoList)
                {
                    ComponentVideo componentVideo = await sqlHelper.Get<ComponentVideo>().Where(a => a.IsActive && a.ComponentId == targetVideo.ComponentId).SingleAsync();
                    if (componentVideo.BurnedInSubtitleLanguageTypeId != null)
                        continue;
                    TraceHelper.Write(traceContext, TraceLevel.Informational, methodName, string.Format(TraceMessageFormat, info, "componentVideo: ", componentVideo.ComponentId));
                    if (componentSubtitle.SubtitleLanguageTypeId != null && componentSubtitle.ContentTypeId == LookupLists.SubtitleContentType.Values.Full.ID)
                    {
                        componentVideo.BurnedInSubtitleLanguageTypeId = componentSubtitle.SubtitleLanguageTypeId;
                        TraceHelper.Write(traceContext, TraceLevel.Informational, methodName, string.Format(TraceMessageFormat, info, componentSubtitle.SubtitleLanguageTypeId + " SubtitleLanguage applied as BurnedInSubtitleLanguague to ", "componentVideo: " + componentVideo.ComponentId));
                        await sqlHelper.UpdateAsync<ComponentVideo>(componentVideo);
                    }
                    if (componentSubtitle.SubtitleLanguageTypeId != null && componentSubtitle.ContentTypeId == LookupLists.SubtitleContentType.Values.Forced.ID)
                    {
                        componentVideo.TextedLanguageTypeId = componentSubtitle.SubtitleLanguageTypeId;
                        componentVideo.ForcedSubtitleTypeId = LookupLists.ForcedSubtitleType.Values.Present.ID;
                        TraceHelper.Write(traceContext, TraceLevel.Informational, methodName, string.Format(TraceMessageFormat, info, componentSubtitle.SubtitleLanguageTypeId + " SubtitleLanguage applied as TextedLanguageType to ", "componentVideo: " + componentVideo.ComponentId));
                        await sqlHelper.UpdateAsync<ComponentVideo>(componentVideo);
                    }
                }
            }

            TraceHelper.Write(traceContext, TraceLevel.Informational, methodName, string.Format(TraceMessageFormat, end, "CpySrcSubLangAsTrgVideoBrnSub", string.Empty));

        }

        #endregion
    }
}

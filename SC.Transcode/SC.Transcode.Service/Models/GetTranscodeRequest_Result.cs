using System;
using System.Collections.Generic;
using System.Text;

namespace SC.Transcode.Service.Models
{
    public class GetTranscodeRequest_Result
    {
        public string OriginalClientProfileName { get; set; }
        public Guid? OutboundWorkingSetID { get; set; }
        public Guid? PackageElementID { get; set; }
        public string PackageElementName { get; set; }
        public string Partner { get; set; }
        public string RequestCategory { get; set; }
        public Guid? OriginalClientProfileID { get; set; }
        public Guid? RequestCategoryTypeID { get; set; }
        public DateTime? RequestDueDate { get; set; }
        public int? RequestItemDisplayCode { get; set; }
        public DateTime RequestItemDueDate { get; set; }
        public string Territory { get; set; }
        public string TitleName { get; set; }
        public string TranscodeProcessingType { get; set; }
        public int? RequestDisplayCode { get; set; }
        public string VineFormatType { get; set; }
        public Guid? JobId { get; set; }
        public bool IsEBSTranscode { get; set; }
        public int? AdminPriority { get; set; }
        public int? AlphaDisplayCode { get; set; }
        public string AlphaName { get; set; }
        public string AlphaDesc { get; set; }
        public bool CheckForBlacks { get; set; }
        public Guid? ClientProfileID { get; set; }
        public string ClientProfileName { get; set; }
        public Guid? Contentownerorganizationid { get; set; }
        public Guid? ContentProviderOrganizationID { get; set; }
        public bool? DoStitch { get; set; }
        public int? ElasticBlockStoreID { get; set; }
        public string ElasticBlockStoreName { get; set; }
        public string ElasticComputeCloudInstanceID { get; set; }
        public Guid? ExecutionStepID { get; set; }
        public string ExternalAlphaKey { get; set; }
        public string CPPurchaseType { get; set; }
        public bool? AddForensicWatermark { get; set; }
    }
}

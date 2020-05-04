using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace SC.Transcode.Service.Models
{
    [DataContract]
    public class RequestItemView
    {
        [DataMember]
        public string VineID { get; set; }
        [DataMember]
        public string CustomerName { get; set; }
        [DataMember]
        public Guid OrganizationCustomerID { get; set; }
        [DataMember]
        public string OrganizationPartnerName { get; set; }
        [DataMember]
        public Guid OrganizationPartnerID { get; set; }
        [DataMember]
        public string IngestRequestWorkflow { get; set; }
        [DataMember]
        public PackageElementView[] PackageElements { get; set; }
        [DataMember]
        public bool IsTimeEstimationComplete { get; set; }
        [DataMember]
        public bool IsCostEstimationComplete { get; set; }
        [DataMember]
        public decimal BillableCost { get; set; }
        [DataMember]
        public string QCOnOutputType { get; set; }
        [DataMember]
        public string StatusMessage { get; set; }
        [DataMember]
        public DateTime? DoNotDeliverBeforeDate { get; set; }
        [DataMember]
        public DateTime? EstimatedCompletionDate { get; set; }
        [DataMember]
        public bool IsForceDeliver { get; set; }
        [DataMember]
        public bool IsAutoDeliver { get; set; }
        [DataMember]
        public DateTime? SLAStartDate { get; set; }
        [DataMember]
        public string RequestItemType { get; set; }
        [DataMember]
        public string MediaPulseCustomerId { get; set; }
        [DataMember]
        public int DisplayCode { get; set; }
        [DataMember]
        public virtual DateTime? DueDate { get; set; }
        [DataMember]
        public string Status { get; set; }
        [DataMember]
        public int AdminPriority { get; set; }
        [DataMember]
        public bool DoStitch { get; set; }
        [DataMember]
        public int? EstimatedTimeForCompletion { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public string Territory { get; set; }
        [DataMember]
        public ComponentAttribute[] ComponentAttributes { get; set; }
        [DataMember]
        public DateTime? EstimatedDueDate { get; set; }
        [DataMember]
        public int UserPriority { get; set; }
        [DataMember]
        public DateTime? SLADueDate { get; set; }
        [DataMember]
        public int? AverageTimeForExternalTask { get; set; }
        [DataMember]
        public bool ResumedAfterAvailExpiry { get; set; }
    }
}

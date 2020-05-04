using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace SC.Transcode.Service.Models
{
    [DataContract]
    public class RequestView
    {
        [DataMember]
        public RequestItemView[] RequestLineItems { get; set; }
        [DataMember]
        public string PONumber { get; set; }
        [DataMember]
        public string MediaPulseJobID { get; set; }
        [DataMember]
        public string POComment { get; set; }
        [DataMember]
        public string Comment { get; set; }
        [DataMember]
        public bool IsFinancialApprovalRequired { get; set; }
        [DataMember]
        public string NonBillableReason { get; set; }
        [DataMember]
        public bool IsBillable { get; set; }
        [DataMember]
        public string StatusMessage { get; set; }
        [DataMember]
        public string Status { get; set; }
        [DataMember]
        public string WorkBreakdownStructure { get; set; }
        [DataMember(IsRequired = false)]
        public UserDetails ScheduledBy { get; set; }
        [DataMember]
        public string Territory { get; set; }
        [DataMember]
        public string ExternalPOOrganization { get; set; }
        [DataMember]
        public string ExternalPOStatus { get; set; }
        [DataMember]
        public string ExternalPONumber { get; set; }
        [DataMember]
        public string Account { get; set; }
        [DataMember]
        public string RequestType { get; set; }
        [DataMember]
        public DateTime? DueDate { get; set; }
        [DataMember]
        public DateTime DoNotDeliverBeforeDate { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public int? DisplayCode { get; set; }
        [DataMember]
        public Guid ID { get; set; }
        [DataMember]
        public string Department { get; set; }
        [DataMember]
        public string CategoryType { get; set; }
    }
}

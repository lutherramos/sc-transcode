using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace SC.Transcode.Service.Models
{
    [DataContract]
    public class ExecutionStepView
    {
        [DataMember]
        public Guid ID { get; set; }
        [DataMember]
        public int EstimatedProcessingTimeMinutes { get; set; }
        [DataMember]
        public string SystemIntegrationService { get; set; }
        [DataMember]
        public DateTime? ValidStartDate { get; set; }
        [DataMember]
        public DateTime? ValidEndDate { get; set; }
        [DataMember]
        public string[] ManufacturingFunctionTypes { get; set; }
        [DataMember]
        public WorkingSetView InputWorkingSet { get; set; }
        [DataMember]
        public WorkingSetView OutputWorkingSet { get; set; }
    }
}

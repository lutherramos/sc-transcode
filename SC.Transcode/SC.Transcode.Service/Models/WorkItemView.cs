using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace SC.Transcode.Service.Models
{
    [DataContract]
    public class WorkItemView
    {
        [DataMember]
        public Guid ID { get; set; }
        [DataMember]
        public ExecutionStepView[] ExecutionSteps { get; set; }
        [DataMember]
        public string Status { get; set; }
        [DataMember]
        public string StatusMessage { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace SC.Transcode.Service.Models
{
    [DataContract]
    public class RelatedExecutionStep
    {
        [DataMember]
        public Guid ExecutionStepID { get; set; }

        [DataMember]
        public string[] ManufacturingFunctionTypes { get; set; }
    }
}

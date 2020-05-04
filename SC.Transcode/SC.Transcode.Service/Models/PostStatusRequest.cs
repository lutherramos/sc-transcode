using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace SC.Transcode.Service.Models
{
    [DataContract]
    public class PostStatusRequest
    {
        [DataMember]
        public Guid JobId { get; set; }
        [DataMember]
        public ExecutionStatus JobStatus { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace SC.Transcode.Service.Models
{
    /// <summary>
    /// Data contract for status response
    /// </summary>
    [DataContract]
    public class StatusResponse
    {
        [DataMember]
        public Guid JobId { get; set; }

        [DataMember]
        public JobStatus Status { get; set; }

        [DataMember]
        public string StatusCode { get; set; }

        [DataMember]
        public string StatusMessage { get; set; }
        [DataMember]
        public int PercentageCompleted { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace SC.Transcode.Service.Models
{
    [DataContract]
    public class StatusRequest
    {
        [DataMember]
        public Guid JobId { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace SC.Transcode.Service.Models
{
    [DataContract]
    public enum ExecutionStatus
    {
        [EnumMember]
        Pending = 0,
        [EnumMember]
        InProgress = 1,
        [EnumMember]
        Complete = 2,
        [EnumMember]
        Error = 3,
        [EnumMember]
        Cancelled = 4
    }
}

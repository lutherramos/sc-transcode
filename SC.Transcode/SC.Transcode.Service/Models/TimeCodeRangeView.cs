using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace SC.Transcode.Service.Models
{
    [DataContract]
    public class TimeCodeRangeView
    {
        [DataMember]
        public Guid ID { get; set; }
        [DataMember]
        public string TimecodeRangeName { get; set; }
        [DataMember]
        public string TimecodeRangeDesc { get; set; }
        [DataMember]
        public string TimecodeIn { get; set; }
        [DataMember]
        public string TimecodeOut { get; set; }
        [DataMember]
        public string TimecodeRangeType { get; set; }
        [DataMember]
        public ComponentView Component { get; set; }
        [DataMember]
        public int FramesIn { get; set; }
        [DataMember]
        public int FramesOut { get; set; }
        [DataMember]
        public int MillisecondsIn { get; set; }
        [DataMember]
        public int MillisecondsOut { get; set; }
        [DataMember]
        public int DurationMilliseconds { get; set; }
        [DataMember]
        public string AssemblyDurationType { get; set; }
        [DataMember]
        public string AssemblyPositionType { get; set; }
    }
}

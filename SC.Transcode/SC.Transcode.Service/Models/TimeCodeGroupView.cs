using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace SC.Transcode.Service.Models
{
    [DataContract]
    public class TimeCodeGroupView
    {
        [DataMember]
        public Guid ID { get; set; }
        [DataMember]
        public string TimecodeGroupName { get; set; }
        [DataMember]
        public string TimecodeGroupDesc { get; set; }
        [DataMember]
        public string TimecodeGroupType { get; set; }
        [DataMember]
        public TimeCodeRangeView[] TimecodeRanges { get; set; }
    }
}

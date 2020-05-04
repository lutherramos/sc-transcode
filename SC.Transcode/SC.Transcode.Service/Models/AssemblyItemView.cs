using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace SC.Transcode.Service.Models
{
    [DataContract]
    public class AssemblyItemView
    {
        [DataMember]
        public Guid ID { get; set; }
        [DataMember]
        public string AssemblyItemName { get; set; }
        [DataMember]
        public string AssemblyItemDesc { get; set; }
        [DataMember]
        public string TimecodeGroup { get; set; }
        [DataMember]
        public int AssemblyItemOrder { get; set; }
        [DataMember]
        public TimeCodeRangeView SourceTimecodeRange { get; set; }
        [DataMember]
        public TimeCodeRangeView TargetTimecodeRange { get; set; }
        [DataMember]
        public string Type { get; set; }
    }
}

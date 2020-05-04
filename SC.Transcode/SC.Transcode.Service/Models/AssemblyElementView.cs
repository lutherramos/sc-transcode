using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace SC.Transcode.Service.Models
{
    [DataContract]
    public class AssemblyElementView
    {
        [DataMember]
        public Guid ID { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public string Position { get; set; }
        [DataMember]
        public string TimeUnits { get; set; }
        [DataMember]
        public string TimecodeGroup { get; set; }
        [DataMember]
        public int AssemblyItemOrder { get; set; }
        [DataMember]
        public bool IsInGroup { get; set; }
        [DataMember]
        public bool IsRequired { get; set; }
        [DataMember]
        public TimeCodeRangeView SourceTimecodeRange { get; set; }
        [DataMember]
        public TimeCodeRangeView TargetTimecodeRange { get; set; }
        [DataMember]
        public ComponentView[] Components { get; set; }
    }
}

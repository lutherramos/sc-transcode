using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace SC.Transcode.Service.Models
{
    [DataContract]
    public class ComponentGroupView
    {
        [DataMember]
        public Guid ID { get; set; }
        [DataMember]
        public int? DisplayCode { get; set; }
        [DataMember]
        public string ComponentGroupType { get; set; }
        [DataMember]
        public string ComponentGroupName { get; set; }
        [DataMember]
        public string ComponentGroupDesc { get; set; }
        [DataMember]
        public TimeCodeGroupView[] TimeCodeGroups { get; set; }
        [DataMember]
        public Guid[] ComponentIDs { get; set; }
    }
}

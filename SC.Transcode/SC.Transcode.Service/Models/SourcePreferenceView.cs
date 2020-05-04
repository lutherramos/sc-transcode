using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace SC.Transcode.Service.Models
{
    [DataContract]
    public class SourcePreferenceView
    {
        [DataMember]
        public Guid ID { get; set; }
        [DataMember]
        public string SourcePreferenceName { get; set; }
        [DataMember]
        public string SourcePreferenceDesc { get; set; }
    }
}

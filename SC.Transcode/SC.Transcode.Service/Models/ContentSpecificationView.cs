using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace SC.Transcode.Service.Models
{
    [DataContract]
    public class ContentSpecificationView
    {
        [DataMember]
        public Guid ID { get; set; }
        [DataMember]
        public string ContentSpecificationName { get; set; }
        [DataMember]
        public string ContentSpecificationDesc { get; set; }
        [DataMember]
        public AssemblySpecificationView[] AssemblySpecificationViews { get; set; }
        [DataMember]
        public SourcePreferenceView SourcePreference { get; set; }
        [DataMember]
        public TechnicalSpecificationView[] TechnicalSpecificationViews { get; set; }
    }
}

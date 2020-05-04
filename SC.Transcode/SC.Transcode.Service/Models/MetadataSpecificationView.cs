using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace SC.Transcode.Service.Models
{
    [DataContract]
    public class MetadataSpecificationView
    {
        [DataMember]
        public Guid ID { get; set; }
        [DataMember]
        public string MetadataSpecificationName { get; set; }
        [DataMember]
        public string MetadataSpecificationDesc { get; set; }
        [DataMember]
        public string MetadataSpecification { get; set; }
        [DataMember]
        public string TitleLevelType { get; set; }
        [DataMember]
        public Guid? TitleLevelTypeID { get; set; }
    }
}

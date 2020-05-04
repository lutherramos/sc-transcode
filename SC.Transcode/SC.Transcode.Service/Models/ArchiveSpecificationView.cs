using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace SC.Transcode.Service.Models
{
    [DataContract]
    public class ArchiveSpecificationView
    {
        [DataMember]
        public Guid ID { get; set; }
        [DataMember]
        public string ArchiveSpecificationName { get; set; }
        [DataMember]
        public string ArchiveSpecificationDesc { get; set; }
        [DataMember]
        public string ManufacturingFunction { get; set; }
    }
}

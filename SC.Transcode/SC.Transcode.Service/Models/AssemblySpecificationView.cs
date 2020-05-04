using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace SC.Transcode.Service.Models
{
    [DataContract]
    public class AssemblySpecificationView
    {
        [DataMember]
        public Guid ID { get; set; }
        [DataMember]
        public string AssemblySpecificationName { get; set; }
        [DataMember]
        public string AssemblySpecificationDesc { get; set; }
        [DataMember]
        public AssemblyItemView[] AssemblyItems { get; set; }
        [DataMember]
        public string AssemblyMode { get; set; }
    }
}

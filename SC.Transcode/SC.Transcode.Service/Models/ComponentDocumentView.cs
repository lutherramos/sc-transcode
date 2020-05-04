using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace SC.Transcode.Service.Models
{
    [DataContract]
    public class ComponentDocumentView
    {
        [DataMember]
        public Guid ID { get; set; }
        [DataMember]
        public string ComponentDocumentName { get; set; }
        [DataMember]
        public string ComponentDocumentDesc { get; set; }
        [DataMember]
        public string ComponentDocumentType { get; set; }
        [DataMember]
        public string SupportingMaterialPackage { get; set; }
    }
}

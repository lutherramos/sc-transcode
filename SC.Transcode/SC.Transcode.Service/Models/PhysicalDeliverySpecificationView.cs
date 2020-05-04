using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace SC.Transcode.Service.Models
{
    [DataContract]
    public class PhysicalDeliverySpecificationView
    {
        [DataMember]
        public Guid ID { get; set; }
        [DataMember]
        public string ClientContact { get; set; }
        [DataMember]
        public string Destination { get; set; }
        [DataMember]
        public string MediaRequired { get; set; }
        [DataMember]
        public string FormatRequired { get; set; }
        [DataMember]
        public string EncryptionRequired { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace SC.Transcode.Service.Models
{
    [DataContract]
    public class DeliverySpecificationView
    {
        [DataMember]
        public Guid ID { get; set; }
        [DataMember]
        public string DeliverySpecificationName { get; set; }
        [DataMember]
        public string DeliverySpecificationDesc { get; set; }
        [DataMember]
        public string ManufacturingFunction { get; set; }
        [DataMember]
        public string DeliveryMechanism { get; set; }
        [DataMember]
        public PhysicalDeliverySpecificationView PhysicalDeliverySpecification { get; set; }
    }
}

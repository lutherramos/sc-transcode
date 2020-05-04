using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace SC.Transcode.Service.Models
{
    [DataContract]
    public class PackageElementView
    {
        [DataMember]
        public Guid TechnicalSpecificationID { get; set; }
        [DataMember]
        public string BatonQCType { get; set; }
        [DataMember(IsRequired = true)]
        public string PackageElementStatus { get; set; }
        [DataMember(IsRequired = false)]
        public List<Guid> PackageComponents { get; set; }
        [DataMember(IsRequired = false)]
        public MetadataSpecificationView MetadataSpecification { get; set; }
        [DataMember(IsRequired = false)]
        public ContentSpecificationView ContentSpecification { get; set; }
        [DataMember]
        public int TimecodeStart { get; set; }
        [DataMember(IsRequired = false)]
        public DeliverySpecificationView DeliverySpecification { get; set; }
        [DataMember]
        public AssemblyElementView[] AssemblyElements { get; set; }
        [DataMember]
        public ManuFacturingPlanView ManuFacturingPlan { get; set; }
        [DataMember]
        public PackageElementType PackageElementType { get; set; }
        [DataMember]
        public string PackageElementName { get; set; }
        [DataMember]
        public Guid PackageElementID { get; set; }
        [DataMember]
        public Guid ID { get; set; }
        [DataMember(IsRequired = false)]
        public SupportingMaterialSpecificationView SupportingMaterialSpecification { get; set; }
        [DataMember]
        public int Ordinal { get; set; }
    }
}

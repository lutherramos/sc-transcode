using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace SC.Transcode.Service.Models
{
    [DataContract]
    public class PackageSpecificationView
    {
        [DataMember]
        public ContentSpecificationView ContentSpecification { get; set; }
        [DataMember]
        public SupportingMaterialSpecificationView SupportingMaterialSpecification { get; set; }
        [DataMember]
        public DeliverySpecificationView DeliverySpecification { get; set; }
        [DataMember]
        public bool IsEnabled { get; set; }
        [DataMember]
        public bool IsRequiredForForceDelivery { get; set; }
        [DataMember]
        public bool IsAutoDeliver { get; set; }
        [DataMember]
        public bool IsDoChecksum { get; set; }
        [DataMember]
        public bool IsArchived { get; set; }
        [DataMember]
        public bool IsQCRequired { get; set; }
        [DataMember]
        public string QCAutomationType { get; set; }
        [DataMember]
        public string ChecksumType { get; set; }
        [DataMember]
        public string ArchiveSourcePath { get; set; }
        [DataMember]
        public string RelativeOutputPath { get; set; }
        [DataMember]
        public string WatermarkType { get; set; }
        [DataMember]
        public string EncryptionType { get; set; }
        [DataMember]
        public string PackageSpecificationStatus { get; set; }
        [DataMember]
        public string PackageSpecificationName { get; set; }
        [DataMember]
        public string PackageSpecificationType { get; set; }
        [DataMember]
        public Guid ID { get; set; }
        [DataMember]
        public MetadataSpecificationView MetadataSpecification { get; set; }
        [DataMember]
        public ArchiveSpecificationView ArchiveSpecification { get; set; }
    }
}

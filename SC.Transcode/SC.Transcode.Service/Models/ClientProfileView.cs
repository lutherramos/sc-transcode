using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace SC.Transcode.Service.Models
{
    [DataContract]
    public class ClientProfileView
    {

        [DataMember]
        public Guid ContractingEntityID { get; set; }
        [DataMember]
        public string StoreCode { get; set; }
        [DataMember]
        public string OriginalClientProfileName { get; set; }
        [DataMember]
        public Guid OriginalClientProfileID { get; set; }
        [DataMember]
        public PackageSpecificationView[] PackageSpecifications { get; set; }
        [DataMember]
        public string DefaultLanguage { get; set; }
        [DataMember]
        public string ServiceDescription { get; set; }
        [DataMember]
        public string ContractingEntityName { get; set; }
        [DataMember]
        public DateTime? ClientProfileOnBoardDate { get; set; }
        [DataMember]
        public string DistributionPlatform { get; set; }
        [DataMember]
        public string ClientProfileCurrentStatus { get; set; }
        [DataMember]
        public string Territory { get; set; }
        [DataMember]
        public string ClientProfileType { get; set; }
        [DataMember]
        public string ClientProfileDesc { get; set; }
        [DataMember]
        public string ClientProfileName { get; set; }
        [DataMember]
        public Guid ID { get; set; }
        [DataMember]
        public string ClientProfileOnboardingStatus { get; set; }
        [DataMember]
        public bool DoStitch { get; set; }
    }
}

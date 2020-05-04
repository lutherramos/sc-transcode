using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace SC.Transcode.Service.Models
{
    [DataContract]
    public class TechnicalSpecificationView
    {
        [DataMember]
        public Guid ID { get; set; }
        [DataMember]
        public string TechnicalSpecificationName { get; set; }
        [DataMember]
        public string TechnicalSpecificationDesc { get; set; }
        [DataMember]
        public string CurrentStatus { get; set; }
        [DataMember]
        public string OnboardingStatus { get; set; }
        [DataMember]
        public string DistributionPlatform { get; set; }
        [DataMember]
        public ProfileCodeMapView[] profileCodeMaps { get; set; }
        [DataMember]
        public int PreferenceOrder { get; set; }
    }
}

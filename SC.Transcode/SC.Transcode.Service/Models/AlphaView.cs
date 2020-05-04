using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace SC.Transcode.Service.Models
{
    [DataContract]
    public class AlphaView
    {
        [DataMember]
        public string AlphaNotes { get; set; }
        [DataMember]
        public string RatingType { get; set; }
        [DataMember]
        public string TerritoryType { get; set; }
        [DataMember]
        public string ScreenAspectRatio { get; set; }
        [DataMember]
        public string Copyright { get; set; }
        [DataMember]
        public int RuntimeMinutes { get; set; }
        [DataMember]
        public bool IsFirstReleaseMedia { get; set; }
        [DataMember]
        public string RatingReason { get; set; }
        [DataMember]
        public string MadeForMedia { get; set; }
        [DataMember]
        public string AlphaStatus { get; set; }
        [DataMember]
        public string AlphaTypeID { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public Guid ID { get; set; }
        [DataMember]
        public string ExternalAlphaKey { get; set; }
        [DataMember]
        public int AlphaDisplayCode { get; set; }
    }
}

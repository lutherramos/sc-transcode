using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace SC.Transcode.Service.Models
{
    [DataContract]
    public class ProfileCodeMapView
    {
        [DataMember]
        public string SystemIntegration { get; set; }
        [DataMember]
        public string ProfileCode { get; set; }
        [DataMember]
        public int Ordinal { get; set; }
    }
}

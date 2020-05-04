using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace SC.Transcode.Service.Models
{
    [DataContract]
    public class ComponentAttribute
    {
        [DataMember]
        public string Language { get; set; }
        [DataMember]
        public int ComponentDisplayCode { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace SC.Transcode.Service.Models
{
    [DataContract]
    public class WorkingSetView
    {
        [DataMember]
        public Guid ID { get; set; }
        [DataMember]
        public ComponentView[] Components { get; set; }
    }
}

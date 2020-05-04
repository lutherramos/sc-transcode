using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace SC.Transcode.Service.Models
{
    [DataContract]
    public class ComponentAudioChannelView : ComponentView
    {
        [DataMember]
        public string ComponentAudioChannelName { get; set; }
        [DataMember]
        public string ComponentAudioChannelDesc { get; set; }
        [DataMember]
        public string AudioChannelAssignment { get; set; }
        [DataMember]
        public int? AudioChannelInFile { get; set; }
        [DataMember]
        public int? AudioChannelOnTape { get; set; }
        [DataMember]
        public int? AudioStreamInFile { get; set; }
        [DataMember]
        public int? AudioChannelInStream { get; set; }
        [DataMember]
        public int NumberOfChannelsInFile { get; set; }
    }
}

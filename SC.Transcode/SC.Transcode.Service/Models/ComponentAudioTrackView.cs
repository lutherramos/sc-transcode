using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace SC.Transcode.Service.Models
{
    [DataContract]
    public class ComponentAudioTrackView
    {
        [DataMember]
        public int ExportDirectoryBaseIndex { get; set; }
        [DataMember]
        public int NumberOfChannels { get; set; }
        [DataMember]
        public bool IsMultiLanguage { get; set; }
        [DataMember]
        public string AudioElementType { get; set; }
        [DataMember]
        public string AudioSpeed { get; set; }
        [DataMember]
        public string AudioContent { get; set; }
        [DataMember]
        public string AudioMix { get; set; }
        [DataMember]
        public long RuntimeInMilliseconds { get; set; }
        [DataMember]
        public string AudioContainer { get; set; }
        [DataMember]
        public string AudioBitsPerSample { get; set; }
        [DataMember]
        public bool IsOriginalVersion { get; set; }
        [DataMember]
        public int MaxAudioBitrateKbps { get; set; }
        [DataMember]
        public string AudioFileLayout { get; set; }
        [DataMember]
        public bool IsConstantBitrate { get; set; }
        [DataMember]
        public string AudioTrackSampleRate { get; set; }
        [DataMember]
        public string AudioTrackConfiguration { get; set; }
        [DataMember]
        public int AudioBitrateKbps { get; set; }
        [DataMember]
        public string Language { get; set; }
        [DataMember]
        public string AudioCodec { get; set; }
        [DataMember]
        public string ComponentAudioTrackViewDesc { get; set; }
        [DataMember]
        public string ComponentAudioTrackViewName { get; set; }
        [DataMember]
        public Guid ID { get; set; }
        [DataMember]
        public int MinAudioBitrateKbps { get; set; }
        [DataMember]
        public ComponentAudioChannelView[] ComponentAudioChannels { get; set; }
    }
}

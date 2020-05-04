using System;
using System.Collections.Generic;
using System.Text;

namespace SC.Transcode.Service.Models
{
    public class MediaInfoAudioChannels
    {
        public String FilePath { get; set; }
        public String ChannelLayout { get; set; }
        public Int32 ChannelInFile { get; set; }
        public Int32 ChannelInStream { get; set; }
        public Int32 StreamInFile { get; set; }
    }
}

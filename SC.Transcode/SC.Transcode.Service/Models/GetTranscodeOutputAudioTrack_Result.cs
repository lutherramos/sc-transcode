using System;
using System.Collections.Generic;
using System.Text;

namespace SC.Transcode.Service.Models
{
    public class GetTranscodeOutputAudioTrack_Result
    {
        public string AudioContent { get; set; }
        public string AudioTrackConfiguration { get; set; }
        public Guid? ComponentID { get; set; }
        public string ComponentName { get; set; }
        public string Language { get; set; }
        public int? NumberOfChannels { get; set; }
        public string VineFormatType { get; set; }
    }
}

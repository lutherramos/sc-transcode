using System;
using System.Collections.Generic;
using System.Text;

namespace SC.Transcode.Service.Models
{
    public class GetTranscodeOutputAudioChannel_Result
    {
        public string AudioChannelAssignment { get; set; }
        public int? AudioChannelInFile { get; set; }
        public int? AudioChannelOnTape { get; set; }
        public int? AudioStreamInFile { get; set; }
        public Guid? ComponentID { get; set; }
        public string ComponentName { get; set; }
        public Guid? ParentComponentID { get; set; }
        public string VineFormatType { get; set; }
    }
}

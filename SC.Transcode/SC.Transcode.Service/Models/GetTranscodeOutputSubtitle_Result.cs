using System;
using System.Collections.Generic;
using System.Text;

namespace SC.Transcode.Service.Models
{
    public class GetTranscodeOutputSubtitle_Result
    {
        public string ActivePictureAspectRatioType { get; set; }
        public string ActivePictureFormatType { get; set; }
        public string CCStandardType { get; set; }
        public Guid? ComponentID { get; set; }
        public string ContentType { get; set; }
        public string FrameRateType { get; set; }
        public string HorizontalResolution { get; set; }
        public string ScreenAspectRatioType { get; set; }
        public string SubtitleFormatType { get; set; }
        public string SubtitleLanguageType { get; set; }
        public string TextEncodingType { get; set; }
        public string VerticalResolution { get; set; }
        public string VineFormatType { get; set; }
    }
}

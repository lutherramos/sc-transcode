using System;
using System.Collections.Generic;
using System.Text;

namespace SC.Transcode.Service.Models
{
    public class GetTranscodeOutputVideo_Result
    {
        public string TextInPicture { get; set; }
        public string TextedLanguage { get; set; }
        public string SourceVideoFileSpec { get; set; }
        public string ScreenAspectRatio { get; set; }
        public string ScanMode { get; set; }
        public long? RuntimeInMilliseconds { get; set; }
        public string MadeForEye { get; set; }
        public bool? IsDerivative { get; set; }
        public int? FrameSizeVertical { get; set; }
        public string VideoCodec { get; set; }
        public int? FrameSizeHorizontal { get; set; }
        public string ForcedSubtitle { get; set; }
        public Guid? ComponentID { get; set; }
        public string BurnedInSubtitleLanguage { get; set; }
        public int? ActiveTopLeftY { get; set; }
        public int? ActiveTopLeftX { get; set; }
        public string ActivePictureFormat { get; set; }
        public string ActivePictureAspectRatio { get; set; }
        public int? ActiveBottomRightY { get; set; }
        public int? ActiveBottomRightX { get; set; }
        public string FrameRate { get; set; }
        public string VineFormatType { get; set; }
    }
}

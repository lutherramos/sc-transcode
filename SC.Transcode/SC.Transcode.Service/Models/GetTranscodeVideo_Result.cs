using System;
using System.Collections.Generic;
using System.Text;

namespace SC.Transcode.Service.Models
{
    public class GetTranscodeVideo_Result
    {
        public string TextInPicture { get; set; }
        public string VideoCodec { get; set; }
        public string VineFormatType { get; set; }
        public int? ST2086Gx { get; set; }
        public int? ST2086Gy { get; set; }
        public int? ST2086Bx { get; set; }
        public int? ST2086By { get; set; }
        public int? ST2086Rx { get; set; }
        public int? ST2086Ry { get; set; }
        public int? ST2086WPx { get; set; }
        public int? ST2086WPy { get; set; }
        public int? ST2086Lmin { get; set; }
        public int? ST2086Lmax { get; set; }
        public int? MaxCLL { get; set; }
        public int? MaxFALL { get; set; }
        public string TextedLanguage { get; set; }
        public string SourceVideoFileSpec { get; set; }
        public string ScreenAspectRatio { get; set; }
        public string ScanMode { get; set; }
        public int? ActiveBottomRightX { get; set; }
        public int? ActiveBottomRightY { get; set; }
        public string ActivePictureAspectRatio { get; set; }
        public string ActivePictureFormat { get; set; }
        public int? ActiveTopLeftX { get; set; }
        public int? ActiveTopLeftY { get; set; }
        public string BurnedInSubtitleLanguage { get; set; }
        public string TransferFunction { get; set; }
        public Guid? ComponentID { get; set; }
        public string FrameRate { get; set; }
        public int? FrameSizeHorizontal { get; set; }
        public int? FrameSizeVertical { get; set; }
        public bool? IsDerivative { get; set; }
        public bool? IsDubCardPresent { get; set; }
        public string MadeForEye { get; set; }
        public long? RuntimeInMilliseconds { get; set; }
        public string ForcedSubtitle { get; set; }
        public string DynamicRange { get; set; }
    }
}

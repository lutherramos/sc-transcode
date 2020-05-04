using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace SC.Transcode.Service.Models
{
    [DataContract]
    public class ComponentVideoView
    {
        [DataMember]
        public string VideoBitDepth { get; set; }
        [DataMember]
        public string ColorStandard { get; set; }
        [DataMember]
        public string TextedLanguage { get; set; }
        [DataMember]
        public string TextInPicture { get; set; }
        [DataMember]
        public string BurnedInSubtitleLanguage { get; set; }
        [DataMember]
        public int KeyframeSpacing { get; set; }
        [DataMember]
        public int HorizontalResolutionPixels { get; set; }
        [DataMember]
        public int VerticalResolutionPixels { get; set; }
        [DataMember]
        public bool IsVBIPresent { get; set; }
        [DataMember]
        public string FieldDominance { get; set; }
        [DataMember]
        public bool IsDropFrame { get; set; }
        [DataMember]
        public bool IsTextlessAtTail { get; set; }
        [DataMember]
        public int ExportDirectoryBaseIndex { get; set; }
        [DataMember]
        public string ForcedSubtitle { get; set; }
        [DataMember]
        public bool IsOriginalVersion { get; set; }
        [DataMember]
        public bool HasForcedSubsInOriginalVersion { get; set; }
        [DataMember]
        public bool HasTextInOriginalVersion { get; set; }
        [DataMember]
        public bool HasTextlessAvailable { get; set; }
        [DataMember]
        public bool IsDubCardPresent { get; set; }
        [DataMember]
        public string DubCardLanguageType { get; set; }
        [DataMember]
        public bool IsConstantBitrate { get; set; }
        [DataMember]
        public string FieldOrder { get; set; }
        [DataMember]
        public string ScanMode { get; set; }
        [DataMember]
        public string ChromaFormat { get; set; }
        [DataMember]
        public Guid ID { get; set; }
        [DataMember]
        public string ComponentVideoName { get; set; }
        [DataMember]
        public string ComponentVideoDesc { get; set; }
        [DataMember]
        public string ScreenAspectRatio { get; set; }
        [DataMember]
        public string VideoColorSpace { get; set; }
        [DataMember]
        public string FrameRate { get; set; }
        [DataMember]
        public string PixelAspectRatio { get; set; }
        [DataMember]
        public string VideoStandard { get; set; }
        [DataMember]
        public string VideoCodec { get; set; }
        [DataMember]
        public string VideoContainer { get; set; }
        [DataMember]
        public int VideoBitrateKbps { get; set; }
        [DataMember]
        public string ActivePictureFormat { get; set; }
        [DataMember]
        public string FullFrameProcess { get; set; }
        [DataMember]
        public string NativeFrameRate { get; set; }
        [DataMember]
        public long RuntimeInMilliseconds { get; set; }
        [DataMember]
        public int ActiveTopLeftX { get; set; }
        [DataMember]
        public int ActiveTopLeftY { get; set; }
        [DataMember]
        public int ActiveBottomRightX { get; set; }
        [DataMember]
        public int ActiveBottomRightY { get; set; }
        [DataMember]
        public bool IsCenterCutSafe { get; set; }
        [DataMember]
        public string ActivePictureAspectRatio { get; set; }
        [DataMember]
        public string MadeForEye { get; set; }
        [DataMember]
        public string StereoscopicFormat { get; set; }
    }
}

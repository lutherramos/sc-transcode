using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace SC.Transcode.Service.Models
{
    [DataContract]
    public class ComponentSubtitleView
    {
        [DataMember]
        public string CCStandardType { get; set; }
        [DataMember]
        public string ActivePictureFormatType { get; set; }
        [DataMember]
        public string ActivePictureAspectRatioType { get; set; }
        [DataMember]
        public string ScreenAspectRatioType { get; set; }
        [DataMember]
        public string HorizontalResolution { get; set; }
        [DataMember]
        public string VerticalResolution { get; set; }
        [DataMember]
        public string FrameRateType { get; set; }
        [DataMember]
        public string SubtitleLanguageType { get; set; }
        [DataMember]
        public string ContentType { get; set; }
        [DataMember]
        public string SubTitleFormatType { get; set; }
        [DataMember]
        public string TextEncodingType { get; set; }
        [DataMember]
        public string SubtitleTimingFileFormatType { get; set; }
        [DataMember]
        public string SubtitleImageFileFormatType { get; set; }
        [DataMember]
        public string VaultedFileType { get; set; }
        [DataMember]
        public string SubtitleFileLayoutType { get; set; }
        [DataMember]
        public string ComponentSubtitleName { get; set; }
        [DataMember]
        public string ComponentSubtitleDesc { get; set; }
    }
}

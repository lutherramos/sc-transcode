using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace SC.Transcode.Service.Models
{
    [DataContract]
    public class ComponentImageView
    {
        [DataMember]
        public Guid ID { get; set; }
        [DataMember]
        public string ComponentImageName { get; set; }
        [DataMember]
        public string ComponentImageDesc { get; set; }
        [DataMember]
        public string ComponentImageType { get; set; }
        [DataMember]
        public int VerticalResolutionPixels { get; set; }
        [DataMember]
        public int HorizontalResolutionPixels { get; set; }
        [DataMember]
        public int ImageDotsPerInch { get; set; }
        [DataMember]
        public string ImageFormat { get; set; }
        [DataMember]
        public string ImageColorSpace { get; set; }
        [DataMember]
        public string ImageBitDepth { get; set; }
        [DataMember]
        public string TextedLanguage { get; set; }
        [DataMember]
        public string ScreenAspectRatio { get; set; }
        [DataMember]
        public string ActivePictureAspectRatio { get; set; }
        [DataMember]
        public string PixelAspectRatio { get; set; }
        [DataMember]
        public string ActivePictureFormat { get; set; }
    }
}

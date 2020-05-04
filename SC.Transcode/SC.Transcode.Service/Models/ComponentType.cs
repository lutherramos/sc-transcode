using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace SC.Transcode.Service.Models
{
    [DataContract]
    public enum ComponentType
    {
        Audio = 0,
        Audio_Channel_Component = 1,
        Document = 2,
        Audio_Track = 3,
        Closed_Caption = 4,
        Image = 5,
        Subtitle = 6,
        Video = 7
    }
}

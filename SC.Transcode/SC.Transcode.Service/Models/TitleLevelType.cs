using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace SC.Transcode.Service.Models
{
    [DataContract]
    public enum TitleLevelType
    {
        Episode = 0,
        NonEpisodic = 1,
        Season = 2,
        Series = 3,
        Series_Two_Level = 4,
        Unassigned = 5
    }
}

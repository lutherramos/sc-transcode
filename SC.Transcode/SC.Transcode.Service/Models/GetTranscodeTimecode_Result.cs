using System;
using System.Collections.Generic;
using System.Text;

namespace SC.Transcode.Service.Models
{
    public class GetTranscodeTimecode_Result
    {
        public Guid? TimecodeGroupID { get; set; }
        public string TimecodeGroupType { get; set; }
        public Guid? TimecodeGroupTypeID { get; set; }
        public string TimecodeIn { get; set; }
        public string TimecodeOut { get; set; }
        public string TimecodeRangeDesc { get; set; }
        public Guid? TimecodeRangeID { get; set; }
        public string TimecodeRangeName { get; set; }
        public string TimecodeRangeType { get; set; }
        public Guid? TimecodeRangeTypeID { get; set; }
    }
}

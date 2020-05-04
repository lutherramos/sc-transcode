using System;
using System.Collections.Generic;
using System.Text;

namespace SC.Transcode.Service.Models
{
    public class GetTranscodeOutputFile_Result
    {
        public string ComponentEntityType { get; set; }
        public Guid? ComponentID { get; set; }
        public string ComponentName { get; set; }
        public string ComponentType { get; set; }
        public string FileFormatType { get; set; }
        public string OutputFileName { get; set; }
        public Guid? StorageZoneID { get; set; }
        public string VineFormatType { get; set; }
    }
}

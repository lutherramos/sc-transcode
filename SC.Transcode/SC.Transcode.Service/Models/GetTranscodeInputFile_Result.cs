using System;
using System.Collections.Generic;
using System.Text;

namespace SC.Transcode.Service.Models
{
    public class GetTranscodeInputFile_Result
    {
        public string ComponentAssetType { get; set; }
        public string ComponentEntityType { get; set; }
        public Guid? ComponentID { get; set; }
        public string ComponentName { get; set; }
        public string ComponentType { get; set; }
        public string FileFormatType { get; set; }
        public string InputFileName { get; set; }
        public string MadeForEye { get; set; }
        public Guid? StorageZoneID { get; set; }
        public string StorageZoneName { get; set; }
        public string VineFormatType { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace SC.Transcode.Service.Models
{
    [DataContract]
    public class FileLocationView
    {
        [DataMember]
        public Guid ID { get; set; }
        [DataMember(IsRequired = true)]
        public string FilePath { get; set; }
        [DataMember]
        public string FileName { get; set; }
        [DataMember]
        public string FileExtension { get; set; }
        [DataMember]
        public Guid StorageZoneId { get; set; }
    }
}

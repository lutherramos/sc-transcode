using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace SC.Transcode.Service.Models
{
    [DataContract]
    public class FileDescriptorView
    {
        [DataMember]
        public Guid ID { get; set; }
        [DataMember]
        public FileLocationView[] FileLocations { get; set; }
        [DataMember]
        public string FileDescriptorName { get; set; }
        [DataMember]
        public DateTime? ExpirationDate { get; set; }
        [DataMember]
        public string ChecksumValue { get; set; }
        [DataMember]
        public string FileFormat { get; set; }
        [DataMember]
        public long FileSizeInBytes { get; set; }
    }
}

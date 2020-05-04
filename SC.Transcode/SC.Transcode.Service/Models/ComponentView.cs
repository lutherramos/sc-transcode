using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace SC.Transcode.Service.Models
{
    [DataContract]
    public class ComponentView
    {
        [DataMember]
        public ComponentDocumentView ComponentDocument { get; set; }
        [DataMember]
        public ComponentImageView ComponentImage { get; set; }
        [DataMember]
        public ComponentAudioTrackView ComponentAudioTrack { get; set; }
        [DataMember]
        public ComponentVideoView ComponentVideo { get; set; }
        [DataMember]
        public string VineFormatType { get; set; }
        [DataMember]
        public string MasterSpecificationName { get; set; }
        [DataMember]
        public Guid MasterSpecificationID { get; set; }
        [DataMember]
        public Guid ContentProviderOrganizationID { get; set; }
        [DataMember]
        public bool IsTreatedAsFile { get; set; }
        [DataMember]
        public bool IsDerivative { get; set; }
        [DataMember]
        public ComponentSubtitleView ComponentSubtitle { get; set; }
        [DataMember]
        public DateTime? ComponentDueDate { get; set; }
        [DataMember]
        public ComponentEntityType EntityType { get; set; }
        [DataMember]
        public string ComponentStatus { get; set; }
        [DataMember]
        public ComponentType ComponentType { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public string ComponentAssetType { get; set; }
        [DataMember]
        public string UsageRestriction { get; set; }
        [DataMember]
        public string ExternalComponentKey { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public int? DisplayCode { get; set; }
        [DataMember]
        public Guid ID { get; set; }
        [DataMember]
        public string TapeBarcode { get; set; }
        [DataMember]
        public FileDescriptorView[] FileDescriptors { get; set; }
    }
}

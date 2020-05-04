using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace SC.Transcode.Service.Models
{
    [DataContract]
    public class TitleView
    {
        [DataMember]
        public Guid ContentOwnerOrganizationName { get; set; }
        [DataMember]
        public string WalkerID { get; set; }
        [DataMember]
        public Guid ContentOwnerOrganizationID { get; set; }
        [DataMember]
        public Guid ContentProviderOrganizationID { get; set; }
        [DataMember]
        public int TitleDisplayCode { get; set; }
        [DataMember]
        public string OriginalLanguage { get; set; }
        [DataMember]
        public string MadeForMedia { get; set; }
        [DataMember]
        public int ProductionYear { get; set; }
        [DataMember]
        public string EpisodeNo { get; set; }
        [DataMember]
        public AlphaView[] Alphas { get; set; }
        [DataMember]
        public string ExternalTitleKey { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public Guid EntityClassID { get; set; }
        [DataMember]
        public string TitleStatus { get; set; }
        [DataMember]
        public Guid? TitleLevelTypeID { get; set; }
        [DataMember]
        public TitleLevelType TitleLevel { get; set; }
        [DataMember]
        public Guid ID { get; set; }
        [DataMember]
        public string TitleType { get; set; }
        [DataMember]
        public string SeasonNo { get; set; }
    }
}

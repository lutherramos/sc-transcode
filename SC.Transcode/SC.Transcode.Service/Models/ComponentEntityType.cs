using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace SC.Transcode.Service.Models
{
    [DataContract]
    public enum ComponentEntityType
    {
        QCMaterial = 0,
        Assembly = 1,
        LoggingProxy = 2,
        Metadata = 3,
        Package = 4,
        Core = 5,
        Master = 6,
        Supporting = 7,
        AncillaryMaterial = 8
    }
}

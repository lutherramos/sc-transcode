using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace SC.Transcode.Service.Models
{
    [DataContract]
    public enum PackageElementType
    {
        Assembly = 0,
        Delivery = 1,
        Metadata = 2,
        Package = 3,
        Support = 4,
        Technical = 5
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace SC.Transcode.Service.Models
{
    public class InjectorPostStatusRequest
    {
        public Guid DBBJobId { get; set; }
        public string Status { get; set; }
    }
}

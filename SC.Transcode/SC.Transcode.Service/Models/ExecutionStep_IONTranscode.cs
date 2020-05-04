using System;
using System.Collections.Generic;
using System.Text;

namespace SC.Transcode.Service.Models
{
    public class ExecutionStep_IONTranscode
    {
        public Guid ExecutionStep_IONTranscodeID { get; set; }
        public Guid ExecutionStepID { get; set; }
        public int? PercentComplete { get; set; }
        public string ProcessStatus { get; set; }
        public int? JobID { get; set; }
        public string ErrorMessage { get; set; }
        public string JobXML { get; set; }
        public bool IsActive { get; set; }
        public DateTime UpdatedDate { get; set; }
        public Guid UpdatedByID { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid CreatedByID { get; set; }
    }
}

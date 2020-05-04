using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace SC.Transcode.Service.Models
{
    [DataContract]
    public class JobRequestMessage
    {
        [DataMember]
        public Guid JobId { get; set; }

        [DataMember]
        public ClientProfileView Profile { get; set; }

        [DataMember]
        public TitleView Title { get; set; }

        [DataMember]
        public RequestView Request { get; set; }

        [DataMember]
        public RelatedExecutionStep[] RelatedExecutionSteps { get; set; }

        /// </summary>
        /// <value>An <see cref="List"/> that contains ComponentGroups.</value>
        [DataMember]
        public ComponentGroupView[] InputComponentGroups { get; set; }

        [DataMember]
        [XmlArray("CancellableIONOrders")]
        [XmlArrayItem("JobId")]
        public Guid[] CancellableIONOrders { get; set; }

        private JobRequestMessage()
        {

        }

        public override string ToString()
        {
            string result = null;

            using (MemoryStream stream = new MemoryStream())
            {
                XmlSerializer s = new XmlSerializer(typeof(JobRequestMessage));
                s.Serialize(XmlWriter.Create(stream), this);
                stream.Flush();
                stream.Seek(0, SeekOrigin.Begin);
                StreamReader reader = new StreamReader(stream);
                result = reader.ReadToEnd();
            }

            return result;
        }

        public static async Task<JobRequestMessage> CreateJobRequestMessage(Guid DBBJobId)
        {
            throw new NotImplementedException();
        }
    }
}

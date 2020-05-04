using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Text;
using SC.Transcode.Service.Models;

namespace SC.Transcode.Service
{
    [ServiceContract]
    public interface IIONService
    {
        [OperationContract]
        void SubmitJob(JobRequestMessage request);

        [OperationContract]
        StatusResponse GetStatus(StatusRequest request);

        [OperationContract(IsOneWay = false)]
        void PostStatus(PostStatusRequest request);
    }
}

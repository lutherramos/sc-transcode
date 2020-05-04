using Refit;
using SC.Transcode.Service.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SC.Transcode.Service
{
    public interface IInjectorService
    {
        [Post("")]
        public Task SubmitJob([Body]TranscodePayload payload);
    }
}

using System.Net.Http;
using Seventh.Bot.Client.Models.QqLightClient;
using WebApiClient;
using WebApiClient.Attributes;

namespace Seventh.Bot.Client.Abstractions
{
    
    /// <summary>
    /// 
    /// </summary>
    [HttpHost("http://127.0.0.1:7776")]
    [TraceFilter(OutputTarget = OutputTarget.Console)]
    public interface IQqLightClient : IHttpApi
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="invokeModel"></param>
        /// <returns></returns>
        [HttpPost("/httpAPI")]
        ITask<HttpResponseMessage> Invoke([JsonContent]InvokeModel invokeModel);
    }
}
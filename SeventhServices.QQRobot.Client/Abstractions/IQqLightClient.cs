using System.Net.Http;
using SeventhServices.QQRobot.Client.Attributes;
using SeventhServices.QQRobot.Client.Models;
using SeventhServices.QQRobot.Client.Models.MahuaClient;
using SeventhServices.QQRobot.Client.Models.QqLightClient;
using WebApiClient;
using WebApiClient.Attributes;

namespace SeventhServices.QQRobot.Client.Abstractions
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
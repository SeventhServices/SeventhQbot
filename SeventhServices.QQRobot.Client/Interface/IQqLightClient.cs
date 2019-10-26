using System.Net.Http;
using SeventhServices.QQRobot.Client.Attributes;
using SeventhServices.QQRobot.Client.Models;
using WebApiClient;
using WebApiClient.Attributes;

namespace SeventhServices.QQRobot.Client.Interface
{
    
    [HttpHost("http://127.0.0.1:36524")]
    [TraceFilter(OutputTarget = OutputTarget.Console)]
    public interface IQqLightClient : IHttpApi
    {

        [HttpPost("/api/v1/QQLight/Api_SendMsg")]
        ITask<HttpResponseMessage> SendMsgAsync ([JsonPatchContext] SendMsgRequest sendMsgRequest);

        [HttpPost("/api/v1/QQLight/Api_GetNick")]
        ITask<HttpResponseMessage> GetNickAsync([JsonPatchContext] GetNikeRequest sendMsgRequest);
    }
}
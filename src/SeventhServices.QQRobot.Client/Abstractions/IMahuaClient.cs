using System;
using System.Net.Http;
using SeventhServices.QQRobot.Client.Attributes;
using SeventhServices.QQRobot.Client.Models;
using SeventhServices.QQRobot.Client.Models.MahuaClient;
using WebApiClient;
using WebApiClient.Attributes;

namespace SeventhServices.QQRobot.Client.Abstractions
{
    /// <summary>
    /// 
    /// </summary>
    [Obsolete]
    public interface IMahuaClient : IHttpApi
    {
        [HttpPost("/api/v1/Mpq/Api_SendMsg")]
        ITask<HttpResponseMessage> SendMsgForMpqAsync([JsonPatchContext] SendMsgRequest sendMsgRequest);

        [HttpPost("/api/v1/QQLight/Api_SendMsg")]
        ITask<HttpResponseMessage> SendMsgAsync([JsonPatchContext] SendMsgRequest sendMsgRequest);

        [HttpPost("/api/v1/QQLight/Api_GetNick")]
        ITask<HttpResponseMessage> GetNickAsync([JsonPatchContext] GetNikeRequest sendMsgRequest);

        [HttpPost("/api/v1/QQLight/Api_UpLoadPic")]
        ITask<HttpResponseMessage> UploadPicAsync([JsonPatchContext] UploadPicRequest uploadPicRequest);
    }
}
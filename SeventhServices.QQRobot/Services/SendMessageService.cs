using System.ComponentModel;
using System.Net.Http;
using System.Threading.Tasks;
using SeventhServices.QQRobot.Client.Abstractions;
using SeventhServices.QQRobot.Client.Enums;
using SeventhServices.QQRobot.Client.Models;
using SeventhServices.QQRobot.Models;

namespace SeventhServices.QQRobot.Services
{
    /// <summary>
    /// 
    /// </summary>
    public class SendMessageService
    {
        private readonly IQqLightClient _qqLightClient;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="qqLightClient"></param>
        public SendMessageService(IQqLightClient qqLightClient)
        {
            _qqLightClient = qqLightClient;
        }

        public async Task<SendResult> SendAsync(string message ,string qq, string group, MsgType msgType )
        {
            if (msgType == MsgType.Group)
            {
                return await SendToGroupAsync(message, group).ConfigureAwait(false);
            }

            var response = await _qqLightClient.SendMsgAsync(new SendMsgRequest
            {
                ReceiveQq = qq,
                MsgType = msgType,
                Message = ProcessMessageUtils.FilterReceivePic(message)
            });

            return new SendResult
            {
                IsSuccess = response.IsSuccessStatusCode,
                Message = message,
                MsgType = msgType,
                Qq = qq
            };
        }

        public async Task<SendResult> SendToFriendAsync(string message, string qq = RobotOptions.MasterQq)
        {
            var response = await _qqLightClient.SendMsgAsync(new SendMsgRequest
            {
                ReceiveQq = qq,
                MsgType = MsgType.Friend,
                Message = ProcessMessageUtils.FilterReceivePic(message)
            });

            return new SendResult
            {
                IsSuccess = response.IsSuccessStatusCode,
                Message = message,
                MsgType = MsgType.Friend,
                Qq = qq
            };
        }

        public async Task<SendResult> SendToGroupAsync(string message, string group = RobotOptions.ServeGroup)
        {
            var response = await _qqLightClient.SendMsgAsync(new SendMsgRequest
            {
                ReceiveGroup = group,
                MsgType = MsgType.Group,
                Message = ProcessMessageUtils.FilterReceivePic(message)
            });

            return new SendResult
            {
                IsSuccess = response.IsSuccessStatusCode,
                Message = message,
                MsgType = MsgType.Group,
                Qq = group
            };
        }
    }
}
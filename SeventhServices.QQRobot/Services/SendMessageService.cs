using System.ComponentModel;
using System.Net.Http;
using System.Threading.Tasks;
using SeventhServices.QQRobot.Client.Enums;
using SeventhServices.QQRobot.Client.Interface;
using SeventhServices.QQRobot.Client.Models;
using SeventhServices.QQRobot.Models;

namespace SeventhServices.QQRobot.Services
{
    public class SendMessageService
    {
        private readonly IQqLightClient _qqLightClient;


        public SendMessageService(IQqLightClient qqLightClient)
        {
            _qqLightClient = qqLightClient;
        }

        public async Task<SendResult> SendAsync(string message ,string qq, MsgType msgType )
        {
            if (msgType == MsgType.Group)
            {
                return await SendToGroupAsync(message, qq).ConfigureAwait(false);
            }

            var response = await _qqLightClient.SendMsgAsync(new SendMsgRequest
            {
                ReceiveQq = qq,
                MsgType = msgType,
                Message = Util.FilterPic(message)
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
                Message = Util.FilterPic(message)
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
                Message = Util.FilterPic(message)
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
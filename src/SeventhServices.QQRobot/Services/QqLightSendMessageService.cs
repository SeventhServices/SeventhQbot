using System.Net.Http;
using System.Threading.Tasks;
using SeventhServices.QQRobot.Abstractions.Services;
using SeventhServices.QQRobot.Client.Abstractions;
using SeventhServices.QQRobot.Client.Enums;
using SeventhServices.QQRobot.Client.Extensions;
using SeventhServices.QQRobot.Client.Models.MahuaClient;
using SeventhServices.QQRobot.Client.Models.QqLightClient;
using SeventhServices.QQRobot.Models;
using static SeventhServices.QQRobot.Utils.ProcessMessageUtils;

namespace SeventhServices.QQRobot.Services
{
    public class QqLightSendMessageService : ISendMessageService
    {
        private readonly IQqLightClient _qqLightClient;

        public QqLightSendMessageService(IQqLightClient qqLightClient)
        {
            _qqLightClient = qqLightClient;
        }
        public async Task<SendResult> SendAsync(string message, string qq,
            string @group, MsgType msgType)
        {
            //filter special unicode.
            message = FilterSendMessage(FilterReceivePic(message));

            var response = await _qqLightClient.Invoke(new InvokeModel
            {
                api = msgType.ToQqLightApiName(),
                Qq = qq,
                Group = @group,
                Message = message
            });


            return new SendResult
            {
                IsSuccess = response.IsSuccessStatusCode,
                Message = message,
                Group = group,
                MsgType = msgType,
                Qq = qq
            };
        }

        public async Task<SendResult> SendToFriendAsync(string message, string qq = RobotOptions.MasterQq)
        {
            return await SendAsync(message, qq, null, MsgType.Friend)
                .ConfigureAwait(false);
        }

        public async Task<SendResult> SendToGroupAsync(string message, string group = RobotOptions.ServeGroup)
        {
            return await SendAsync(message, null, group, MsgType.Group)
                .ConfigureAwait(false);
        }
    } 
}
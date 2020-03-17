using System.Threading.Tasks;
using Seventh.Bot.Abstractions.Services;
using Seventh.Bot.Client.Abstractions;
using Seventh.Bot.Client.Enums;
using Seventh.Bot.Client.Extensions;
using Seventh.Bot.Client.Models.QqLightClient;
using Seventh.Bot.Models;
using static Seventh.Bot.Utils.ProcessMessageUtils;

namespace Seventh.Bot.Services
{
    public class QqLightSendMessageService : ISendMessageService
    {
        private readonly IQqLightClient _qqLightClient;

        public QqLightSendMessageService(IQqLightClient qqLightClient)
        {
            _qqLightClient = qqLightClient;
        }
        public async Task<SendResult> SendAsync(string message, string qq,
            string group, MsgType msgType)
        {
            //filter special unicode.
            message = FilterSendMessage(FilterReceiveMessage(message));

            var response = await _qqLightClient.Invoke(new InvokeModel
            {
                api = msgType.ToQqLightApiName(),
                Qq = qq,
                Group = group,
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
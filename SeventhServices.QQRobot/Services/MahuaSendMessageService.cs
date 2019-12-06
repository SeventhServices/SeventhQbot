using System;
using System.ComponentModel;
using System.Net.Http;
using System.Threading.Tasks;
using SeventhServices.QQRobot.Abstractions.Services;
using SeventhServices.QQRobot.Client.Abstractions;
using SeventhServices.QQRobot.Client.Enums;
using SeventhServices.QQRobot.Client.Models;
using SeventhServices.QQRobot.Client.Models.MahuaClient;
using SeventhServices.QQRobot.Models;
using SeventhServices.QQRobot.Utils;
using static SeventhServices.QQRobot.Utils.ProcessMessageUtils;

namespace SeventhServices.QQRobot.Services
{
    /// <summary>
    /// Mahua send message service for qqLight
    /// </summary>
    [Obsolete("mahua not support qqLight v3.0+")]
    public class SendMessageService : ISendMessageService
    {
        private readonly IMahuaClient _qqLightClient;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="qqLightClient"></param>
        public SendMessageService(IMahuaClient qqLightClient)
        {
            _qqLightClient = qqLightClient;
        }

        public async Task<SendResult> SendAsync(string message ,string qq, string group, MsgType msgType )
        {
            HttpResponseMessage response;

            //filter special unicode.
            message = FilterSendMessage(FilterReceivePic(message));

            if (msgType == MsgType.Group)
            {
                response = await _qqLightClient.SendMsgAsync(new SendMsgRequest
                {
                    ReceiveGroup = group,
                    MsgType = msgType,
                    Message = message
                });
            }

            response = await _qqLightClient.SendMsgAsync(new SendMsgRequest
            {
                ReceiveQq = qq,
                MsgType = msgType,
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
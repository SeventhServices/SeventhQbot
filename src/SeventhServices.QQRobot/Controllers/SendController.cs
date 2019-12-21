using System;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SeventhServices.QQRobot.Abstractions.Services;
using SeventhServices.QQRobot.Client.Abstractions;
using SeventhServices.QQRobot.Client.Enums;
using SeventhServices.QQRobot.Client.Models;
using SeventhServices.QQRobot.Client.Models.MahuaClient;
using SeventhServices.QQRobot.Services;
using SeventhServices.QQRobot.Utils;

namespace SeventhServices.QQRobot.Controllers
{
    [Route("Message")]
    public class SendController : Controller
    {
        private readonly ISendMessageService _sendMessageService;

        public SendController(ISendMessageService sendMessageService)
        {
            _sendMessageService = sendMessageService;
        }


        [HttpGet("Text")]
        public async Task<IActionResult> Text(string context, string qq = RobotOptions.MasterQq,
            string group = RobotOptions.TestGroup, MsgType msgType = MsgType.Friend )
        {
            var sendResult = await _sendMessageService.SendAsync(
                    context,qq,group,msgType)
                .ConfigureAwait(false);
            return Ok(sendResult);
        }

        [HttpGet("Pic")]
        public async Task<IActionResult> Pic(Uri uri, string qq = RobotOptions.MasterQq, 
            string group = RobotOptions.TestGroup,MsgType msgType = MsgType.Friend)
        {
            var sendResult = await _sendMessageService.SendAsync(
                    ProcessMessageUtils.FilterSendPic(uri), qq, group , msgType)
                .ConfigureAwait(false);
            return Ok(sendResult);
        }

    }
}
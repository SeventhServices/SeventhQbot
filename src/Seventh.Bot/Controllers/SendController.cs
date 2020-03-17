using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Seventh.Bot.Abstractions.Services;
using Seventh.Bot.Client.Enums;
using Seventh.Bot.Utils;

namespace Seventh.Bot.Controllers
{
    [Route("message")]
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

        [HttpGet("pic")]
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
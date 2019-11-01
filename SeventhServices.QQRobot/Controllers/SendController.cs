using System;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SeventhServices.QQRobot.Client.Abstractions;
using SeventhServices.QQRobot.Client.Enums;
using SeventhServices.QQRobot.Client.Models;
using SeventhServices.QQRobot.Services;

namespace SeventhServices.QQRobot.Controllers
{
    [Route("Message")]
    public class SendController : Controller
    {
        private readonly SendMessageService _sendMessageService;
        private readonly IQqLightClient _qqLightClient;

        public SendController(SendMessageService sendMessageService,IQqLightClient qqLightClient)
        {
            _sendMessageService = sendMessageService;
            _qqLightClient = qqLightClient;
        }

        [HttpGet]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> Index()
        {
            await _qqLightClient.GetNickAsync(new GetNikeRequest {Qq = RobotOptions.MasterQq}).ConfigureAwait(true);
            var sendResult = await _sendMessageService.SendToFriendAsync(@"").ConfigureAwait(false);
            return Ok(sendResult);
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
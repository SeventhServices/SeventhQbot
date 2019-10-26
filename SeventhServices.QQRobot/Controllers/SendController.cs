using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SeventhServices.QQRobot.Client.Enums;
using SeventhServices.QQRobot.Client.Interface;
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

        public async Task<IActionResult> Index()
        {
            //var response = await _qqLightClient.GetNickAsync(new GetNikeRequest {Qq = RobotOptions.MasterQq}).ConfigureAwait(true);
            
            var sendResult = await _sendMessageService.SendToFriendAsync("").ConfigureAwait(false);
            return Ok(sendResult);
        }

        [HttpPost]
        public async Task<IActionResult> Text(string context, string qq = RobotOptions.MasterQq, MsgType msgType = MsgType.Friend )
        {
            var sendResult = await _sendMessageService.SendAsync(context,qq,msgType)
                .ConfigureAwait(false);
            return Ok(sendResult);
        }
    }
}
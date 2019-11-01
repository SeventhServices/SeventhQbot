using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SeventhServices.QQRobot.Abstractions;
using SeventhServices.QQRobot.Client.Enums;
using SeventhServices.QQRobot.Client.Models;
using SeventhServices.QQRobot.Models;
using SeventhServices.QQRobot.Services;

namespace SeventhServices.QQRobot.Controllers
{
    [ApiController]
    [Route("api/ReceiveMahuaOutput")]
    public class ReceiveController : ControllerBase
    {
        private readonly IMessagePipeline _messagePipeline;
        private readonly SendMessageService _sendMessage;

        public ReceiveController(IMessagePipeline messagePipeline,SendMessageService sendMessage)
        {
            _messagePipeline = messagePipeline;
            _sendMessage = sendMessage;
        }

        [HttpPost]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> Index([FromBody] BotReceive botReceive)
        {
            if (botReceive?.Result != null)
            {
                await _sendMessage.SendToFriendAsync(botReceive.Result)
                    .ConfigureAwait(false);
                return Ok();
            }

            if (botReceive?.Message == null)
            {
                return Ok();
            }

            await _messagePipeline.Pocess(botReceive).ConfigureAwait(false);

            return Ok();
        }

    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SeventhServices.QQRobot.Abstractions;
using SeventhServices.QQRobot.Abstractions.Services;
using SeventhServices.QQRobot.Client.Enums;
using SeventhServices.QQRobot.Client.Models;
using SeventhServices.QQRobot.Models;
using SeventhServices.QQRobot.Services;

namespace SeventhServices.QQRobot.Controllers
{
    [ApiController]
    
    public class ReceiveController : ControllerBase
    {
        private readonly IMessagePipeline _messagePipeline;
        private readonly ISendMessageService _sendMessage;


        public ReceiveController(IMessagePipeline messagePipeline,ISendMessageService sendMessage)
        {
            _messagePipeline = messagePipeline;
            _sendMessage = sendMessage;
        }

        [HttpPost]
        [ApiExplorerSettings(IgnoreApi = true)]
        [Route("api/ReceiveMahuaOutput")]
        public async Task<IActionResult> ReceiveMahuaOutput([FromBody] BotReceive botReceive)
        {
            if (botReceive?.Result != null)
            {
                await _sendMessage.SendToFriendAsync(botReceive.Result,RobotOptions.MasterQq)
                    .ConfigureAwait(false);
                return Ok();
            }

            if (botReceive?.Message == null)
            {
                return Ok();
            }

            await _messagePipeline.Pocess(botReceive.Message,botReceive.FromQq,
                botReceive.FromGroup,botReceive.Type).ConfigureAwait(false);

            return Ok();
        }

        [HttpPost]
        [ApiExplorerSettings(IgnoreApi = true)]
        [Route("api/ReceiveQqLightOutput")]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<IActionResult> ReceiveWebApiOutput([FromForm] IFormCollection form)
        {
            var botReceive = JsonSerializer.Deserialize<QqLightBotReceive>(form?.Keys.First());

            if (botReceive.Msg == null)
            {
                return Ok();
            }

            await _messagePipeline.Pocess(botReceive.Msg,botReceive.QQID
            ,botReceive.GroupID,botReceive.Type).ConfigureAwait(false);

            return Ok();
        }

    }
}
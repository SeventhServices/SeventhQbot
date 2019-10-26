using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SeventhServices.QQRobot.Client.Enums;
using SeventhServices.QQRobot.Client.Interface;
using SeventhServices.QQRobot.Client.Models;
using SeventhServices.QQRobot.Models;
using SeventhServices.QQRobot.Services;

namespace SeventhServices.QQRobot.Controllers
{
    [ApiController]
    [Route("api/ReceiveMahuaOutput")]
    public class ReceiveController : ControllerBase
    {
        private readonly RandomRepeat _randomRepeat;

        public ReceiveController(RandomRepeat randomRepeat)
        {
            _randomRepeat = randomRepeat;
        }

        [HttpPost]
        public async Task<IActionResult> Index([FromBody] Receive receive , [FromServices] IQqLightClient myPcQqClient)
        {
            if (receive != null && receive.Message == null) return Ok();

            await _randomRepeat.SetGroup(RobotOptions.TestGroup, 
                0.1F,receive).ConfigureAwait(false);

            await _randomRepeat.Set(() 
                    => receive != null 
                       && (receive.Type == MsgType.Friend
                        || receive.Type == MsgType.TemporarilyGroup)
                , 0.5F, receive).ConfigureAwait(false);

            return Ok();
        }
    }
}
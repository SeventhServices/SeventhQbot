using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SeventhServices.QQRobot.Client.Enums;
using SeventhServices.QQRobot.Models;

namespace SeventhServices.QQRobot.Services
{
    public class RandomRepeat
    {
        private readonly RandomService _randomService;
        private readonly SendMessageService _sendMessageService;

        public RandomRepeat(RandomService randomService, SendMessageService sendMessageService)
        {
            _randomService = randomService;
            _sendMessageService = sendMessageService;
        }

        public async Task Set(Func<bool> predicate, float probability, BotReceive botReceive)
        {
            if (predicate != null && predicate())
            {
                if (_randomService.RandomBool(probability))
                {
                    await _sendMessageService.SendAsync(
                        botReceive.Message,
                        botReceive.FromQq,
                        botReceive.FromGroup,
                        botReceive.Type)
                        .ConfigureAwait(false);
                }
            }
        }



        public async Task SetGroup(string group, float probability, BotReceive botReceive)
        {
            if (botReceive != null && botReceive.FromGroup == @group)
            {
                if (_randomService.RandomBool(probability))
                {
                    await _sendMessageService.SendToGroupAsync(
                        botReceive.Message,
                        botReceive.FromGroup).ConfigureAwait(false);
                }
            }
        }

        public async Task SetQq(string qq, float probability, BotReceive botReceive)
        {
            if (botReceive != null && botReceive.FromQq == qq)
            {
                if (_randomService.RandomBool(probability))
                {
                    await _sendMessageService.SendAsync(
                        botReceive.Message,
                        botReceive.FromQq,
                        botReceive.FromGroup,
                        botReceive.Type).ConfigureAwait(false);
                }
            }
        }


    }
}
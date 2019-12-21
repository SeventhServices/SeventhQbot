using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SeventhServices.QQRobot.Abstractions.Services;
using SeventhServices.QQRobot.Client.Enums;
using SeventhServices.QQRobot.Models;

namespace SeventhServices.QQRobot.Services
{
    public class RandomRepeat
    {
        private readonly RandomService _randomService;
        private readonly ISendMessageService _sendMessageService;

        public RandomRepeat(RandomService randomService, ISendMessageService sendMessageService)
        {
            _randomService = randomService;
            _sendMessageService = sendMessageService;
        }

        public async Task Set(Func<bool> predicate, float probability,
            string message, string fromQq, string fromGroup, MsgType msgType)
        {
            if (predicate != null && predicate())
            {
                if (_randomService.RandomBool(probability))
                {
                    await _sendMessageService.SendAsync(message, fromQq, fromGroup, msgType)
                        .ConfigureAwait(false);
                }
            }
        }



        public async Task SetGroup(string group, float probability, string message, string fromGroup)
        {
            if (fromGroup == group)
            {
                if (_randomService.RandomBool(probability))
                {
                    await _sendMessageService.SendToGroupAsync(
                        message,
                        fromGroup).ConfigureAwait(false);
                }
            }
        }

        public async Task SetQq(string qq, float probability, 
            string message, string fromQq, string fromGroup, MsgType msgType)
        {
            if (fromQq == qq)
            {
                if (_randomService.RandomBool(probability))
                {
                    await _sendMessageService.SendAsync(message, fromQq, fromGroup, msgType)
                        .ConfigureAwait(false);
                }
            }
        }


    }
}
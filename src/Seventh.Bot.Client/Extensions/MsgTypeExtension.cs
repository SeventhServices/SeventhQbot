using System;
using Seventh.Bot.Client.Enums;

namespace Seventh.Bot.Client.Extensions
{
    public static class MsgTypeExtension
    {
        public static string ToQqLightApiName(this MsgType msgType)
        {
            return msgType switch
            {
                MsgType.Friend => "发送好友消息",
                MsgType.Group => "发送群消息",
                _ => throw new ArgumentOutOfRangeException(nameof(msgType), msgType, null)
            };
        }
    }
}
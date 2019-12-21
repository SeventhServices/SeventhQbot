using System;
using System.Text.Json.Serialization;
using SeventhServices.QQRobot.Client.Enums;

namespace SeventhServices.QQRobot.Models
{
    public class BotReceive : BotActionResult
    {
        public string Message { get; set; }
        public MsgType Type { get; set; }
        [JsonPropertyName("Fromgroup")] 
        public string FromGroup { get; set; }
        [JsonPropertyName("Fromqq")] 
        public string FromQq { get; set; }
        public string MessageId { get; set; }
    }
}
using Seventh.Bot.Client.Enums;
using System.Text.Json.Serialization;

namespace Seventh.Bot.Models.MahuaModels
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
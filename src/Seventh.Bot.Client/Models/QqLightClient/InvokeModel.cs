using System.Text.Json.Serialization;

namespace Seventh.Bot.Client.Models.QqLightClient
{
    public class InvokeModel
    {
        public string api { get; set; }

        [JsonPropertyName("QQ号")]
        public string Qq { get; set; }
        [JsonPropertyName("群号")]
        public string Group { get; set; }
        [JsonPropertyName("消息")]
        public string Message { get; set; }
    }
}
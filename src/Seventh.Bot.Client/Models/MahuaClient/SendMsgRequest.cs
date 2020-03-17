using System.Text.Json.Serialization;
using Seventh.Bot.Client.Enums;

namespace Seventh.Bot.Client.Models.MahuaClient
{

    public class SendMsgRequest
    {
        [JsonIgnore]
        public string TypeCode { get; set; } = "Api_SendMsg";

        [JsonPropertyName("类型")]
        public MsgType MsgType { get; set; }

        [JsonPropertyName("群组")] 
        public string ReceiveGroup { get; set; } = "";

        [JsonPropertyName("qQ号")] 
        public string ReceiveQq { get; set; } = "";

        [JsonPropertyName("内容")] 
        public string Message { get; set; } = "";

    }
}
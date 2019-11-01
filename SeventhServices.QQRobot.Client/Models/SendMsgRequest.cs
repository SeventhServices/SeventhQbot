using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using SeventhServices.QQRobot.Client.Enums;
using WebApiClient.DataAnnotations;

namespace SeventhServices.QQRobot.Client.Models
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
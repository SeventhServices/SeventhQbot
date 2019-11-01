using System.Text.Json.Serialization;

namespace SeventhServices.QQRobot.Client.Models
{
    public class UploadPicRequest
    {
        [JsonIgnore]
        public string TypeCode { get; set; } = "Api_UploadPic";

        [JsonPropertyName("类型")]
        public int Type { get; set; }

        [JsonPropertyName("对象")]
        public string Qq { get; set; }

        [JsonPropertyName("字节集")]
        public string PicHex { get; set; }
    }
}
using System.Text.Json.Serialization;

namespace SeventhServices.QQRobot.Client.Models.MahuaClient
{
    public class GetNikeRequest
    {
        [JsonIgnore]
        public string TypeCode { get; set; } = "Api_GetNick";

        [JsonPropertyName("qQ号")]
        public string Qq { get; set; } = "";

    }
}

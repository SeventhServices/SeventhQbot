using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace SeventhServices.QQRobot.Client.Models
{
    public class GetNikeRequest
    {
        [JsonIgnore]
        public string TypeCode { get; set; } = "Api_GetNick";

        [JsonPropertyName("qQ号")]
        public string Qq { get; set; } = "";

    }
}

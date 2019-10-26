using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace SeventhServices.QQRobot.Client.Models
{
    public class GetNikeRequest
    {
        [JsonPropertyName("qQ号")]
        public string Qq { get; set; } = "";

    }
}

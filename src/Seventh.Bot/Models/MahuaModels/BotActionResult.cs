using System;

namespace Seventh.Bot.Models.MahuaModels
{
    public class BotActionResult
    {
        public string Result { get; set; }
        public string TypeCode { get; set; }
        public int Platform { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
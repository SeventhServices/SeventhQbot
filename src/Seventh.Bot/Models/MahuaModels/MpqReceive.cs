using System;

namespace Seventh.Bot.Models.MahuaModels
{
    public class MpqReceive
    {
        public string TypeCode { get; set; }
        public string ReceiverQq { get; set; }
        public int EventAdditionType { get; set; }
        public string EventOperator { get; set; }
        public int EventType { get; set; }
        public string FromNum { get; set; }
        public string Message { get; set; }
        public string RawMessage { get; set; }
        public string Triggee { get; set; }
        public int Platform { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
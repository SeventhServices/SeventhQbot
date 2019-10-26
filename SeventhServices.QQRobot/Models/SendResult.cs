using SeventhServices.QQRobot.Client.Enums;

namespace SeventhServices.QQRobot.Models
{
    public class SendResult
    {
        public bool IsSuccess { get; set; }
        public string Qq { get; set; }
        public MsgType MsgType { get; set; }
        public string MsgTypeName { get; set; } = nameof(MsgType);
        public string Message { get; set; }
    }
}
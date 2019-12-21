using SeventhServices.QQRobot.Client.Enums;

namespace SeventhServices.QQRobot.Models
{
    public class QqLightBotReceive
    {
        public string MsgID { get; set; }
        public string Event { get; set; }
        public string QQ { get; set; }
        public MsgType Type { get; set; }
        public string GroupID { get; set; }
        public string Msg { get; set; }
        public string QQID { get; set; }
        public string Name { get; set; }
        public string Explain { get; set; }
    }
}
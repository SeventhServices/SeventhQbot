using SeventhServices.QQRobot.Client.Enums;
using WebApiClient.DataAnnotations;

namespace SeventhServices.QQRobot.Client.Models
{
    public class MpqSendMsgRequest
    {

        [AliasAs("响应的QQ")]
        public string SendQq { get; set; }

        [AliasAs("信息类型")]
        public MsgType MsgType { get; set; }

        [AliasAs("参考子类型")]
        public int ReferenceType { get; set; } = 0;

        [AliasAs("收信群_讨论组")]
        public string ReceiveGroup { get; set; }

        [AliasAs("收信对象")]
        public string ReceiveQq { get; set; }

        [AliasAs("内容")]
        public string Message { get; set; }

    }
}
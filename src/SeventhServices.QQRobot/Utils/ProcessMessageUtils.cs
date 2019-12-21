using System;

namespace SeventhServices.QQRobot.Utils
{
    public static class ProcessMessageUtils
    {
        public static string LargeCardPic(int id)
        {
            return FilterSendPic($"http://qbot.sagilio.net:65321/Card/l/card_l_{id:D5}.jpg");
        }

        public static string FilterReceivePic(string message)
        {
            return message?.Replace("\u0003", null,
                StringComparison.Ordinal);
        }

        public static string FilterSendMessage(string message)
        {
            return message?.Replace("\u00A0", "\u0020",
                StringComparison.Ordinal);
        }

        public static string FilterSendPic(string path)
        {
            return $"[QQ:pic={path}]";
        }

        public static string FilterSendPic(Uri uri)
        {
            return $"[QQ:pic={uri}]";
        }
    }
}
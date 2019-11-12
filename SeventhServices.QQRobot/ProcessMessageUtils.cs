using System;

namespace SeventhServices.QQRobot
{
    public static class ProcessMessageUtils
    {
        public static string FilterReceivePic(string message)
        {
            return message?.Replace("\u0003", null,
                StringComparison.CurrentCulture);
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
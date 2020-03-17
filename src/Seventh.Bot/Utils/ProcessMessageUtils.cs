using System;

namespace Seventh.Bot.Utils
{
    public static class ProcessMessageUtils
    {
        public static string LargeCardPic(int id)
        {
            return FilterSendPic($"https://resource.t7s.sagilio.net/asset/sorted/card/l/card_l_{id:D5}.jpg");
        }

        public static string FilterReceiveMessage(string message)
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
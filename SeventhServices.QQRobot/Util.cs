using System;

namespace SeventhServices.QQRobot
{
    public static class Util
    {
        public static string FilterPic(string message)
        {
            return message.Replace("\u0003", null,
                StringComparison.CurrentCulture);
        }
    }
}
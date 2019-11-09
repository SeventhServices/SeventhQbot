using System;
using System.Globalization;
using System.Text.RegularExpressions;
using SeventhServices.QQRobot.Classes;
using SeventhServices.QQRobot.Commands;

namespace SeventhServices.QQRobot.Services
{
    public class MessageParser
    {
        private readonly Regex intRegex = new Regex("[1-9]\\d*");



        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public MessageCommand Parse(string message)
        {
            if (message == null)
            {
                return null;
            }
            if (!message.StartsWith(RobotOptions.Command,StringComparison.CurrentCulture))
            {
                return null;
            }

            if (message.Contains("卡",StringComparison.CurrentCulture))
            {
                var match = intRegex.Match(message);

                if (match.Success)
                {
                    return new CardCommand
                    {
                        CardId = int.Parse(match.Value, CultureInfo.CurrentCulture)
                    };
                }

                return new CardCommand
                {
                    ReturnMessage = "www"
                };

            }


            return new DefaultCommand{ReturnMessage = string.Concat(
                message.Replace("吗",null,StringComparison.CurrentCulture),"!")};

        }
    }
}
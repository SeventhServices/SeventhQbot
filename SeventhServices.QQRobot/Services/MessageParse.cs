using System;
using System.Globalization;
using System.Text.RegularExpressions;
using Microsoft.VisualBasic.CompilerServices;
using SeventhServices.Asset.LocalDB.Classes;
using SeventhServices.QQRobot.Abstractions;
using SeventhServices.QQRobot.Classes;
using SeventhServices.QQRobot.Commands;

namespace SeventhServices.QQRobot.Services
{
    public class MessageParser
    {
        private readonly IRepository<Card> _cardRepository;
        private readonly IRepository<Character> _characterRepository;
        private readonly Regex intRegex = new Regex("[1-9]\\d*");


        public MessageParser(
            IRepository<Card> cardRepository,
            IRepository<Character> characterRepository)
        {
            _cardRepository = cardRepository;
            _characterRepository = characterRepository;
        }
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
            if (!message.StartsWith(RobotOptions.Command, StringComparison.CurrentCulture))
            {
                return null;
            }

            message = message.TrimStart(RobotOptions.Command.ToCharArray());

            if (message.StartsWith("查", StringComparison.CurrentCulture))
            {
                message = message.TrimStart('查');
                var match = intRegex.Match(message);
                var id = 0;
                if (match.Success)
                {
                    id = int.Parse(match.Value, CultureInfo.CurrentCulture);
                }

                if (message.StartsWith("卡数据", StringComparison.CurrentCulture))
                {
                    return new CardCommand
                    {
                        CardId = id,
                        ReturnMessage = _cardRepository.GetById(id).ToString()
                    };
                }
                if (message.StartsWith("卡图", StringComparison.CurrentCulture))
                {
                    return new CardCommand
                    {
                        CardId = id,
                        ReturnMessage = ProcessMessageUtils.FilterSendPic($"http://qbot.sagilio.net:65321/Card/l/card_l_{id:D5}.jpg")
                    };
                }
                if (message.StartsWith("偶像", StringComparison.CurrentCulture))
                {
                    return new CardCommand
                    {
                        CardId = id,
                        ReturnMessage = _characterRepository.GetById(id).ToString()
                    };
                }
            }

            return new DefaultCommand
            {
                ReturnMessage = string.Concat(
                message.Replace("吗", null, StringComparison.CurrentCulture), "!")
            };
        }
    }
}
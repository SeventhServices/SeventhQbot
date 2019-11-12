using SeventhServices.QQRobot.Abstractions;
using SeventhServices.QQRobot.Models;
using System.Threading.Tasks;
using SeventhServices.Asset.LocalDB.Classes;
using SeventhServices.QQRobot.Classes;
using SeventhServices.QQRobot.Client.Enums;
using SeventhServices.QQRobot.Commands;

namespace SeventhServices.QQRobot.Services
{
    public class SimpleReturnPipeline : IMessagePipeline
    {
        private readonly RandomRepeat _randomRepeat;
        private readonly MessageParser _messageParser;
        private readonly SendMessageService _sendMessage;
        private readonly IRepository<Card> _cardRepository;

        public SimpleReturnPipeline(RandomRepeat randomRepeat,
            MessageParser messageParser,
            SendMessageService sendMessage,
            IRepository<Card> cardRepository
            )
        {
            _randomRepeat = randomRepeat;
            _messageParser = messageParser;
            _sendMessage = sendMessage;
            _cardRepository = cardRepository;
        }

        public async Task Pocess(BotReceive receive)
        {
            if (receive == null)
            {
                return;
            }

            var command = _messageParser.Parse(receive.Message);

            if (command == null)
            { 
                 await Random(receive).ConfigureAwait(false);
                 return;
            }

            //switch (command)
            //{
            //    case CardCommand c:
            //        c.ReturnMessage = _cardRepository.GetById(c.CardId).ToString();
            //        break;
            //}

            await _sendMessage.SendAsync(command?.ReturnMessage, 
                 receive.FromQq, receive.FromGroup, receive.Type)
                 .ConfigureAwait(false);
        }





        private async Task Random(BotReceive receive)
        {
            await _randomRepeat.SetGroup(RobotOptions.TestGroup,
                0.05F, receive).ConfigureAwait(false);

            await _randomRepeat.SetGroup(RobotOptions.ServeGroup,
                0.01F, receive).ConfigureAwait(false);

            await _randomRepeat.Set(()
                    => (receive.Type == MsgType.Friend
                        || receive.Type == MsgType.TemporarilyGroup)
                , 0.5F, receive).ConfigureAwait(false);
        }
    }
}
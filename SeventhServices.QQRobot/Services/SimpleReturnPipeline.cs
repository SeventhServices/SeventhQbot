using SeventhServices.QQRobot.Abstractions;
using SeventhServices.QQRobot.Models;
using System.Threading.Tasks;
using SeventhServices.Asset.LocalDB.Classes;
using SeventhServices.QQRobot.Client.Enums;
namespace SeventhServices.QQRobot.Services
{
    public class SimpleReturnPipeline : IMessagePipeline
    {
        private readonly RandomRepeat _randomRepeat;
        private readonly MessageParser _messageParser;
        private readonly SendMessageService _sendMessage;

        public SimpleReturnPipeline(RandomRepeat randomRepeat,
            MessageParser messageParser,
            SendMessageService sendMessage
        )
        {
            _randomRepeat = randomRepeat;
            _messageParser = messageParser;
            _sendMessage = sendMessage;
        }

        public async Task Pocess(BotReceive receive)
        {
            if (receive == null)
            {
                return;
            }

            var command = _messageParser.Parse(receive.Message, receive.FromQq);

            if (command == null)
            { 
                 await Random(receive).ConfigureAwait(false);
                 return;
            }

            foreach (var message in command.ReturnMessage)
            {
                await _sendMessage.SendAsync(message,
                        receive.FromQq, receive.FromGroup, receive.Type)
                    .ConfigureAwait(false);
            }

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
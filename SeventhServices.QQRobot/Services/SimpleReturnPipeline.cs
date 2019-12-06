using SeventhServices.QQRobot.Abstractions;
using SeventhServices.QQRobot.Models;
using System.Threading.Tasks;
using SeventhServices.Asset.LocalDB.Classes;
using SeventhServices.Client.Common.Params;
using SeventhServices.QQRobot.Client.Enums;
using SeventhServices.Asset.Services;
using SeventhServices.QQRobot.Abstractions.Services;

namespace SeventhServices.QQRobot.Services
{
    public class SimpleReturnPipeline : IMessagePipeline
    {
        private readonly RandomRepeat _randomRepeat;
        private readonly MessageParser _messageParser;
        private readonly ISendMessageService _sendMessage;

        public SimpleReturnPipeline(RandomRepeat randomRepeat,
            MessageParser messageParser,
            ISendMessageService sendMessage
        )
        {
            _randomRepeat = randomRepeat;
            _messageParser = messageParser;
            _sendMessage = sendMessage;
        }

        public async Task Pocess(string message, string fromQq, 
            string fromGroup, MsgType msgType)
        {
            var command = _messageParser.Parse(message, fromQq);

            if (command == null)
            { 
                 await Random(message, fromQq, fromGroup, msgType).ConfigureAwait(false);
                 return;
            }

            foreach (var returnMessage in command.ReturnMessage)
            {
                await _sendMessage.SendAsync(returnMessage, fromQq, fromGroup, msgType)
                    .ConfigureAwait(false);
            }

        }

        private async Task Random(string message, string fromQq, 
            string fromGroup, MsgType msgType)
        {
            await _randomRepeat.SetGroup(RobotOptions.TestGroup,
                0.05F, message, fromGroup)
                .ConfigureAwait(false);

            await _randomRepeat.SetGroup(RobotOptions.ServeGroup,
                0.01F, message, fromGroup)
                .ConfigureAwait(false);

            await _randomRepeat.Set(()
                    => (msgType == MsgType.Friend
                        || msgType == MsgType.TemporarilyGroup)
                , 0.5F, message, fromQq, fromGroup, msgType)
                .ConfigureAwait(false);
        }
    }
}
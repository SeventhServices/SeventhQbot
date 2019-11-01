using SeventhServices.QQRobot.Abstractions;
using SeventhServices.QQRobot.Models;
using System.Threading.Tasks;
using SeventhServices.QQRobot.Client.Enums;

namespace SeventhServices.QQRobot.Services
{
    public class SimpleReturnPipeline : IMessagePipeline
    {
        private readonly RandomRepeat _randomRepeat;

        public SimpleReturnPipeline(RandomRepeat randomRepeat)
        {
            _randomRepeat = randomRepeat;
        }

        public async Task Pocess(BotReceive receive)
        {

            await _randomRepeat.SetGroup(RobotOptions.TestGroup,
                0.2F, receive).ConfigureAwait(false);

            await _randomRepeat.SetGroup(RobotOptions.ServeGroup,
                0.01F, receive).ConfigureAwait(false);

            await _randomRepeat.Set(()
                    => (receive.Type == MsgType.Friend
                        || receive.Type == MsgType.TemporarilyGroup)
                , 0.5F, receive).ConfigureAwait(false);

        }
    }
}
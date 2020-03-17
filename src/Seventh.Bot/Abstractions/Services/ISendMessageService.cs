using System.Threading.Tasks;
using Seventh.Bot.Client.Enums;
using Seventh.Bot.Models;

namespace Seventh.Bot.Abstractions.Services
{
    public interface ISendMessageService
    {
        public Task<SendResult> SendAsync(
            string message, string qq, string group, MsgType msgType);

        public Task<SendResult> SendToFriendAsync(
            string message, string qq);

        public Task<SendResult> SendToGroupAsync(
            string message, string group);
    }
}
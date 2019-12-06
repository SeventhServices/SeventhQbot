using System.Threading.Tasks;
using SeventhServices.QQRobot.Client.Enums;
using SeventhServices.QQRobot.Models;

namespace SeventhServices.QQRobot.Abstractions.Services
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
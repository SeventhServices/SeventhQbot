using System.Threading.Tasks;
using SeventhServices.QQRobot.Client.Enums;
using SeventhServices.QQRobot.Models;

namespace SeventhServices.QQRobot.Abstractions
{
    public interface IMessagePipeline
    {
        Task Pocess(string message, string fromQq, string fromGroup, MsgType msgType);
    }
}
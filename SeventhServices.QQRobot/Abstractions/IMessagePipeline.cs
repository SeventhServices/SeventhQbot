using System.Threading.Tasks;
using SeventhServices.QQRobot.Models;

namespace SeventhServices.QQRobot.Abstractions
{
    public interface IMessagePipeline
    {
        Task Pocess(BotReceive receive);
    }
}
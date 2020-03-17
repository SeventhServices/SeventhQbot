using Seventh.Bot.Client.Enums;
using System.Threading.Tasks;

namespace Seventh.Bot.Abstractions
{
    public interface IMessagePipeline
    {
        Task Pocess(string message, string fromQq, string fromGroup, MsgType msgType);
    }
}
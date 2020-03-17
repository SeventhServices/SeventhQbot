using Sagilio.Bot.Parser.Abstractions;
using Seventh.Bot.Resource.Entities;

namespace Seventh.Bot.Extensions
{
    public static class EntityExtension
    {
        public static bool CheckBinding(this AccountBinding binding, MessageCommand command, string qq)
        {

                if (binding?.BoundAccount == null)
                {
                    command?.ReturnMessage.Add($"{qq}未绑定，请私信团团pid/id(账号文件内)绑定哦\n" +
                                        "私信格式 [ 团团绑定 pid (这里填pid) uid (这里填id) ]");
                    return false;
                }
                if (binding.IsPidOnly)
                {
                    command?.ReturnMessage.Add($"{qq}为仅pid绑定，不能进行游戏操作，请私信团团pid/id(账号文件内)绑定哦\n" +
                                        "私信格式 [ 团团绑定 pid (这里填pid) uid (这里填id) ]");
                    return false;
                }

                return true;
        }
    }
}
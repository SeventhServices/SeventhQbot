using Microsoft.Extensions.DependencyInjection;
using Sagilio.Bot.Parser.Abstractions;

namespace Sagilio.Bot.Parser
{
    public static class ServiceExtension
    {
        public static IServiceCollection AddRobotCommandParser(this IServiceCollection services)
        {
            services.AddSingleton<ICommandParser,CommandParser>();
            return services;
        }



    }
}

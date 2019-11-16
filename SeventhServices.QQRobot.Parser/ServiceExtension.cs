using Microsoft.Extensions.DependencyInjection;
using System;
using System.ComponentModel.Design;
using System.Security;
using System.Xml.Serialization;
using SeventhServices.QQRobot.Parser.Abstractions;

namespace SeventhServices.QQRobot.Parser
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

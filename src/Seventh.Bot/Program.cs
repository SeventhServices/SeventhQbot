using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Seventh.Bot.Common;
using Seventh.Bot.Resource;
using SeventhServices.Resource.Common.Utilities;

namespace Seventh.Bot
{
    internal class Program
    {

        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            ConfigureWatcher.TryAddConfigure<RobotStatus>();

            CreateDbIfNotExists(host);
            host.Run();
        }

        private static void CreateDbIfNotExists(IHost host)
        {
            using var scope = host.Services.CreateScope();
            var services = scope.ServiceProvider;

            try
            {
                var context = services.GetRequiredService<QBotDbContext>();
                context.Database.EnsureCreated();
            }
            catch (Exception ex)
            {
                var logger = services.GetRequiredService<ILogger<Program>>();
                logger.LogError(ex, "An error occurred creating the DB.");
            }
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseUrls("http://*:65321");
                    webBuilder.UseStartup<Startup>();
                });
    }
}

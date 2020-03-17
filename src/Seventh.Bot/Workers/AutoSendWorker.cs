using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Seventh.Bot.Abstractions;
using Seventh.Bot.Client.Enums;

namespace Seventh.Bot.Workers
{
    public class AutoSendWorker : BackgroundService
    {
        private readonly ILogger<AutoSendWorker> _logger;
        private readonly IMessagePipeline _messagePipeline;

        public AutoSendWorker(
            ILogger<AutoSendWorker> logger,
            IMessagePipeline messagePipeline)
        {
            _logger = logger;
            _messagePipeline = messagePipeline;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    _logger.LogInformation("Checked update at: {time}", DateTimeOffset.Now);

                    await _messagePipeline.Pocess(
                        fromGroup : RobotOptions.ServeGroup,
                        fromQq : RobotOptions.MasterQq,
                        message : "团团检查更新",
                        msgType : MsgType.Friend
                    ).ConfigureAwait(false);
                }
                catch (Exception e)
                {
                    _logger.LogError(e.Message);
                }

                try
                {
                    _logger.LogInformation("Sent at: {time}", DateTimeOffset.Now);

                    await _messagePipeline.Pocess(
                        fromGroup: RobotOptions.ServeGroup,
                        fromQq: RobotOptions.MasterQq,
                        message: "团团查档线",
                        msgType: MsgType.Group
                    ).ConfigureAwait(false);

                }
                catch (Exception e)
                {
                    _logger.LogError(e.Message);
                }

                //await Task.Delay(TimeSpan.FromHours(2), stoppingToken)
                //    .ConfigureAwait(false);

                await Task.Delay(new TimeSpan(DateTime.Now.Date.AddHours(
                                                  DateTime.Now.Hour + 3).Ticks 
                                              - DateTime.Now.Ticks), stoppingToken)
                    .ConfigureAwait(false);
            }

        }
    }
}
using System;
using OCR;
using System.Threading.Tasks;
using ICM;
using Microsoft.Extensions.DependencyInjection;
using bot.Services;
using bot.Helpers;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.EventLog;

namespace bot
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            // Dependency Injection setup
            IServiceProvider serviceProvider = new ServiceCollection()
                .AddLogging(configure => configure
                    .AddConsole()
                    .AddEventLog(new EventLogSettings()
                    {
                        SourceName = "PokerBot",
                        LogName = "PokerBotLog",
                        Filter = (x, y) => y >= LogLevel.Information
                    })
                )
                .AddSingleton<IPokerBotService, PokerBotService>()
                .AddSingleton<IIcmService, IcmService>()
                .AddSingleton<IImageProcessor, ImageProcessor>()
                .AddSingleton<ISuitFinder, SuitFinder>()
                .AddSingleton<IBoardStateHelper, BoardStateHelper>()
                .AddSingleton<IBoardStateService, BoardStateService>()
                .AddSingleton<IScreenCaptureService, ScreenCaptureService>()
                .BuildServiceProvider();

            // Run the bot
            IPokerBotService pokerBotService = serviceProvider.GetService<IPokerBotService>();
            await pokerBotService.Run();
        }
    }
}

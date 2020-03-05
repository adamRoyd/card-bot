using System;
using OCR;
using System.Threading.Tasks;
using ICM;
using Microsoft.Extensions.DependencyInjection;
using bot.Services;
using bot.Helpers;
using Microsoft.Extensions.Logging;

namespace bot
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            // Dependency Injection setup
            IServiceProvider serviceProvider = new ServiceCollection()
                .AddSingleton<IPokerBotService, PokerBotService>()
                .AddSingleton<IIcmService, IcmService>()
                .AddSingleton<IImageProcessor, ImageProcessor>()
                .AddSingleton<ISuitFinder, SuitFinder>()
                .AddSingleton<IBoardStateHelper, BoardStateHelper>()
                .AddSingleton<IBoardStateService, BoardStateService>()
                .AddSingleton<IScreenCaptureService, ScreenCaptureService>()
                .BuildServiceProvider();

            var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder
                    .AddFilter("Microsoft", LogLevel.Warning)
                    .AddFilter("System", LogLevel.Warning)
                    .AddFilter("LoggingConsoleApp.Program", LogLevel.Debug)
                    .AddConsole()
                    .AddEventLog();
            });

            ILogger logger = loggerFactory.CreateLogger<Program>();
            logger.LogInformation("Example log message");

            // Run the bot
            IPokerBotService pokerBotService = serviceProvider.GetService<IPokerBotService>();
            await pokerBotService.Run();
        }
    }
}

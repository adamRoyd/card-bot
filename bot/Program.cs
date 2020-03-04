using System;
using System.IO;
using OCR;
using Engine.Models;
using System.Threading.Tasks;
using Engine.Enums;
using bot.Logging;
using System.Linq;
using ICM;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Extensions.DependencyInjection;
using System.Drawing.Imaging;
using bot.Services;

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

            // Run the bot
            IPokerBotService pokerBotService = serviceProvider.GetService<IPokerBotService>();
            await pokerBotService.Run();
        }
    }
}

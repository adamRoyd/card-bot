using Engine.Models;

namespace bot.Services
{
    public interface IHandHistoryService
    {
        Player[] GetPlayersFromHistory(string path);
    }
}
